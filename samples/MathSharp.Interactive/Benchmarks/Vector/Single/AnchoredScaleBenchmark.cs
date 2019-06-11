using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using OpenTK;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    using Vector = MathSharp.Vector;

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    [Orderer]
    public class AnchoredScaleBenchmark
    {
        private Vector2 _openTkTranslation;
        private Vector2 _openTkAnchor;
        private Vector2 _openTkScale;
        private Vector2 _openTkAmount;

        private Vector128<float> _mathSharpTranslation;
        private Vector128<float> _mathSharpAnchor;
        private Vector128<float> _mathSharpScale;
        private Vector128<float> _mathSharpAmount;
        private Vector128<float> _mathSharpOne;

        [GlobalSetup]
        public void Setup()
        {
            _openTkTranslation = new Vector2(1.7f, 2.3f);
            _openTkAnchor = new Vector2(1.0f, 0.0f);
            _openTkScale = new Vector2(7.0f, 3.6f);
            _openTkAmount = new Vector2(0.5f, 0.25f);

            _mathSharpTranslation = Vector128.Create(1.7f, 2.3f, 0f, 0f);
            _mathSharpAnchor = Vector128.Create(1.0f, 0.0f, 0f, 0f);
            _mathSharpScale = Vector128.Create(7.0f, 3.6f, 0f, 0f);
            _mathSharpAmount = Vector128.Create(0.5f, 0.25f, 0f, 0f);
            _mathSharpOne = Vector128.Create(1.0f, 1.0f, 0f, 0f);
        }

        [Benchmark]
        public Vector128<float> MathSharp()
        {
            Vector128<float> newScale = Vector.Multiply(_mathSharpScale, _mathSharpAmount);
            Vector128<float> deltaT = Vector.Multiply(_mathSharpScale, Vector.Subtract(_mathSharpOne, _mathSharpAmount));
            deltaT = Vector.Multiply(deltaT, _mathSharpAnchor);
            return Vector.Multiply((Vector.Add(_mathSharpTranslation, deltaT)), newScale);
        }

        [Benchmark]
        public Vector2 OpenTkMath()
        {
            Vector2 newScale = _openTkScale * _openTkAmount;
            Vector2 deltaT = _openTkScale 
                         * (Vector2.One - _openTkAmount)
                         * _openTkAnchor;
            return (_openTkTranslation + deltaT) * newScale;
        }
    }
}