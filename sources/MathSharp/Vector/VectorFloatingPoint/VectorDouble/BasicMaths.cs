using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    
    

    public static partial class Vector
    {
        #region Vector

        /// <summary>
        /// Returns the absolute value of each element in <paramref name="vector"/>
        /// </summary>
        /// <param name="vector">The vector to take the absolute values</param>
        /// <returns>The absolute value of each element in <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Abs(Vector256<double> vector)
            => Max(Subtract(Vector256<double>.Zero, vector), vector);


        [MethodImpl(MaxOpt)]
        public static Vector256<double> HorizontalAdd(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.HorizontalAdd(left, right);
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
        public static Vector256<double> Add(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Add(left, right);
            }

            return Add_Software(left, right);
        }

        /// <summary>
        /// Adds each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to be added</param>
        /// <param name="right">The right vector to be added</param>
        /// <returns>The per-element addition of <paramref name="left"/> and <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<double> Add(Vector128<double> left, Vector128<double> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Add(left, right);
            }

            return Add_Software(left, right);
        }

        /// <summary>
        /// Adds each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to be added</param>
        /// <param name="right">The right vector to be added</param>
        /// <returns>The per-element addition of <paramref name="left"/> and <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<int> Add(Vector128<int> left, Vector128<int> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Add(left, right);
            }

            return Add_Software(left, right);
        }

        /// <summary>
        /// Adds a scalar to each element of a vector
        /// </summary>
        /// <param name="vector">The vector to be added</param>
        /// <param name="scalar">The scalar to be added to each element of <paramref name="vector"/></param>
        /// <returns>The per-element addition of <paramref name="vector"/> with <paramref name="scalar"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Add(Vector256<double> vector, double scalar)
            => Add(vector, Vector256.Create(scalar));

        /// <summary>
        /// Subtracts each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to have <paramref name="right"/>subtracted from</param>
        /// <param name="right">The right vector to be subtracted from <paramref name="left"/></param>
        /// <returns>The per-element subtraction of <paramref name="right"/> from <paramref name="left"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Subtract(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Subtract(left, right);
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
        public static Vector128<double> Subtract(Vector128<double> left, Vector128<double> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        /// <summary>
        /// Subtracts a scalar from each element of a vector
        /// </summary>
        /// <param name="vector">The vector to be added</param>
        /// <param name="scalar">The scalar to be subtracted from each element of <paramref name="vector"/></param>
        /// <returns>The per-element subtraction of <paramref name="scalar"/> from <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Subtract(Vector256<double> vector, double scalar)
            => Subtract(vector, Vector256.Create(scalar));

        /// <summary>
        /// Multiplies each element of 2 vectors
        /// </summary>
        /// <param name="left">The left vector to be multiplied</param>
        /// <param name="right">The right vector to be multiplied</param>
        /// <returns>The per-element multiplication of <paramref name="left"/> and <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Multiply(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Multiply(left, right);
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
        public static Vector128<double> Multiply(Vector128<double> left, Vector128<double> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Multiply(left, right);
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
        public static Vector128<double> Multiply(Vector128<double> left, double right)
            => Multiply(left, Vector128.Create(right));


        [MethodImpl(MaxOpt)]
        public static Vector256<double> Square(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.Multiply(vector, vector);
            }

            return Multiply_Software(vector, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Multiply(Vector256<double> vector, double scalar)
            => Multiply(vector, Vector256.Create(scalar));

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Divide(Vector256<double> dividend, Vector256<double> divisor)
        {
            if (Avx.IsSupported)
            {
                return Avx.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<double> Divide(Vector128<double> dividend, Vector128<double> divisor)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<double> Divide(Vector128<double> dividend, double scalarDivisor)
            => Subtract(dividend, Vector128.Create(scalarDivisor));

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Divide(Vector256<double> dividend, double scalarDivisor)
            => Subtract(dividend, Vector256.Create(scalarDivisor));

        public static Vector256<double> Clamp(Vector256<double> vector, Vector256<double> low, Vector256<double> high)
            => Max(Min(vector, high), low);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Sqrt(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.Sqrt(vector);
            }

            return Sqrt_Software(vector);
        }


        [MethodImpl(MaxOpt)]
        public static Vector256<double> Max(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Max(left, right);
            }

            return Max_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector256<double> Min(Vector256<double> left, Vector256<double> right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Min(left, right);
            }

            return Min_Software(left, right);
        }

        public static Vector256<double> Negate(Vector256<double> vector)
            => Xor(DoubleConstants.MaskNotSign, vector);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CopySign(Vector256<double> sign, Vector256<double> vector)
            => Or(ExtractSign(sign), Abs(vector));

        [MethodImpl(MaxOpt)]
        public static Vector256<double> ExtractSign(Vector256<double> vector)
            => And(vector, DoubleConstants.MaskNotSign);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Mod2Pi(Vector256<double> vector)
        {
            Vector256<double> result = Multiply(vector, DoubleConstants.OneDiv2Pi);

            result = Round(result);
            result = Multiply(result, DoubleConstants.Pi2);

            return Subtract(vector, result);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Round(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.RoundToNearestInteger(vector);
            }

            if (Sse41.IsSupported)
            {
                GetLowHigh(vector, out var low, out var high);
                return FromLowHigh(Sse41.RoundToNearestInteger(low), Sse41.RoundToNearestInteger(high));
            }

            return SoftwareFallback(vector);

            static Vector256<double> SoftwareFallback(Vector256<double> vector)
            {
                // TODO is this semantically equivalent to 'roundps'?
                return Vector256.Create(
                    Math.Round(X(vector)),
                    Math.Round(Y(vector)),
                    Math.Round(Z(vector)),
                    Math.Round(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Remainder(Vector256<double> left, Vector256<double> right)
        {
            Vector256<double> n = Divide(left, right);
            n = Truncate(n);

            Vector256<double> y = Multiply(n, right);

            return Subtract(left, y);
        }

        public static Vector256<double> Remainder(Vector256<double> left, double right)
            => Remainder(left, Vector256.Create(right));

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Truncate(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.RoundToZero(vector);
            }

            if (Sse41.IsSupported)
            {
                GetLowHigh(vector, out var low, out var high);
                return FromLowHigh(Sse41.RoundToZero(low), Sse41.RoundToZero(high));
            }

            return SoftwareFallback(vector);

            static Vector256<double> SoftwareFallback(Vector256<double> vector)
            {
                return Vector256.Create(
                    Math.Truncate(X(vector)),
                    Math.Truncate(Y(vector)),
                    Math.Truncate(Z(vector)),
                    Math.Truncate(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Reciprocal(Vector256<double> vector)
        {
            return Divide(DoubleConstants.One, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> ReciprocalSqrt(Vector256<double> vector)
        {
            return Divide(DoubleConstants.One, Sqrt(vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> ReciprocalApprox(Vector256<double> vector)
        {
            return Reciprocal(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> ReciprocalSqrtApprox(Vector256<double> vector)
        {
            return ReciprocalSqrt(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> InBounds(Vector256<double> vector, Vector256<double> bound)
        {
            var lessThan = CompareLessThanOrEqual(vector, bound);
            var greaterThan = CompareGreaterThanOrEqual(vector, Negate(bound));

            return And(lessThan, greaterThan);
        }

        #endregion
    }
}
