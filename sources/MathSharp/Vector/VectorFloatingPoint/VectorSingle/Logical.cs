using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static float GetX(Vector128<float> vector) => vector.GetElement(0);

        [MethodImpl(MaxOpt)]
        public static float GetY(Vector128<float> vector) => vector.GetElement(1);

        [MethodImpl(MaxOpt)]
        public static float GetZ(Vector128<float> vector) => vector.GetElement(2);

        [MethodImpl(MaxOpt)]
        public static float GetW(Vector128<float> vector) => vector.GetElement(3);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Permute(Vector4FParam1_3 vector, byte control)
        {
            if (Avx.IsSupported)
            {
                return Avx.Permute(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> PermuteWithX(Vector4FParam1_3 vector)
            => Permute(vector, ShuffleValues._0_0_0_0);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> PermuteWithY(Vector4FParam1_3 vector)
            => Permute(vector, ShuffleValues._1_1_1_1);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> PermuteWithZ(Vector4FParam1_3 vector)
            => Permute(vector, ShuffleValues._2_2_2_2);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> PermuteWithW(Vector4FParam1_3 vector)
            => Permute(vector, ShuffleValues._3_3_3_3);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Shuffle(Vector4FParam1_3 left, Vector4FParam1_3 right, byte control)
        {
            if (Sse.IsSupported)
            {
                return Sse.Shuffle(left, right, control);
            }

            return Shuffle_Software(left, right, control);
        }

        [MethodImpl(MaxOpt)]
        public static byte MoveMask(Vector4F vector)
        {
            if (Sse.IsSupported)
            {
                return (byte)Sse.MoveMask(vector);
            }

            return SoftwareFallback(vector);

            static byte SoftwareFallback(Vector4F vector)
            {
                float s0 = X(vector);
                float s1 = Y(vector);
                float s2 = Z(vector);
                float s3 = W(vector);

                int e0 = Unsafe.As<float, int>(ref s0);
                int e1 = Unsafe.As<float, int>(ref s1);
                int e2 = Unsafe.As<float, int>(ref s2);
                int e3 = Unsafe.As<float, int>(ref s3);

                return (byte)(SignAsByteBool(e0) | (SignAsByteBool(e1) << 1) | (SignAsByteBool(e2) << 2) | (SignAsByteBool(e3) << 3));
            }

            static byte SignAsByteBool(int i) => i < 0 ? (byte)1 : (byte)0;
        }
    }
}
