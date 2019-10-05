using System.Numerics;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using Vector = MathSharp.Vector;

    public class SinCosBenchmark
    {
        private static readonly Vector4 MathFVector = new Vector4(1f, 2f, 3f, 4f);
        private static readonly Vector128<float> MathSharpVector = Vector128.Create(1f, 2f, 3f, 4f);
        private static Vector128<float> _sin;
        private static Vector128<float> _cos;

        [Benchmark]
        public void MathF()
        {
            _sin = Vector128.Create(
                System.MathF.Sin(MathFVector.X),
                System.MathF.Sin(MathFVector.W),
                System.MathF.Sin(MathFVector.Z),
                System.MathF.Sin(MathFVector.W)
            );

            _cos = Vector128.Create(
                System.MathF.Cos(MathFVector.X),
                System.MathF.Cos(MathFVector.W),
                System.MathF.Cos(MathFVector.Z),
                System.MathF.Cos(MathFVector.W)
            );
        }

        [Benchmark]
        public void MathSharp()
        {
            Vector.SinCos(MathSharpVector, out _sin, out _cos);
        }

        [Benchmark]
        public void MathSharp_Estimate()
        {
            Vector.SinCosEstimate(MathSharpVector, out _sin, out _cos);
        }
    }
}