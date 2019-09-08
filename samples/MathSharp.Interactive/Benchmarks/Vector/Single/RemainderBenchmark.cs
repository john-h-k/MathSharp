using BenchmarkDotNet.Attributes;
using System.Numerics;
namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using Vector = MathSharp.Vector;
    public class RemainderBenchmark
    {
        private Vector4 vec;

        [GlobalSetup]
        public void Setup()
        {
            vec = new Vector4(0, 1.2f, 1.6f, 3.2f);
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
        private Vector4 mathSharp(Vector4 vector, float divisor)
        {
            var v = vector.Load();
            v = Vector.Remainder(v, divisor);
            Vector.Store(v, out Vector4 result);
            return result;
        }

        private Vector4 normal(Vector4 vector, float divisor)
           => new Vector4(vector.X % 2, vector.Y % 2, vector.Z % 2, vector.W % 2);
    }
}
