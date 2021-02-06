using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using SysVector4 = System.Numerics.Vector4;
    using Vector = MathSharp.Vector;
    using SysVector2 = System.Numerics.Vector2;
    using TkVector2 = OpenTK.Vector2;

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


    [RPlotExporter]
    public class VectorArithmeticBenchmark
    {
        private TkVector2 _openTk1;
        private TkVector2 _openTk2;

        private SysVector2 _sysNumerics1;
        private SysVector2 _sysNumerics2;

        private Vector2<float> _mathSharp1;
        private Vector2<float> _mathSharp2;

        [Benchmark]
        public TkVector2 OpenTk()
        {
            var x = _openTk1 + _openTk2 * _openTk2;
            x *= Val0;
            x /= Val1;
            var k = x * x / 5;
            return k;
        }

        private float Val0 = 9;
        private float Val1 = 88;
        private float Val2 = 5;

        [Benchmark]
        public SysVector2 SystemNumerics()
        {
            var x = _sysNumerics1 + _sysNumerics2 * _sysNumerics2;
            x *= Val0;
            x /= Val1;
            var k = x * x / 5;
            return k;
        }

        [Benchmark]
        public Vector2<float> MathSharp()
        {
            PaddedVector2<float> x = _mathSharp1 + _mathSharp2;
            x *= Val0;
            x /= Val1;
            var k = x * x / 5;
            return k;
        }
    }
}