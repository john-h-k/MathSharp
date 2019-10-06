using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.MatrixTests.Single
{
    [RPlotExporter]
    public class MatrixTransposeBenchmark
    {
        private Matrix4 _openTk1;
        private MatrixSingle _mathSharp1;
        private Matrix4x4 _sys1;

        [Benchmark]
        public MatrixSingle MathSharp()
        {
            return Matrix.Transpose(_mathSharp1);
        }

        [Benchmark]
        public Matrix4x4 SystemNumerics()
        {
            return Matrix4x4.Transpose(_sys1);
        }

        [Benchmark]
        public Matrix4 OpenTk()
        {
            return Matrix4.Transpose(_openTk1);
        }
    }
}