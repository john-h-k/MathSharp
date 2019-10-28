using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using Microsoft.VisualBasic.CompilerServices;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    
    

    public static partial class Vector
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum CompareMask : int
        {
            X = 0b_0001,
            Y = 0b_0010,
            Z = 0b_0100,
            W = 0b_1000,

            XY = X | Y,
            XYZ = X | Y | Z,
            XYZW = X | Y | Z | W,
        }

        [MethodImpl(MaxOpt)]
        public static bool AllTrue(this Vector128<float> vector, CompareMask mask)
            => (MoveMask(vector) & (int)mask) == (0b_1111 & (int)mask);

        [MethodImpl(MaxOpt)]
        public static bool AllFalse(this Vector128<float> vector, CompareMask mask)
            => (MoveMask(vector) & (int)mask) == 0b_0000;

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
        public static Vector128<int> CompareEqualBitwise32(Vector128<float> left, Vector128<float> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left.AsInt32(), right.AsInt32());
            }

            return SoftwareFallback(left, right);

            static Vector128<int> SoftwareFallback(Vector128<float> left, Vector128<float> right)
            {
                var l = left.AsInt32();
                var r = right.AsInt32();

                return Vector128.Create(
                    Eq(l.GetElement(0), r.GetElement(0)),
                    Eq(l.GetElement(1), r.GetElement(1)),
                    Eq(l.GetElement(2), r.GetElement(2)),
                    Eq(l.GetElement(3), r.GetElement(3))
                );
            }

            static int Eq(int left, int right) => left == right ? -1 : 0;
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
