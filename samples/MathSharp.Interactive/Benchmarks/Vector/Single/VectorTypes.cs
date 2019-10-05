using System.Runtime.Intrinsics;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    public struct Vector2S
    {
        public Vector2S(float xy)
        {
            X = xy;
            Y = xy;
        }

        public Vector2S(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X, Y;
    }

    public struct Vector3S
    {
        public Vector3S(float xyz)
        {
            X = xyz;
            Y = xyz;
            Z = xyz;
        }

        public Vector3S(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3S(Vector2S xy, float z)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }

        public float X, Y, Z;
    }

    public struct Vector4S
    {
        public Vector4S(float xyzw)
        {
            X = xyzw;
            Y = xyzw;
            Z = xyzw;
            W = xyzw;
        }

        public Vector4S(Vector2S xy, float z, float w)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
            W = w;
        }

        public Vector4S(Vector3S xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }

        public Vector4S(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float X, Y, Z, W;
    }

    public static unsafe class VectorTypeExtensions
    {
        public static Vector128<float> Load(this Vector2S vector) => MathSharp.Vector.Load2D(&vector.X);
        public static Vector128<float> Load(this Vector3S vector) => MathSharp.Vector.Load3D(&vector.X);
        public static Vector128<float> Load(this Vector4S vector) => MathSharp.Vector.Load4D(&vector.X);

        public static void Store(this Vector128<float> vector, out Vector2S destination)
        {
            fixed (void* p = &destination)
            {
                vector.Store2D((float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out Vector3S destination)
        {
            fixed (void* p = &destination)
            {
                vector.Store3D((float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out Vector4S destination)
        {
            fixed (void* p = &destination)
            {
                vector.Store4D((float*)p);
            }
        }
    }
}