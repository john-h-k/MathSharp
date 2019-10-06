using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Permute(Vector4DParam1_3 vector, byte control)
        {
            if (Avx.IsSupported)
            {
                return Avx.Permute(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithX(Vector4DParam1_3 vector)
            => Permute(vector, ShuffleValues._0_0_0_0);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithY(Vector4DParam1_3 vector)
            => Permute(vector, ShuffleValues._1_1_1_1);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithZ(Vector4DParam1_3 vector)
            => Permute(vector, ShuffleValues._2_2_2_2);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithW(Vector4DParam1_3 vector)
            => Permute(vector, ShuffleValues._3_3_3_3);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Shuffle(Vector4DParam1_3 left, Vector4DParam1_3 right, byte control)
        {
            if (Avx.IsSupported)
            {
                return Avx.Shuffle(left, right, control);
            }

            return Shuffle_Software(left, right, control);
        }

        [MethodImpl(MaxOpt)]
        public static byte MoveMask(Vector4D vector)
        {
            if (Avx.IsSupported)
            {
                return (byte)Avx.MoveMask(vector);
            }

            return SoftwareFallback(vector);

            static byte SoftwareFallback(Vector4D vector)
            {
                double s0 = X(vector);
                double s1 = Y(vector);
                double s2 = Z(vector);
                double s3 = W(vector);

                long e0 = Unsafe.As<double, long>(ref s0);
                long e1 = Unsafe.As<double, long>(ref s1);
                long e2 = Unsafe.As<double, long>(ref s2);
                long e3 = Unsafe.As<double, long>(ref s3);

                return (byte)(SignAsByteBool(e0) | (SignAsByteBool(e1) << 1) | (SignAsByteBool(e2) << 2) | (SignAsByteBool(e3) << 3));
            }

            static byte SignAsByteBool(long i) => i < 0 ? (byte)1 : (byte)0;
        }
    }
}
