using BenchmarkDotNet.Attributes;

namespace MathSharp.Interactive.Benchmarks.MatrixTests.Single
{
    public class MatrixIsIdentityBenchmark
    {
        private readonly MatrixSingle _matrix = default;

        [Benchmark]
        public bool Normal()
        {
            return Matrix.IsIdentity(_matrix);
        }
    }
}