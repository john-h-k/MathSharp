using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;
#pragma warning disable 162

namespace MathSharp
{

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    public static partial class Vector
    {
        #region Vector

        /// <summary>
        /// Returns the absolute value of each element in <paramref name="vector"/>
        /// </summary>
        /// <param name="vector">The vector to take the absolute values</param>
        /// <returns>The absolute value of each element in <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Abs(Vector128<float> vector)
            => And(vector, SingleConstants.MaskSign);

        /// <summary>
        /// Returns the absolute value of each element in <paramref name="vector"/>
        /// </summary>
        /// <param name="vector">The vector to take the absolute values</param>
        /// <returns>The absolute value of each element in <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<float> Abs(Vector256<float> vector)
            => And(vector, SingleConstants256.MaskSign);


        [MethodImpl(MaxOpt)]
        public static Vector128<float> HorizontalAdd(Vector128<float> left, Vector128<float> right)
        {
            /*
             * return Vector128.Create(
             *     X(left) + Y(left),
             *     Z(left) + W(left),
             *     X(right) + Y(right),
             *     Z(right) + W(right)
             * );
             *
             * HorizontalAdd of A - (Ax, Ay, Az, Aw) and B - (Bx, By, Bz, Bw) is
             * (Ax + Ay, Az + Aw, Bx + By, Bz + Bw)
             *
             * So when we don't have hadd instruction, we can just use normal add after getting the vectors
             * (Ax, Az, Bx, Bz) and (Ay, Aw, By, Bw)
             *
             * We explicitly use the Sse methods here as this would be a slow way to do it on the software fallback
             */

            if (Sse3.IsSupported)
            {
                return Sse3.HorizontalAdd(left, right);
            }

            if (Sse.IsSupported)
            {
                Vector128<float> vector1 = Sse.Shuffle(left, right, ShuffleValues.XZXZ);
                Vector128<float> vector2 = Sse.Shuffle(left, right, ShuffleValues.YWYW);

                return Sse.Add(vector1, vector2);
            }

            return HorizontalAdd_Software(left, right);
        }

        /// <summary>
        /// Adds each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to be added</param>
        /// <param name="right">The right vector to be added</param>
        /// <returns>The per-element addition of <paramref name="left"/> and <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Add(left, right);
            }

