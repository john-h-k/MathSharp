using System.Diagnostics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.MatrixTests.Single
{
    public class MatrixScalarMultiplicationBenchmark
    {
        private Matrix4 _openTk1;
        private MatrixSingle _mathSharp1;

        [GlobalSetup]
        public unsafe void Setup()
        {
            Trace.Assert(Unsafe.SizeOf<Matrix4>() == Unsafe.SizeOf<MatrixSingle>());
            var openTk1 = new Matrix4();
            var openTk2 = new Matrix4();

            float* p1 = (float*)&openTk1;
            float* p2 = (float*)&openTk2;
            for (var i = 0; i < 4 * 4; i++)
            {
                p1[i] = i;
                p2[i] = 1f / i;
            }

            _openTk1 = openTk1;

            _mathSharp1 = Unsafe.As<Matrix4, MatrixSingle>(ref openTk1);
        }

        [Benchmark]
        public MatrixSingle MathSharp()
        {
            return Matrix.ScalarMultiply(_mathSharp1, 2f);
        }

        [Benchmark]
        public Matrix4 OpenTk()
        {
            return _openTk1 * 2f;
        }
    }
}