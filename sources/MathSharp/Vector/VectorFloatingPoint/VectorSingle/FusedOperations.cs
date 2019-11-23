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
        // Whether 'Fastxxx' operations use 'Fusedxxx' operations or not
        public static bool CanFuseOperations
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Fma.IsSupported && !Options.StrictMath;
        }

        private static void ThrowPlatformNotSupported() =>
            ThrowHelper.ThrowPlatformNotSupportedException(FmaRequiredPlatformNotSupportedMessage);

        private static string FmaRequiredPlatformNotSupportedMessage => "Platform not supported for operation as it does not support FMA instructions";

        /// <summary>
        /// Returns (x * y) + z on each element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) + z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplyAdd(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector128<float> x, Vector128<float> y, Vector128<float> z)
            {
                return Vector128.Create(
                    MathF.FusedMultiplyAdd(X(x), X(y), X(z)),
                    MathF.FusedMultiplyAdd(Y(x), Y(y), Y(z)),
                    MathF.FusedMultiplyAdd(Z(x), Z(y), Z(z)),
                    MathF.FusedMultiplyAdd(W(x), W(y), W(z))
                );
            }
        }

        /// <summary>
        /// Returns (x * y) + z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a single, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) + z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplyAdd(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedMultiplyAdd(x, y, z);
            }

            return Add(Multiply(x, y), z);
        }

        /// <summary>
        /// Returns -(x * y) + z on each element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to to the infinite precision negated multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) + z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedNegateMultiplyAdd(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector128<float> x, Vector128<float> y, Vector128<float> z)
            {
                ThrowPlatformNotSupported();
                return default;
            }
        }

        /// <summary>
        /// Returns -(x * y) + z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a single, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to the negated multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) + z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastNegateMultiplyAdd(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedNegateMultiplyAdd(x, y, z);
            }

            return Add(Negate(Multiply(x, y)), z);
        }

        /// <summary>
        /// Returns (x * y) - z on each element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) - z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplySubtract(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector128<float> x, Vector128<float> y, Vector128<float> z)
            {
                return FusedMultiplyAdd(x, y, Negate(z));
            }
        }

        /// <summary>
        /// Returns (x * y) - z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a single, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from to multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) - z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplySubtract(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtract(x, y, z);
            }

            return Subtract(Multiply(x, y), z);
        }

        /// <summary>
        /// Returns -(x * y) - z on each element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) - z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedNegateMultiplySubtract(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector128<float> x, Vector128<float> y, Vector128<float> z)
            {
                ThrowPlatformNotSupported();
                return default;
            }
        }

        /// <summary>
        /// Returns -(x * y) - z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a single, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be subtracted from the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>-(x * y) - z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastNegateMultiplySubtract(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (CanFuseOperations)
            {
                return FusedNegateMultiplySubtract(x, y, z);
            }

            return Subtract(Negate(Multiply(x, y)), z);
        }

        /// <summary>
        /// Returns (x * y) + z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation,
        /// and (x * y) - z on each odd-indexed element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplyAddSubtractAlternating(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddSubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector128<float> x, Vector128<float> y, Vector128<float> z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, SingleConstants.MaskNotSignYW));
            }
        }

        /// <summary>
        /// Returns (x * y) + z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation,
        /// and (x * y) - z on each odd-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a single, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplyAddSubtractAlternating(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplyAddSubtractAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, SingleConstants.MaskNotSignYW);
            return Add(mul, negate);
        }

        /// <summary>
        /// Returns (x * y) - z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation,
        /// and (x * y) + z on each odd-indexed element of a <see cref="Vector128{Single}"/>, rounded as one ternary operation
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the infinite precision multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element, rounded as one ternary operation</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplySubtractAddAlternating(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector128<float> x, Vector128<float> y, Vector128<float> z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, SingleConstants.MaskNotSignXZ));
            }
        }

        /// <summary>
        /// Returns (x * y) - z on each even-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation,
        /// and (x * y) + z on each odd-indexed element of a <see cref="Vector128{Single}"/>, rounded either as 2 binary operations, or as one ternary operation
        /// It is implemented as a single, ternary operation if <see cref="CanFuseOperations"/> is <langword>true</langword>
        /// This presumes indexing beginning at 0
        /// </summary>
        /// <param name="x">The vector to be multiplied with <paramref name="y"/></param>
        /// <param name="y">The vector to be multiplied with <paramref name="x"/></param>
        /// <param name="z">The vector to be added to or subtracted from to the multiplication of <paramref name="x"/> and <paramref name="y"/></param>
        /// <returns>(x * y) +/- z on each element</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplySubtractAddAlternating(Vector128<float> x, Vector128<float> y, Vector128<float> z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtractAddAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, SingleConstants.MaskNotSignXZ);
            return Add(mul, negate);
        }
    }
}
