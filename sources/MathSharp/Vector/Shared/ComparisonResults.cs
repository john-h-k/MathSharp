using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Transactions;
using static System.Runtime.CompilerServices.Unsafe;
using static MathSharp.Vector.ComparisonMask;

namespace MathSharp
{///
    public static partial class Vector
    {
        /// <summary>
        /// An enum used to compare masks returned by comparison functions and extracted with
        /// <see cref="Vector.MoveMask(Vector128{byte})"/>, <see cref="Vector.MoveMask(Vector128{float})"/>, <see cref="Vector.MoveMask(Vector256{double})"/>.
        /// Each element is represented by a single bit
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum ComparisonMask : int
        {
            None = 0,

            X = E00,
            Y = E01,
            Z = E02,
            W = E03,

            XY = X | Y,
            XYZ = X | Y | Z,
            XYZW = X | Y | Z | W,

            E00 = 1 << 0,
            E01 = 1 << 1,
            E02 = 1 << 2,
            E03 = 1 << 3,
            E04 = 1 << 4,
            E05 = 1 << 5,
            E06 = 1 << 6,
            E07 = 1 << 7,
            E08 = 1 << 8,
            E09 = 1 << 9,
            E10 = 1 << 10,
            E11 = 1 << 11,
            E12 = 1 << 12,
            E13 = 1 << 13,
            E14 = 1 << 14,
            E15 = 1 << 15,
            E16 = 1 << 16,
            E17 = 1 << 17,
            E18 = 1 << 18,
            E19 = 1 << 19,
            E20 = 1 << 20,
            E21 = 1 << 21,
            E22 = 1 << 22,
            E23 = 1 << 23,
            E24 = 1 << 24,
            E25 = 1 << 25,
            E26 = 1 << 26,
            E27 = 1 << 27,
            E28 = 1 << 28,
            E29 = 1 << 29,
            E30 = 1 << 30,
            E31 = 1 << 31
        }

        [MethodImpl(MaxOpt)]
        private static ComparisonMask GetCmpMaskForT<T>() where T : struct
        {
            if (SizeOf<T>() == 16)
            {
                return E00;
            }
            if (SizeOf<T>() == 8)
            {
                return E00 | E01;
            }
            if (SizeOf<T>() == 4)
            {
                return E00 | E01 | E02 | E03;
            }
            if (SizeOf<T>() == 2)
            {
                return E00 | E01 | E02 | E03 | E04 | E05 | E06 | E07;
            }
            if (SizeOf<T>() == 1)
            {
                return E00 | E01 | E02 | E03 | E04 | E05 | E06 | E07 | E08 | E09 | E10 | E11 | E12 | E13 | E14 | E15;
            }

            Debug.Fail("oh no. oh nooo. oh noooooooooooooo");
            return default;
        }

        [MethodImpl(MaxOpt)]
        private static ComparisonMask GetSubElemMaskForTAsByte<T>() where T : struct
        {
            if (SizeOf<T>() == 1)
            {
                return None;
            }
            if (SizeOf<T>() == 2)
            {
                return E00 | E01;
            }
            if (SizeOf<T>() == 4)
            {
                return E00 | E01 | E02 | E03;
            }
            if (SizeOf<T>() == 8)
            {
                return E00 | E01 | E02 | E03 | E04 | E05 | E06 | E07;
            }

            Debug.Fail("oh no. oh nooo. oh noooooooooooooo");
            return default;
        }

        /// <summary>
        /// Determines whether a vector contains only <see langword="true"/> values, comparing only elements specified in a comparison mask
        /// of <paramref name="mask"/> to the comparison vector
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <param name="mask">The <see cref="ComparisonMask"/> to determine which elements of <paramref name="vector"/> are to be checked</param>
        /// <returns><see langword="true"/> if the masked elements in the vector contains only <see langword="true"/> values, else <see langword="false"/></returns>
        [MethodImpl(MaxOpt)]
        public static bool AllTrue(this Vector128<float> vector, ComparisonMask mask)
            => (MoveMask(vector) & (int)mask) == (0b_1111 & (int)mask);

        /// <summary>
        /// Determines whether a vector contains only <see langword="false"/> values, comparing only elements specified in a comparison mask
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <param name="mask">The <see cref="ComparisonMask"/> to determine which elements of <paramref name="vector"/> are to be checked</param>
        /// <returns><see langword="true"/> if the masked elements in the vector contains only <see langword="false"/> values, else <see langword="false"/></returns>
        [MethodImpl(MaxOpt)]
        public static bool AllFalse(this Vector128<float> vector, ComparisonMask mask)
            => (MoveMask(vector) & (int)mask) == 0b_0000;

        /// <summary>
        /// Determines whether a vector contains only <see langword="true"/> values
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <returns><see langword="true"/> if the vector contains only <see langword="true"/> values, else <see langword="false"/></returns>
        [MethodImpl(MaxOpt)]
        public static bool AllTrue(this Vector128<float> vector)
            => MoveMask(vector) == 0b_1111;

