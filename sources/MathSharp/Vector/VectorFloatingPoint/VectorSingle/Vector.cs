using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Constants;

namespace MathSharp
{
    public static partial class Vector
    {
        public static class SingleConstants
        {
            public static Vector128<float> Zero => Vector128<float>.Zero;
            public static Vector128<float> One => Vector128.Create(1f);

            // Uses the 'ones idiom', which is where
            // cmpeq xmmN, xmmN
            // is used, and the result is guaranteed to be all ones
            // it has no dependencies and is useful
            // For anyone looking at codegen, it is actually
            // [v]cmpps xmm0, xmm0, xmm0, 0x0
            // which is functionally identical
            public static HwVectorAnyS AllBitsSet
            {
                [MethodImpl(MaxOpt)]
                get
                {
                    Vector128<float> v = Zero;
                    return Equality(v, v);
                }
            }

            public static readonly Vector128<float> SignFlip2D = Vector128.Create(int.MinValue, int.MinValue, 0, 0).AsSingle();
            public static readonly Vector128<float> SignFlip3D = Vector128.Create(int.MinValue, int.MinValue, int.MinValue, 0).AsSingle();
            public static readonly Vector128<float> SignFlip4D = Vector128.Create(int.MinValue, int.MinValue, int.MinValue, int.MinValue).AsSingle();

            public static readonly Vector128<float> MaskW = Vector128.Create(-1, -1, -1, 0).AsSingle();
            public static readonly Vector128<float> MaskXYZ = Vector128.Create(0, 0, 0, -1).AsSingle();


            public static readonly Vector128<float> UnitX = Vector128.Create(1f, 0f, 0f, 0f);
            public static readonly Vector128<float> UnitY = Vector128.Create(0f, 1f, 0f, 0f);
            public static readonly Vector128<float> UnitZ = Vector128.Create(0f, 0f, 1f, 0f);
            public static readonly Vector128<float> UnitW = Vector128.Create(0f, 0f, 0f, 1f);

            public static readonly HwVectorAnyS OneDiv2Pi = Vector128.Create(ScalarSingleConstants.OneDiv2Pi);
            public static readonly HwVectorAnyS Pi2 = Vector128.Create(ScalarSingleConstants.Pi2);
            public static readonly HwVectorAnyS Pi = Vector128.Create(ScalarSingleConstants.Pi);
            public static readonly HwVectorAnyS PiDiv2 = Vector128.Create(ScalarSingleConstants.PiDiv2);
        }

        public static class DoubleConstants
        {
            public static Vector256<double> Zero => Vector256<double>.Zero;
            public static Vector256<double> One => Vector256.Create(1d);

            // Uses the 'ones idiom', which is where
            // cmpeq xmmN, xmmN
            // is used, and the result is guaranteed to be all ones
            // it has no dependencies and is useful
            // For anyone looking at codegen, it is actually
            // [v]cmpps xmm0, xmm0, xmm0, 0x0
            // which is functionally identical
            public static HwVectorAnyD AllBitsSet
            {
                [MethodImpl(MaxOpt)]
                get
                {
                    Vector256<double> v = Zero;
                    return Equality(v, v);
                }
            }
        }
    }
}
