using System;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using Vector = MathSharp.Vector;
    public class CameraClassBenchmark
    {
        private float _pitch;
        private float _yaw;
        private Vector128<float> _unitY;
        private Vector3Test _unitYWrapper;

        private Vector128<float> _mathSharpFront;
        private Vector3Test _mathSharpWrapperFront;
        private Vector3 _openTkFront;

        [GlobalSetup]
        public void Setup()
        {
            _pitch =  11f;
            _yaw = 0.008888f;
            _unitY = Vector128.Create(0f, 1f, 0f, 0f);
            _unitYWrapper = new Vector3Test(_unitY);
            float x = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            float y = MathF.Sin(_pitch);
            float z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _mathSharpFront = Vector128.Create(x, y, z, 0);
            _mathSharpWrapperFront = new Vector3Test(_mathSharpFront);
            _openTkFront = new Vector3(x, y, z);
        }

        [Benchmark]
        public Vector128<float> MathSharp()
        {
            Vector128<float> front = _mathSharpFront;
            front = Vector.Normalize3D(front);
            Vector128<float> right = Vector.Normalize3D(Vector.CrossProduct3D(_unitY, front));

            Vector128<float> up = Vector.Normalize3D(Vector.CrossProduct3D(front, right));
            return up;
        }

        [Benchmark]
        public Vector3Test MathSharp_WrapperStruct()
        {
            Vector3Test front = _mathSharpWrapperFront;
            front = TestVector.Normalize(front);
            Vector3Test right = TestVector.Normalize(TestVector.CrossProduct(_unitYWrapper, front));

            Vector3Test up = TestVector.Normalize(TestVector.CrossProduct(front, right));
            return up;
        }

        [Benchmark]
        public Vector3 OpenTkMath()
        {
            Vector3 front = _openTkFront;
            front = Vector3.Normalize(front);
            Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, front));

            Vector3 up = Vector3.Normalize(Vector3.Cross(front, right));
            return up;
        }
    }

    public readonly struct Vector3Test
    {
        public readonly Vector128<float> Value;

        public Vector3Test(Vector128<float> value)
        {
            Value = value;
        }
    }


    public static class TestVector
    {
        public static Vector3Test Normalize(Vector3Test vector) => new Vector3Test(Vector.Normalize3D(vector.Value));
        public static Vector3Test CrossProduct(Vector3Test left, Vector3Test right) => new Vector3Test(Vector.CrossProduct3D(left.Value, right.Value));
        public static Vector128<float> Normalize3D(Vector128<float> vector) => Vector.Normalize3D(vector);
        public static Vector128<float> CrossProduct3D(Vector128<float> left, Vector128<float> right) => Vector.CrossProduct3D(left, right);
    }
}