            return Add_Software(left, right);
        }

        /// <summary>
        /// Adds a scalar to each element of a vectors
        /// </summary>
        /// <param name="vector">The vector to be added</param>
        /// <param name="scalar">The scalar to be added to each element of <paramref name="vector"/></param>
        /// <returns>The per-element addition of <paramref name="vector"/> with <paramref name="scalar"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add(Vector128<float> vector, float scalar)
            => Add(vector, Vector128.Create(scalar));

        /// <summary>
        /// Adds each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to be added</param>
        /// <param name="right">The right vector to be added</param>
        /// <returns>The per-element addition of <paramref name="left"/> and <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<float> Add(Vector256<float> left, Vector256<float> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Add(left, right);
            }

            return FromLowHigh(Add(left.GetLower(), right.GetLower()), Add(left.GetUpper(), right.GetLower()));
        }

        /// <summary>
        /// Subtracts each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to have <paramref name="right"/>subtracted from</param>
        /// <param name="right">The right vector to be subtracted from <paramref name="left"/></param>
        /// <returns>The per-element subtraction of <paramref name="right"/> from <paramref name="left"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        /// <summary>
        /// Subtracts each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to have <paramref name="right"/>subtracted from</param>
        /// <param name="right">The right vector to be subtracted from <paramref name="left"/></param>
        /// <returns>The per-element subtraction of <paramref name="right"/> from <paramref name="left"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<int> Subtract(Vector128<int> left, Vector128<int> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        /// <summary>
        /// Subtracts each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to have <paramref name="right"/>subtracted from</param>
        /// <param name="right">The right vector to be subtracted from <paramref name="left"/></param>
        /// <returns>The per-element subtraction of <paramref name="right"/> from <paramref name="left"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<float> Subtract(Vector256<float> left, Vector256<float> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Subtract(left, right);
            }

            return FromLowHigh(Subtract(left.GetLower(), right.GetLower()), Subtract(left.GetUpper(), right.GetLower()));
        }

        /// <summary>
        /// Subtracts a scalar from each element of a vector
        /// </summary>
        /// <param name="vector">The vector to be added</param>
        /// <param name="scalar">The scalar to be subtracted from each element of <paramref name="vector"/></param>
        /// <returns>The per-element subtraction of <paramref name="scalar"/> from <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract(Vector128<float> vector, float scalar)
            => Subtract(vector, Vector128.Create(scalar));

        /// <summary>
        /// Multiplies each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to be multiplied</param>
        /// <param name="right">The right vector to be multiplied</param>
        /// <returns>The per-element multiplication of <paramref name="left"/> and <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        /// <summary>
        /// Multiplies each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to be multiplied</param>
        /// <param name="right">The right vector to be multiplied</param>
        /// <returns>The per-element multiplication of <paramref name="left"/> and <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<float> Multiply(Vector256<float> left, Vector256<float> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Multiply(left, right);
            }

            return FromLowHigh(Multiply(left.GetLower(), right.GetLower()), Multiply(left.GetUpper(), right.GetLower()));
        }

        /// <summary>
        /// Multiplies each element of 2 vectors
        /// </summary>
        /// <param name="vector">The vector to be added</param>
        /// <param name="scalar">The scalar to be subtracted from each element of <paramref name="vector"/></param>
        /// <returns>The per-element subtraction of <paramref name="scalar"/> from <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply(Vector128<float> vector, float scalar)
            => Multiply(vector, Vector128.Create(scalar));

        /// <summary>
        /// Squares each element of 2 vectors
        /// </summary>
        /// <param name="vector">The vector to have each element squared</param>
        /// <returns>The per-element square of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Square(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(vector, vector);
            }

            return Multiply_Software(vector, vector);
        }

        /// <summary>
        /// Squares each element of 2 vectors
        /// </summary>
        /// <param name="vector">The vector to have each element squared</param>
        /// <returns>The per-element square of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<float> Square(Vector256<float> vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.Multiply(vector, vector);
            }

            return Multiply(vector, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide(Vector128<float> dividend, Vector128<float> divisor)
        {
            if (Sse.IsSupported)
            {
                return Sse.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DivideApprox(Vector128<float> dividend, Vector128<float> divisor)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(dividend, Sse.Reciprocal(divisor));
            }

            return Divide(dividend, divisor);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<float> Divide(Vector256<float> dividend, Vector256<float> divisor)
        {
            if (Avx.IsSupported)
            {
                return Avx.Divide(dividend, divisor);
            }

            return FromLowHigh(Divide(dividend.GetLower(), divisor.GetLower()), Divide(dividend.GetUpper(), divisor.GetLower()));
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide(Vector128<float> dividend, float scalarDivisor)
            => Multiply(dividend, Vector128.Create(scalarDivisor));


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Clamp(Vector128<float> vector, Vector128<float> low, Vector128<float> high)
        {
            Debug.Assert(CompareLessThanOrEqual(low, high).AllTrue(), $"Low argument for clamp ({low}) is more than high ({high})", nameof(low));

            return Max(Min(vector, high), low);
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sqrt(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Sqrt(vector);
            }

            return Sqrt_Software(vector);
        }

        // Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded, and also with +0/-0, where with MathF/Math, +0 is returned over -0,
        // - here, the second op is returned irrelevant of value if both are +0/-0
        // TODO We should provide a symmetric alternative to this

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Max(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Max(left, right);
            }

            return Max_Software(left, right);
        }

        // TODO Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded. We should provide a symmetric alternative to this

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Min(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Min(left, right);
            }

            return Min_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Negate(Vector128<float> vector)
            => Xor(vector, SingleConstants.MaskNotSign);

        [MethodImpl(MaxOpt)]
        public static Vector256<float> Negate(Vector256<float> vector)
            => Xor(vector, SingleConstants256.MaskNotSign);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CopySign(Vector128<float> vector, Vector128<float> sign)
            => Or(ExtractSign(sign), Abs(vector));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> ExtractSign(Vector128<float> vector)
            => And(vector, SingleConstants.MaskNotSign);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Mod2Pi(Vector128<float> vector)
        {
            Vector128<float> result = Multiply(vector, SingleConstants.OneDiv2Pi);

            result = Round(result);
            result = Multiply(result, SingleConstants.Pi2);

            return Subtract(vector, result);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Round(Vector128<float> vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNearestInteger(vector);
            }
            // TODO accelerate with SSE

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector128<float> vector)
            {
                // TODO is this semantically equivalent to 'roundps'?
                return Vector128.Create(
                    MathF.Round(X(vector)),
                    MathF.Round(Y(vector)),
                    MathF.Round(Z(vector)),
                    MathF.Round(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Remainder(Vector128<float> left, Vector128<float> right)
        {
            Vector128<float> n = Divide(left, right);
            n = Truncate(n);

            Vector128<float> y = Multiply(n, right);

            return Subtract(left, y);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<float> Remainder(Vector256<float> left, Vector256<float> right)
        {
            Vector256<float> n = Divide(left, right);
            n = Truncate(n);

            Vector256<float> y = Multiply(n, right);

            return Subtract(left, y);
        }

        public static Vector128<float> Remainder(Vector128<float> left, float right)
            => Remainder(left, Vector128.Create(right));

        private static readonly Vector128<float> NoFraction = Vector128.Create(8388608f);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Truncate(Vector128<float> vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToZero(vector);
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector128<float> vector)
            {
                return Vector128.Create(
                    MathF.Truncate(X(vector)),
                    MathF.Truncate(Y(vector)),
                    MathF.Truncate(Z(vector)),
                    MathF.Truncate(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<float> Truncate(Vector256<float> vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.RoundToZero(vector);
            }

            return FromLowHigh(Truncate(vector.GetLower()), Truncate(vector.GetUpper()));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Floor(Vector128<float> vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.Floor(vector);
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector128<float> vector)
            {
                return Vector128.Create(
                    MathF.Floor(X(vector)),
                    MathF.Floor(Y(vector)),
                    MathF.Floor(Z(vector)),
                    MathF.Floor(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Reciprocal(Vector128<float> vector)
        {
            return Divide(SingleConstants.One, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> ReciprocalSqrt(Vector128<float> vector)
        {
            return Divide(SingleConstants.One, Sqrt(vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> ReciprocalApprox(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Reciprocal(vector);
            }

            return Reciprocal(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> ReciprocalSqrtApprox(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.ReciprocalSqrt(vector);
            }

            return ReciprocalSqrt(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> InBounds(Vector128<float> vector, Vector128<float> bound)
        {
            var lessThan = CompareLessThanOrEqual(vector, bound);
            var greaterThan = CompareGreaterThanOrEqual(vector, Negate(bound));

            return And(lessThan, greaterThan);
        }

        #endregion

        private static readonly Vector128<float> FiniteComparison0 = Vector128.Create(0x7FFFFFFF).AsSingle();
        private static readonly Vector128<float> FiniteComparison1 = Vector128.Create(0x7F800000).AsSingle();
        public static Vector128<float> IsFinite(Vector128<float> vector) => CompareLessThan(And(vector, FiniteComparison0), FiniteComparison1);
        public static Vector128<float> IsInfinite(Vector128<float> vector) => CompareEqual(And(vector, FiniteComparison0), FiniteComparison1);
        public static Vector128<float> IsNaN(Vector128<float> vector) => CompareGreaterThan(And(vector, FiniteComparison0), FiniteComparison1);
        public static Vector128<float> IsNotNaN(Vector128<float> vector) => CompareLessThanOrEqual(And(vector, FiniteComparison0), FiniteComparison1);
        public static Vector128<float> IsZero(Vector128<float> vector) => CompareEqual(vector, SingleConstants.Zero);
        public static Vector128<float> IsNotZero(Vector128<float> vector) => CompareNotEqual(vector, SingleConstants.Zero);
    }
}
