using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using SysVector4 = System.Numerics.Vector4;
    using Vector = MathSharp.Vector;

    [RPlotExporter]
    public class VectorAdditionBenchmark
    {
        private Vector4 _openTk1;
        private Vector4 _openTk2;

        private SysVector4 _sysNumerics1;
        private SysVector4 _sysNumerics2;

        private Vector128<float> _mathSharp1;
        private Vector128<float> _mathSharp2;

        [Benchmark]
        public Vector4 OpenTk() => _openTk1 + _openTk2;

        [Benchmark]
        public SysVector4 SystemNumerics() => _sysNumerics1 + _sysNumerics2;

        [Benchmark]
        public Vector128<float> MathSharp() => Vector.Add(_mathSharp1, _mathSharp2);
    }
}