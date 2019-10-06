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
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    public static partial class Vector
    {
        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Abs(Vector4FParam1_3 vector)
            => And(vector, SingleConstants.MaskSign);


        [MethodImpl(MaxOpt)]
        public static Vector128<float> HorizontalAdd(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
                Vector128<float> vector1 = Sse.Shuffle(left, right, ShuffleValues._0_2_0_2);
                Vector128<float> vector2 = Sse.Shuffle(left, right, ShuffleValues._1_3_1_3);

                return Sse.Add(vector1, vector2);
            }

            return HorizontalAdd_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Add(left, right);
            }

            return Add_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add(Vector4FParam1_3 vector, float scalar)
            => Add(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract(Vector4FParam1_3 vector, float scalar)
            => Subtract(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Square(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(vector, vector);
            }

            return Multiply_Software(vector, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply(Vector4FParam1_3 vector, float scalar)
            => Multiply(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide(Vector4FParam1_3 dividend, Vector4FParam1_3 divisor)
        {
            if (Sse.IsSupported)
            {
                return Sse.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide(Vector4FParam1_3 dividend, float scalarDivisor)
            => Multiply(dividend, Vector128.Create(scalarDivisor));


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Clamp(Vector4FParam1_3 vector, Vector4FParam1_3 low, Vector4FParam1_3 high)
        {
            Debug.Assert(CompareLessThanOrEqual(low, high).AllTrue(), "Min (low) argument for clamp is more than max (high)", nameof(low));

            return Max(Min(vector, high), low);
        }


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sqrt(Vector4FParam1_3 vector)
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
        public static Vector128<float> Max(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Max(left, right);
            }

            return Max_Software(left, right);
        }

        // TODO Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded. We should provide a symmetric alternative to this

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Min(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Min(left, right);
            }

            return Min_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Negate(Vector4FParam1_3 vector)
            => Xor(vector, SingleConstants.MaskNotSign);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CopySign(Vector128<float> sign, Vector128<float> vector)
            => Or(ExtractSign(sign), ClearSign(vector));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> ExtractSign(Vector128<float> vector)
            => And(vector, SingleConstants.MaskNotSign);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> ClearSign(Vector128<float> vector)
            => And(vector, SingleConstants.MaskSign);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Mod2Pi(Vector4FParam1_3 vector)
        {
            Vector128<float> result = Multiply(vector, SingleConstants.OneDiv2Pi);

            result = Round(result);
            result = Multiply(result, SingleConstants.Pi2);

            return Subtract(vector, result);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Round(Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNearestInteger(vector);
            }
            // TODO accelerate with SSE

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 vector)
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
        public static Vector128<float> Remainder(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            Vector128<float> n = Divide(left, right);
            n = Truncate(n);

            Vector128<float> y = Multiply(n, right);

            return Subtract(left, y);
        }

        public static Vector128<float> Remainder(Vector4FParam1_3 left, float right)
            => Remainder(left, Vector128.Create(right));

        private static readonly Vector128<float> NoFraction = Vector128.Create(8388608f);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Truncate(Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToZero(vector);
            }

            if (Sse2.IsSupported)
            {
                Vector128<int> fractionalValues = Abs(vector).AsInt32();

                fractionalValues = CompareLessThan(fractionalValues, NoFraction.AsInt32());
                var result = Sse2.ConvertToVector128Single(Sse2.ConvertToVector128Int32WithTruncation(vector)); // Roundtrip float->int32->float to truncate

                // Select results for non fractional values, else select the original value
                result = SelectWhereTrue(result, fractionalValues);
                fractionalValues = SelectWhereFalse(vector, fractionalValues).AsInt32();

                return Or(result, fractionalValues.AsSingle());
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 vector)
            {
                return Vector128.Create(
                    MathF.Truncate(X(vector)),
                    MathF.Truncate(Y(vector)),
                    MathF.Truncate(Z(vector)),
                    MathF.Truncate(W(vector))
                );
            }
        }

        // TODO move to proper Int32 file
        private static Vector128<int> CompareLessThan(Vector128<int> left, Vector128<int> right)
        {
            if (Sse.IsSupported)
            {
                return Sse2.CompareLessThan(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector128<int> SoftwareFallback(Vector128<int> left, Vector128<int> right)
            {
                return Vector128.Create(
                    BoolToSimdBoolInt32(X(left) < X(right)),
                    BoolToSimdBoolInt32(X(left) < X(right)),
                    BoolToSimdBoolInt32(X(left) < X(right)),
                    BoolToSimdBoolInt32(X(left) < X(right))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Floor(Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.Floor(vector);
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 vector)
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
        public static Vector128<float> InBounds(Vector4F vector, Vector4F bound)
        {
            var lessThan = CompareLessThanOrEqual(vector, bound);
            var greaterThan = CompareGreaterThanOrEqual(vector, Negate(bound));

            return And(lessThan, greaterThan);
        }

        #endregion
    }
}
