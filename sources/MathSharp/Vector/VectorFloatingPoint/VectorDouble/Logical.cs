using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    
    

    public static partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Permute(Vector256<double> vector, byte control)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Permute4x64(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithX(Vector256<double> vector)
            => Permute(vector, ShuffleValues._0_0_0_0);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithY(Vector256<double> vector)
            => Permute(vector, ShuffleValues._1_1_1_1);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithZ(Vector256<double> vector)
            => Permute(vector, ShuffleValues._2_2_2_2);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> PermuteWithW(Vector256<double> vector)
            => Permute(vector, ShuffleValues._3_3_3_3);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Shuffle(Vector256<double> left, Vector256<double> right, byte control)
        {
            //There is a way to do Permute4x64 with a few AVX instructions but haven't figured it out yet
            //if (Avx.IsSupported)
            //{
            //    return Avx.Shuffle(left, right, control);
            //}

            return Shuffle_Software(left, right, control);
        }

        [MethodImpl(MaxOpt)]
        public static byte MoveMask(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                return (byte)Avx.MoveMask(vector);
            }

            return SoftwareFallback(vector);

            static byte SoftwareFallback(Vector256<double> vector)
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
