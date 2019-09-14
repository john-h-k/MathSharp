using BenchmarkDotNet.Attributes;
using OpenTK;
using System.Runtime.Intrinsics.X86;

namespace MathSharp.Interactive.Benchmarks.Vector.Double
{
    using Vector = MathSharp.Vector;
    public class RemainderBenchmark
    {
        private Vector4d vec;

        [GlobalSetup]
        public void Setup()
        {
            vec = new Vector4d(0, 1.2f, 1.6f, 3.2f);
        }
        [Benchmark]
        public void Normal()
        {
            var result = normal(vec, 2);
        }

        [Benchmark]
        public void MathSharp()
        {
            var result = mathSharp(vec, 2);
        }

        //Prevents the JIT from compiling Normal() away.
        private unsafe Vector4d mathSharp(Vector4d vector, double divisor)
        {
            var v = Avx.LoadVector256((double*)&vector);
            v = Vector.Remainder(v, divisor);

            Vector4d result;
            Avx.Store((double*)&result, v);
            return result;
        }

        private Vector4d normal(Vector4d vector, double divisor)
           => new Vector4d(vector.X % divisor, vector.Y % divisor, vector.Z % divisor, vector.W % divisor);
    }
}
