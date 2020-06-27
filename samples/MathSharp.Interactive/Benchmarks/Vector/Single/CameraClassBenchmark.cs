 using System;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

 namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using Vector = MathSharp.Vector;
    using Vector3 = System.Numerics.Vector3;

    public class CameraClassBenchmark
    {
        private float _pitch;
        private float _yaw;
        private Vector128<float> _unitY;
        private Vector128<float> _unitYWrapper;

        private Vector128<float> _mathSharpFront;
        private Vector128<float> _mathSharpWrapperFront;
        private Vector3 _openTkFront;

        [GlobalSetup]
        public void Setup()
        {
            _pitch = 11f;
            _yaw = 0.008888f;
            _unitY = Vector128.Create(0f, 1f, 0f, 0f);
            _unitYWrapper = _unitY;
            float x = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            float y = MathF.Sin(_pitch);
            float z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
            _mathSharpFront = Vector128.Create(x, y, z, 0);
            _mathSharpWrapperFront = _mathSharpFront;
            _openTkFront = new Vector3(x, y, z);
        }

        [Benchmark]
        public Vector128<float> MathSharp()
        {
            Vector128<float> front = _mathSharpFront;
            front = Vector.Normalize3D(front);
            Vector128<float> right = Vector.Normalize3D(Vector.Cross3D(_unitY, front));

            Vector128<float> up = Vector.Normalize3D(Vector.Cross3D(front, right));
            return up;
        }

        public void DotAndCross(Vector3 left0, Vector3 left1, Vector3 right0, Vector3 right1, out Vector3 result)
        {
            var leftDot = new Vector3(Vector3.Dot(left0, left1));
            var rightDot = new Vector3(Vector3.Dot(right0, right1));
            result = Vector3.Cross(leftDot, rightDot);
        }

        public void DotAndCross(in Vector3 left0, in Vector3 left1, in Vector3 right0, in Vector3 right1, out Vector3 result)
        {
            var leftDot = Vector.Dot3D(left0.Load(), left1.Load());
            var rightDot = Vector.Dot3D(right0.Load(), right1.Load());

            Vector.Store(Vector.Cross3D(leftDot, rightDot), out result);
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