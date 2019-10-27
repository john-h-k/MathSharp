using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static bool AllTrue(this Vector4FParam1_3 vector)
            => MoveMask(vector) == 0b_1111;


        [MethodImpl(MaxOpt)]
        public static bool AnyTrue(this Vector4FParam1_3 vector)
            => MoveMask(vector) != 0b_0000;

        [MethodImpl(MaxOpt)]
        public static bool AllFalse(this Vector4FParam1_3 vector)
            => MoveMask(vector) == 0b_0000;

        [MethodImpl(MaxOpt)]
        public static bool AnyFalse(this Vector4FParam1_3 vector)
            => MoveMask(vector) != 0b_1111;

        [MethodImpl(MaxOpt)]
        public static bool Mixed(this Vector4FParam1_3 vector)
        {
            var mask = MoveMask(vector);
            return mask != 0 && mask != 0b_1111;
        }

        [MethodImpl(MaxOpt)]
        public static bool ElementTrue(this Vector4FParam1_3 vector, int elem)
        {
            Debug.Assert(elem > 0 && elem < 4);

            return (MoveMask(vector) & (1 << elem)) != 0;
        }


        [MethodImpl(MaxOpt)]
        public static bool ElementFalse(this Vector4FParam1_3 vector, int elem)
        {
            Debug.Assert(elem > 0 && elem < 4);

            return (MoveMask(vector) & (1 << elem)) == 0;
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareEqual(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareEqual(left, right);
            }

            return CompareEqual_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<float> CompareEqual(Vector256<float> left, Vector256<float> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Compare(left, right, FloatComparisonMode.UnorderedEqualNonSignaling);
            }

            return FromLowHigh(CompareEqual(left.GetLower(), right.GetLower()),
                CompareEqual(left.GetUpper(), right.GetUpper()));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareNotEqual(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareNotEqual(left, right);
            }

            return CompareNotEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThan(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareGreaterThan(left, right);
            }

            return CompareGreaterThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThan(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareLessThan(left, right);
            }

            return CompareLessThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThanOrEqual(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareGreaterThanOrEqual(left, right);
            }

            return CompareGreaterThanOrEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThanOrEqual(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareLessThanOrEqual(left, right);
            }

            return CompareLessThanOrEqual_Software(left, right);
        }
    }
}
