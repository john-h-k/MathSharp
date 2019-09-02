using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using OpenTkVector4 = OpenTK.Vector4;
    using Math = MathF;

    public class SinBenchmark
    {
        private static readonly Vector4 MathFVector = new Vector4(1f, 2f, 3f, 4f);
        private static readonly OpenTkVector4 OpenTkVector = new OpenTkVector4(1f, 2f, 3f, 4f);
        private static readonly HwVector4 MathSharpVector = Vector128.Create(1f, 2f, 3f, 4f);

        [Benchmark]
        public Vector4 MathF()
        {
            return new Vector4(
                Math.Sin(MathFVector.X),
                Math.Sin(MathFVector.W),
                Math.Sin(MathFVector.Z),
                Math.Sin(MathFVector.W)
            );
        }

        [Benchmark]
        public HwVector4 MathSharp()
        {
            return global::MathSharp.Vector.Sin(MathSharpVector);
        }
    }
}