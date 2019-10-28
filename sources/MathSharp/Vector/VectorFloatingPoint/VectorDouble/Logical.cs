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
        public static Vector256<double> Shuffle(Vector256<double> vector, byte control)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Permute4x64(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FillWithX(Vector256<double> vector)
            => Shuffle(vector, ShuffleValues.X);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FillWithY(Vector256<double> vector)
            => Shuffle(vector, ShuffleValues.Y);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FillWithZ(Vector256<double> vector)
            => Shuffle(vector, ShuffleValues.Z);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FillWithW(Vector256<double> vector)
            => Shuffle(vector, ShuffleValues.W);

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
