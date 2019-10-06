using System.Numerics;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.MatrixTests.Single
{
    [RPlotExporter]
    public class MatrixEqualityBenchmark
    {
        private static readonly MatrixSingle M1;
        private static readonly MatrixSingle M2;

        private static readonly Matrix4 OpenTk1;
        private static readonly Matrix4 OpenTk2;

        private static readonly Matrix4x4 SysNum1;
        private static readonly Matrix4x4 SysNum2;

        [Benchmark]
        public bool OpenTk()
        {
            return OpenTk1 == OpenTk2;
        }

        [Benchmark]
        public bool SystemNumerics()
        {
            return SysNum1 == SysNum2;
        }

        [Benchmark]
        public bool MathSharp()
        {
            return Matrix.CompareEqual(M1, M2);
        }
    }
}