        /// <summary>
        /// Determines whether a vector contains any <see langword="true"/> values
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <returns><see langword="true"/> if the vector contains any <see langword="true"/> values, else <see langword="false"/></returns>
        [MethodImpl(MaxOpt)]
        public static bool AnyTrue(this Vector128<float> vector)
            => MoveMask(vector) != 0b_0000;


        /// <summary>
        /// Determines whether a vector contains only <see langword="false"/> values
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <returns><see langword="true"/> if the vector contains only <see langword="false"/> values, else <see langword="false"/></returns>
        [MethodImpl(MaxOpt)]
        public static bool AllFalse(this Vector128<float> vector)
            => MoveMask(vector) == 0b_0000;

        /// <summary>
        /// Determines whether a vector contains any <see langword="false"/> values
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <returns><see langword="true"/> if the vector contains any <see langword="false"/> values, else <see langword="false"/></returns>
        [MethodImpl(MaxOpt)]
        public static bool AnyFalse(this Vector128<float> vector)
            => MoveMask(vector) != 0b_1111;

        /// <summary>
        /// Determines whether a vector contains both <see langword="true"/> and <see langword="false"/> values
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <returns><see langword="true"/> if the vector contains both <see langword="true"/> and <see langword="false"/> values, else <see langword="false"/></returns>
        [MethodImpl(MaxOpt)]
        public static bool AreMixed(this Vector128<float> vector)
        {
            var mask = MoveMask(vector);
            // TODO I am too lazy/have not had time to benchmark '&' (branchless) vs '&&' here
            return mask != 0 & mask != 0b_1111;
        }

        [MethodImpl(MaxOpt)]
        public static bool ElementTrue(this Vector128<float> vector, int elem)
        {
            Debug.Assert(elem > 0 && elem < 4);

            return vector.AsInt32().GetElement(elem) != 0;
            //return (MoveMask(vector) & (1 << elem)) != 0;
        }

        [MethodImpl(MaxOpt)]
        public static bool ElementFalse(this Vector128<float> vector, int elem)
        {
            Debug.Assert(elem > 0 && elem < 4);

            return vector.AsInt32().GetElement(elem) == 0;
            //return (MoveMask(vector) & (1 << elem)) == 0;
        }

        //private static ComparisonMask GetMaskAsCmpMask<T>(Vector128<T> vector) where T : struct =>
        //    (ComparisonMask)MoveMask(vector.AsByte());

        //[MethodImpl(MaxOpt)]
        //public static bool AllTrue<T>(this Vector128<T> vector, ComparisonMask mask) where T : struct
        //    => (GetMaskAsCmpMask(vector) & mask) == (GetCmpMaskForT<T>() & mask);

        //[MethodImpl(MaxOpt)]
        //public static bool AllFalse<T>(this Vector128<T> vector, ComparisonMask mask) where T : struct
        //    => (GetMaskAsCmpMask(vector) & mask) == None;

        //[MethodImpl(MaxOpt)]
        //public static bool AllTrue<T>(this Vector128<T> vector) where T : struct
        //    => GetMaskAsCmpMask(vector) == GetCmpMaskForT<T>();

        //[MethodImpl(MaxOpt)]
        //public static bool AnyTrue<T>(this Vector128<T> vector) where T : struct
        //    => GetMaskAsCmpMask(vector) != 0b_0000;

        //[MethodImpl(MaxOpt)]
        //public static bool AllFalse<T>(this Vector128<T> vector) where T : struct
        //    => GetMaskAsCmpMask(vector) == 0b_0000;

        //[MethodImpl(MaxOpt)]
        //public static bool AnyFalse<T>(this Vector128<T> vector) where T : struct
        //    => GetMaskAsCmpMask(vector) != GetCmpMaskForT<T>();

        //[MethodImpl(MaxOpt)]
        //public static bool AreMixed<T>(this Vector128<T> vector) where T : struct
        //{
        //    var mask = GetMaskAsCmpMask(vector);
        //    return mask != 0 && mask != GetCmpMaskForT<T>();
        //}

        //[MethodImpl(MaxOpt)]
        //public static bool ElementTrue<T>(this Vector128<T> vector, int elem) where T : struct
        //{
        //    Debug.Assert(elem > 0 && elem < 4);

        //    return ((int)GetMaskAsCmpMask(vector) & (1 << elem)) != 0;
        //}


        //[MethodImpl(MaxOpt)]
        //public static bool ElementFalse<T>(this Vector128<T> vector, int elem) where T : struct
        //{
        //    Debug.Assert(elem > 0 && elem < 4);

        //    return ((int)GetMaskAsCmpMask(vector) & (1 << elem)) == 0;
        //}
    }
}
