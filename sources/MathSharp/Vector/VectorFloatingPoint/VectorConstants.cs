using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Constants;

namespace MathSharp
{
    public static partial class Vector
    {
        public static class SingleConstants
        {
            public static Vector128<float> Zero
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Vector128<float>.Zero;
            }

            public static Vector128<float> One
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Vector128.Create(1f);
            }

            // Uses the 'ones idiom', which is where
            // cmpeq xmmN, xmmN
            // is used, and the result is guaranteed to be all ones
            // it has no dependencies and is useful
            // For anyone looking at codegen, it is actually
            // [v]cmpps xmm0, xmm0, xmm0, 0x0
            // which is functionally identical
            public static Vector128<float> AllBitsSet
            {
                [MethodImpl(MaxOpt)]
                get
                {
                    if (Sse.IsSupported)
                    {
                        Vector128<float> v = Zero;
                        return CompareEqual(v, v);
                    }

                    return Vector128.Create(-1).AsSingle();
                }
            }

            public static readonly Vector128<float> MaskSign = Vector128.Create(int.MaxValue).AsSingle();
            public static readonly Vector128<float> MaskNotSign = Vector128.Create(~int.MaxValue).AsSingle();

            public static readonly Vector128<float> MaskX = Vector128.Create(+0, -1, -1, -1).AsSingle();
            public static readonly Vector128<float> MaskY = Vector128.Create(-1, +0, -1, -1).AsSingle();
            public static readonly Vector128<float> MaskZ = Vector128.Create(-1, -1, +0, -1).AsSingle();
            public static readonly Vector128<float> MaskW = Vector128.Create(-1, -1, -1, +0).AsSingle();

            // ReSharper disable InconsistentNaming
            public static readonly Vector128<float> MaskXY = Vector128.Create(+0, +0, -1, -1).AsSingle();
            public static readonly Vector128<float> MaskZW = Vector128.Create(-1, -1, +0, +0).AsSingle();

            public static readonly Vector128<float> MaskXYZ = Vector128.Create(+0, +0, +0, -1).AsSingle();
            public static readonly Vector128<float> MaskYZW = Vector128.Create(-1, +0, +0, +0).AsSingle();

            public static readonly Vector128<float> MaskXYZW = Vector128.Create(0).AsSingle();
            // ReSharper restore InconsistentNaming

            public static readonly Vector128<float> UnitX = Vector128.Create(1f, 0f, 0f, 0f);
            public static readonly Vector128<float> UnitY = Vector128.Create(0f, 1f, 0f, 0f);
            public static readonly Vector128<float> UnitZ = Vector128.Create(0f, 0f, 1f, 0f);
            public static readonly Vector128<float> UnitW = Vector128.Create(0f, 0f, 0f, 1f);

            public static readonly Vector128<float> OneDiv2Pi = Vector128.Create(ScalarSingleConstants.OneDiv2Pi);
            public static readonly Vector128<float> Pi2 = Vector128.Create(ScalarSingleConstants.Pi2);
            public static readonly Vector128<float> Pi = Vector128.Create(ScalarSingleConstants.Pi);
            public static readonly Vector128<float> PiDiv2 = Vector128.Create(ScalarSingleConstants.PiDiv2);
        }

        public static class DoubleConstants
        {
            public static Vector256<double> Zero
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Vector256<double>.Zero;
            }

            public static Vector256<double> One
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Vector256.Create(1d);
            }

            // Uses the 'ones idiom', which is where
            // cmpeq xmmN, xmmN
            // is used, and the result is guaranteed to be all ones
            // it has no dependencies and is useful
            // For anyone looking at codegen, it is actually
            // [v]cmpps xmm0, xmm0, xmm0, 0x0
            // which is functionally identical
            public static Vector256<double> AllBitsSet
            {
                [MethodImpl(MaxOpt)]
                get
                {
                    if (Sse.IsSupported)
                    {
                        Vector256<double> v = Zero;
                        return CompareEqual(v, v);
                    }

                    return Vector256.Create(-1).AsDouble();
                }
            }

            public static readonly Vector256<double> MaskSign = Vector256.Create(long.MaxValue).AsDouble();
            public static readonly Vector256<double> MaskNotSign = Vector256.Create(~long.MaxValue).AsDouble();

            public static readonly Vector256<double> MaskLo128 = Vector256.Create(Vector128.Create(+0), Vector128.Create(-1)).AsDouble();
            public static readonly Vector256<double> MaskHi128 = Vector256.Create(Vector128.Create(-1), Vector128.Create(+0)).AsDouble();

            public static readonly Vector256<double> MaskX = Vector256.Create(+0, -1, -1, -1).AsDouble();
            public static readonly Vector256<double> MaskY = Vector256.Create(-1, +0, -1, -1).AsDouble();
            public static readonly Vector256<double> MaskZ = Vector256.Create(-1, -1, +0, -1).AsDouble();
            public static readonly Vector256<double> MaskW = Vector256.Create(-1, -1, -1, 0).AsDouble();

            // ReSharper disable InconsistentNaming
            public static readonly Vector256<double> MaskXY = Vector256.Create(+0, +0, -1, -1).AsDouble();
            public static readonly Vector256<double> MaskZW = Vector256.Create(-1, -1, +0, +0).AsDouble();

            public static readonly Vector256<double> MaskXYZ = Vector256.Create(+0, +0, +0, -1).AsDouble();
            public static readonly Vector256<double> MaskYZW = Vector256.Create(-1, +0, +0, +0).AsDouble();

            public static readonly Vector256<double> MaskXYZW = Vector256.Create(0).AsDouble();
            // ReSharper restore InconsistentNaming

            public static readonly Vector256<double> UnitX = Vector256.Create(1f, 0f, 0f, 0f);
            public static readonly Vector256<double> UnitY = Vector256.Create(0f, 1f, 0f, 0f);
            public static readonly Vector256<double> UnitZ = Vector256.Create(0f, 0f, 1f, 0f);
            public static readonly Vector256<double> UnitW = Vector256.Create(0f, 0f, 0f, 1f);

            public static readonly Vector256<double> OneDiv2Pi = Vector256.Create(ScalarDoubleConstants.OneDiv2Pi);
            public static readonly Vector256<double> Pi2 = Vector256.Create(ScalarDoubleConstants.Pi2);
            public static readonly Vector256<double> Pi = Vector256.Create(ScalarDoubleConstants.Pi);
            public static readonly Vector256<double> PiDiv2 = Vector256.Create(ScalarDoubleConstants.PiDiv2);
        }
    }
}
