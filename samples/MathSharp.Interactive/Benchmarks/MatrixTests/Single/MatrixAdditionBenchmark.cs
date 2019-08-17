using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.MatrixTests.Single
{
    public class MatrixAdditionBenchmark
    {
        private Matrix4 _openTk1;
        private Matrix4 _openTk2;

        private MatrixSingle _mathSharp1;
        private MatrixSingle _mathSharp2;



        [GlobalSetup]
        public unsafe void Setup()
        {
            Trace.Assert(Unsafe.SizeOf<Matrix4>() == Unsafe.SizeOf<MatrixSingle>());
            var openTk1 = new Matrix4();
            var openTk2 = new Matrix4();

            float* p1 = (float*) &openTk1;
            float* p2 = (float*) &openTk2;
            for (var i = 0; i < 4 * 4; i++)
            {
                p1[i] = i;
                p2[i] = 1f / i;
            }

            _openTk1 = openTk1;
            _openTk2 = openTk2;

            _mathSharp1 = Unsafe.As<Matrix4, MatrixSingle>(ref openTk1);
            _mathSharp2 = Unsafe.As<Matrix4, MatrixSingle>(ref openTk2);
        }
            
        [Benchmark]
        public MatrixSingle MathSharp()
        {
            return Matrix.Add(_mathSharp1, _mathSharp2);
        }

        [Benchmark]
        public MatrixSingle MathSharp_Clean_1()
        {
            return Matrix.Add_Clean_1(_mathSharp1, _mathSharp2);
        }

        [Benchmark]
        public MatrixSingle MathSharp_Clean_2()
        {
            return Matrix.Add_Clean_2(_mathSharp1, _mathSharp2);

        }

        [Benchmark]
        public Matrix4 OpenTk()
        {
            return _openTk1 + _openTk2;
        }
    }
}