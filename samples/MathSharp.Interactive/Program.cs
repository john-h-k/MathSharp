using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.MatrixTests.Single;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<MatrixTransposeBenchmark>();
        }
    }
}