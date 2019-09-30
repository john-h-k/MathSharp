using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

#nullable disable

namespace MathSharp.Interactive.Benchmarks.Vector.Single.MathSharpRayTracer
{
    using Vector = MathSharp.Vector;



    public class MathSharpRayTracer
    {
        private int _screenWidth;
        private int _screenHeight;

        public MathSharpRayTracer(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        public void Render(Scene scene)
        {
            for (int y = 0; y < _screenHeight; y++)
            {
                var recenterY = -(y - (_screenHeight / 2.0f)) / (2.0f * _screenHeight);

                for (int x = 0; x < _screenWidth; x++)
                {
                    var recenterX = (x - (_screenWidth / 2.0f)) / (2.0f * _screenWidth);

                    var forward = scene.Camera.Forward.Load();
                    var right = scene.Camera.Right.Load();
                    var up = scene.Camera.Up.Load();
                    var point = Vector.Normalize(forward +
                                                 ((right * recenterX) + (up * recenterY)));

                    var ray = new Ray { Start = scene.Camera.Pos };
                    point.Store(out ray.Dir);


                }
            }
        }

        public void ComputeTraceRay(Ray ray, Scene scene, int depth)
        {
            var sects = new List<>();
            foreach (ISect sect in scene.Intersect(ray))
            {
                if (sect is null) continue;
                
            }
        }

    }

    public static class Color
    {
        public static HwVector3S Make(float r, float g, float b)
        {
            return Vector128.Create(r, g, b, 0);
        }
    }

    public class Ray
    {
        public Vector3S Start;
        public Vector3S Dir;
    }

    public class ISect
    {
        public SceneObject Thing;
        public Ray Ray;
        public float Dist;
    }

    public abstract class Surface
    {
        public abstract HwVector3S Diffuse(HwVector3S vector);
        public abstract HwVector3S Specular(HwVector3S vector);
        public abstract float Reflect(HwVector3S vector);
        public double Roughness;
    }

    public class Shiny : Surface
    {
        public Shiny()
        {
            Roughness = 50;
        }

        public override HwVector3S Diffuse(HwVector3S vector)
            => Color.Make(1, 1, 1);

        public override HwVector3S Specular(HwVector3S vector)
            => Color.Make(0.5f, 0.5f, 0.5f);

        public override float Reflect(HwVector3S vector)
            => 0.6f;
    }

    public class CheckerBoard : Surface
    {
        public CheckerBoard()
        {
            Roughness = 150;
        }

        private static bool Calc(HwVector3S pos)
        {
            var mod2 = (Vector.Floor(pos) + (HwVector3S)Vector.Permute(Vector.Floor(pos), ShuffleValues._2_2_2_2)) % 2;
            var mask = Vector.MoveMask(mod2);
            return (mask & 0b1) != 0;
        }

        public override HwVector3S Diffuse(HwVector3S pos)
        {
            return Calc(pos) ? Color.Make(1f, 1f, 1f) : Color.Make(0f, 0f, 0f);
        }

        public override HwVector3S Specular(HwVector3S pos)
        {
            return Color.Make(1f, 1f, 1f);
        }

        public override float Reflect(HwVector3S pos)
        {
            return Calc(pos) ? 0.1f : 0.7f;
        }
    }

    public class Camera
    {
        public Vector3S Pos;
        public Vector3S Forward;
        public Vector3S Up;
        public Vector3S Right;

        public static Camera Create(Vector3S pos, Vector3S lookAt)
        {
            HwVector3S forward = Vector.Normalize(lookAt.Load() - pos.Load());
            HwVector3S down = Vector128.Create(0f, -1f, 0f, 0f);
            HwVector3S right = Vector.Normalize(Vector.CrossProduct(forward, down)) * 1.5f;
            HwVector3S up = Vector.Normalize(Vector.CrossProduct(forward, right)) * 1.5f;

            var camera = new Camera { Pos = pos };

            forward.Store(out camera.Forward);
            up.Store(out camera.Up);
            right.Store(out camera.Right);

            return camera;
        }
    }

    public class Light
    {
        public Vector3S Pos;
        public HwVector3S Color;
    }

    public abstract class SceneObject
    {
        public Surface Surface;
        public abstract ISect Intersect(Ray ray);
        public abstract Vector3S Normal(Vector3S pos);
    }

    public class Sphere : SceneObject
    {
        public Vector3S Center;
        public float Radius;

        public override ISect Intersect(Ray ray)
        {
            HwVector3S eo = Center.Load() - ray.Start.Load();
            var v = Vector.DotProduct(eo, ray.Dir.Load());
            HwVector3S dist;
            if (Vector.MoveMask(Vector.CompareLessThan(v, Vector128<float>.Zero)) != 0)
            {
                dist = Vector128<float>.Zero;
            }
            else
            {
                HwVector3S disc = MathF.Pow(Radius, 2).LoadScalarBroadcast() - (Vector.LengthSquared(eo) - MathF.Pow(v.Value.ToScalar(), 2));
                dist = Vector.MoveMask(Vector.CompareLessThan(v, Vector128<float>.Zero)) != 0 ? (HwVector3S)Vector128<float>.Zero : v - Vector.Sqrt(disc);
            }
            if (Vector.MoveMask(Vector.CompareEqual(v, Vector128<float>.Zero)) == 0b_1111) return null!;

            return new ISect
            {
                Thing = this,
                Ray = ray,
                Dist = dist.Value.ToScalar()
            };
        }

        public override Vector3S Normal(Vector3S pos)
        {
            var res = Vector.Normalize(pos.Load() - Center.Load());
            res.Store(out Vector3S result);

            return result;
        }
    }

    class Plane : SceneObject
    {
        public Vector3S Norm;
        public float Offset;

        public override ISect Intersect(Ray ray)
        {
            var denom = Vector.DotProduct(Norm.Load(), ray.Dir.Load());
            if (Vector.MoveMask(Vector.CompareGreaterThan(denom, Vector128<float>.Zero)) != 0) return null;
            return new ISect
            {
                Thing = this,
                Ray = ray,
                Dist = ((Vector.DotProduct(Norm.Load(), ray.Start.Load()) + Offset) / -denom).Value.ToScalar()
            };
        }

        public override Vector3S Normal(Vector3S pos)
        {
            return Norm;
        }
    }

    public class Scene
    {
        public SceneObject[] Things;
        public Light[] Lights;
        public Camera Camera;

        public IntersectEnumerator Intersect(Ray r)
        {
            return new IntersectEnumerator(r, Things);
        }

        public struct IntersectEnumerator : IEnumerator<ISect>, IEnumerable<ISect>
        {
            public IntersectEnumerator(Ray ray, SceneObject[] things)
            {
                _ray = ray;
                _things = things;
                _index = 0;
                Current = _things[0].Intersect(ray);
            }

            private readonly Ray _ray;
            private readonly SceneObject[] _things;
            private int _index;

            public bool MoveNext()
            {
                _index++;

                if (_index >= _things.Length)
                {
                    return false;
                }

                Current = _things[_index].Intersect(_ray);
                return true;
            }

            public void Reset()
            {
            }

            public ISect Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public IntersectEnumerator GetEnumerator() => this;

            IEnumerator<ISect> IEnumerable<ISect>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

}