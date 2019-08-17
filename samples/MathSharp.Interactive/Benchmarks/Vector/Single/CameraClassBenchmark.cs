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

        private Vector128<float> _mathSharpFront;
        private Vector3 _openTkFront;

        [GlobalSetup]
        public void Setup()
        {
            _pitch =  11f;
            _yaw = 0.008888f;
            _unitY = Vector128.Create(0f, 1f, 0f, 0f);
            float x = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            float y = MathF.Sin(_pitch);
            float z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _mathSharpFront = Vector128.Create(x, y, z, 0);
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
        public Vector3 OpenTkMath()
        {
            Vector3 front = _openTkFront;
            front = Vector3.Normalize(front);
            Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, front));

            Vector3 up = Vector3.Normalize(Vector3.Cross(front, right));
            return up;
        }
    }
}