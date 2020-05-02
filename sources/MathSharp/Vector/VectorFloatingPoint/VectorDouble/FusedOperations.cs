using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    
    

    public static partial class Vector
    {
        /// <summary>
        /// Returns (x * y) + z on each element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) + z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplyAdd(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector256<double> x, Vector256<double> y, Vector256<double> z)
            {
                return Vector256.Create(
                    Math.FusedMultiplyAdd(X(x), X(y), X(z)),
                    Math.FusedMultiplyAdd(Y(x), Y(y), Y(z)),
                    Math.FusedMultiplyAdd(Z(x), Z(y), Z(z)),
                    Math.FusedMultiplyAdd(W(x), W(y), W(z))
                );
            }
        }

        /// <summary>
        /// Returns (x * y) + z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a Double, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) + z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplyAdd(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedMultiplyAdd(x, y, z);
            }

            return Add(Multiply(x, y), z);
        }

        /// <summary>
        /// Returns -(x * y) + z on each element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to to the infinite precision negated multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) + z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedNegateMultiplyAdd(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector256<double> x, Vector256<double> y, Vector256<double> z)
            {
                return FusedMultiplyAdd(Negate(x), y, z);
            }
        }

        /// <summary>
        /// Returns -(x * y) + z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a Double, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to the negated multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) + z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastNegateMultiplyAdd(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedNegateMultiplyAdd(x, y, z);
            }

            return Add(Negate(Multiply(x, y)), z);
        }

        /// <summary>
        /// Returns (x * y) - z on each element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) - z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplySubtract(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector256<double> x, Vector256<double> y, Vector256<double> z)
            {
                return FusedMultiplyAdd(x, y, Negate(z));
            }
        }

        /// <summary>
        /// Returns (x * y) - z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a Double, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from to multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) - z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplySubtract(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtract(x, y, z);
            }

            return Subtract(Multiply(x, y), z);
        }

        /// <summary>
        /// Returns -(x * y) - z on each element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) - z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedNegateMultiplySubtract(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector256<double> x, Vector256<double> y, Vector256<double> z)
            {
                return FusedMultiplySubtract(Negate(x), y, z);
            }
        }

        /// <summary>
        /// Returns -(x * y) - z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a Double, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) - z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastNegateMultiplySubtract(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (CanFuseOperations)
            {
                return FusedNegateMultiplySubtract(x, y, z);
            }

            return Subtract(Negate(Multiply(x, y)), z);
        }

        /// <summary>
        /// Returns (x * y) + z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation,
        /// and (x * y) - z on each odd-indexed element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplyAddSubtractAlternating(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddSubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector256<double> x, Vector256<double> y, Vector256<double> z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, DoubleConstants.MaskNotSignYW));
            }
        }

        /// <summary>
        /// Returns (x * y) + z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation,
        /// and (x * y) - z on each odd-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a Double, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplyAddSubtractAlternating(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplyAddSubtractAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, DoubleConstants.MaskNotSignYW);
            return Add(mul, negate);
        }

        /// <summary>
        /// Returns (x * y) - z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation,
        /// and (x * y) + z on each odd-indexed element of a <see cref="Vector256{Double}"/>, rounded as one ternary operation
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplySubtractAddAlternating(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector256<double> x, Vector256<double> y, Vector256<double> z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, DoubleConstants.MaskNotSignXZ));
            }
        }

        /// <summary>
        /// Returns (x * y) - z on each even-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation,
        /// and (x * y) + z on each odd-indexed element of a <see cref="Vector256{Double}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a Double, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplySubtractAddAlternating(Vector256<double> x, Vector256<double> y, Vector256<double> z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtractAddAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, DoubleConstants.MaskNotSignXZ);
            return Add(mul, negate);
        }
    }
}
