using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
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

        private HwVector2S _mathSharpTranslation_Wrapper;
        private HwVector2S _mathSharpAnchor_Wrapper;
        private HwVector2S _mathSharpScale_Wrapper;
        private HwVector2S _mathSharpAmount_Wrapper;
        private HwVector2S _mathSharpOne_Wrapper;


        private Vector2 _result;

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

            _mathSharpTranslation_Wrapper = _mathSharpTranslation;
            _mathSharpAnchor_Wrapper = _mathSharpAnchor;
            _mathSharpScale_Wrapper = _mathSharpScale;
            _mathSharpAmount_Wrapper = _mathSharpAmount;
        }

        [Benchmark]
        public void MathSharp()
        {
            Vector128<float> newScale = Vector.Multiply(_mathSharpScale, _mathSharpAmount);
            Vector128<float> deltaT = Vector.Multiply(_mathSharpScale, Vector.Subtract(Vector.AllBitsSet, _mathSharpAmount));
            deltaT = Vector.Multiply(deltaT, _mathSharpAnchor);
            HwVector2S result = Vector.Multiply((Vector.Add(_mathSharpTranslation, deltaT)), newScale);

            result.Store(out _result);
        }

        [Benchmark]
        public void MathSharp_Wrappers()
        {
            HwVector2S newScale = _mathSharpScale_Wrapper * _mathSharpAmount_Wrapper;
            HwVector2S deltaT = _mathSharpScale_Wrapper * (Vector.AllBitsSet - _mathSharpAmount_Wrapper);
            deltaT *= _mathSharpAnchor_Wrapper;
            ((_mathSharpTranslation_Wrapper + deltaT) * newScale).Store(out _result);
        }

        [Benchmark]
        public void OpenTkMath()
        {
            Vector2 newScale = _openTkScale * _openTkAmount;
            Vector2 deltaT = _openTkScale * (Vector2.One - _openTkAmount);
            deltaT *= _openTkAnchor;
            _result = (_openTkTranslation + deltaT) * newScale;
        }
    }

    public static unsafe class OpenGlVectorExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Store(this HwVector2S hwVector, out Vector2 vector)
        {
            if (Sse.IsSupported)
            {
                fixed (void* pDest = &vector)
                {
                    Sse.StoreLow((float*)pDest, hwVector);
                }

                return;
            }

            Store_Software(hwVector, out vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Store_Software(this HwVector2S hwVector, out Vector2 vector)
        {
            vector = Unsafe.As<HwVector2S, Vector2>(ref hwVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2S Load(this Vector2 vector)
        {
            if (Sse.IsSupported)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being 0
                HwVector2S lo = Sse.LoadScalarVector128(&vector.X);
                HwVector2S hi = Sse.LoadScalarVector128(&vector.Y);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                return Sse.UnpackLow(lo, hi);
            }

            return SoftwareFallback(vector);

            static HwVector2S SoftwareFallback(Vector2 vector)
            {
                return Vector128.Create(vector.X, vector.Y, 0, 0);
            }
        }
    }
}