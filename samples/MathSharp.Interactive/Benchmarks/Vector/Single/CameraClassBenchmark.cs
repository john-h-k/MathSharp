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
        private HwVector3 _unitYWrapper;

        private Vector128<float> _mathSharpFront;
        private HwVector3 _mathSharpWrapperFront;
        private Vector3 _openTkFront;

        [GlobalSetup]
        public void Setup()
        {
            _pitch =  11f;
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
            HwVector3 front = _mathSharpFront;
            front = Vector.Normalize(front);
            HwVector3 right = Vector.Normalize(Vector.CrossProduct(_unitY, front));

            HwVector3 up = Vector.Normalize(Vector.CrossProduct(front, right));
            return up;
        }

        [Benchmark]
        public HwVector3 MathSharp_WrapperStruct()
        {
            HwVector3 front = _mathSharpWrapperFront;
            front = Vector.Normalize(front);
            HwVector3 right = Vector.Normalize(Vector.CrossProduct(_unitYWrapper, front));

            HwVector3 up = Vector.Normalize(Vector.CrossProduct(front, right));
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