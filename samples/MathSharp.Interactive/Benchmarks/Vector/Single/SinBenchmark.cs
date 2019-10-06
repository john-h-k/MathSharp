using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using Vector = MathSharp.Vector;

    public class SinBenchmark
    {
        private static readonly Vector4 MathFVector = new Vector4(1f, 2f, 3f, 4f);
        private static readonly Vector128<float> MathSharpVector = Vector128.Create(1f, 2f, 3f, 4f);

        [Benchmark]
        public Vector128<float> MathF()
        {
            return Vector128.Create(
                System.MathF.Sin(MathFVector.X),
                System.MathF.Sin(MathFVector.W),
                System.MathF.Sin(MathFVector.Z),
                System.MathF.Sin(MathFVector.W)
            );
        }

        [Benchmark]
        public Vector128<float> MathSharp()
        {
            return Vector.Sin(MathSharpVector);
        }

        [Benchmark]
        public Vector128<float> MathSharp_Approx()
        {
            return Vector.SinApprox(MathSharpVector);
        }
    }
}