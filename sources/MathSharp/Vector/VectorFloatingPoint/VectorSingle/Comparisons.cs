using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    
    

    public static partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static bool AllTrue(this Vector128<float> vector)
            => MoveMask(vector) == 0b_1111;


        [MethodImpl(MaxOpt)]
        public static bool AnyTrue(this Vector128<float> vector)
            => MoveMask(vector) != 0b_0000;

        [MethodImpl(MaxOpt)]
        public static bool AllFalse(this Vector128<float> vector)
            => MoveMask(vector) == 0b_0000;

        [MethodImpl(MaxOpt)]
        public static bool AnyFalse(this Vector128<float> vector)
            => MoveMask(vector) != 0b_1111;

        [MethodImpl(MaxOpt)]
        public static bool Mixed(this Vector128<float> vector)
        {
            var mask = MoveMask(vector);
            return mask != 0 && mask != 0b_1111;
        }

        [MethodImpl(MaxOpt)]
        public static bool ElementTrue(this Vector128<float> vector, int elem)
        {
            Debug.Assert(elem > 0 && elem < 4);

            return (MoveMask(vector) & (1 << elem)) != 0;
        }


        [MethodImpl(MaxOpt)]
        public static bool ElementFalse(this Vector128<float> vector, int elem)
        {
            Debug.Assert(elem > 0 && elem < 4);

            return (MoveMask(vector) & (1 << elem)) == 0;
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareEqual(Vector128<float> left, Vector128<float> right)
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
        public static Vector128<float> CompareNotEqual(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareNotEqual(left, right);
            }

            return CompareNotEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThan(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareGreaterThan(left, right);
            }

            return CompareGreaterThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThan(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareLessThan(left, right);
            }

            return CompareLessThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareGreaterThanOrEqual(left, right);
            }

            return CompareGreaterThanOrEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThanOrEqual(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareLessThanOrEqual(left, right);
            }

            return CompareLessThanOrEqual_Software(left, right);
        }
    }
}
