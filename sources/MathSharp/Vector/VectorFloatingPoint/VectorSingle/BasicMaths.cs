using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Constants;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    //using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        #region Vector

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Permute(in Vector4FParam1_3 vector, byte control)
        {
            if (Avx.IsSupported)
            {
                return Avx.Permute(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Shuffle(in Vector4FParam1_3 left, in Vector4FParam1_3 right, byte control)
        {
            if (Sse.IsSupported)
            {
                return Sse.Shuffle(left, right, control);
            }

            return Shuffle_Software(left, right, control);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Abs(in Vector4FParam1_3 vector)
            => Max(Subtract(SingleConstants.Zero, vector), vector);


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS HorizontalAdd(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
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
                HwVectorAnyS vector1 = Sse.Shuffle(left, right, ShuffleValues._2_0_2_0);
                HwVectorAnyS vector2 = Sse.Shuffle(left, right, ShuffleValues._3_1_3_1);

                return Sse.Add(vector1, vector2);
            }

            return HorizontalAdd_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Add(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Add(left, right);
            }

            return Add_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Add(in Vector4FParam1_3 vector, float scalar)
            => Add(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Subtract(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Subtract(in Vector4FParam1_3 vector, float scalar)
            => Subtract(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Multiply(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Multiply(in Vector4FParam1_3 vector, float scalar)
            => Multiply(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Divide(in Vector4FParam1_3 dividend, in Vector4FParam1_3 divisor)
        {
            if (Sse.IsSupported)
            {
                return Sse.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Divide(in Vector4FParam1_3 dividend, float scalarDivisor)
            => Multiply(dividend, Vector128.Create(scalarDivisor));


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Clamp(in Vector4FParam1_3 vector, in Vector4FParam1_3 low, in Vector4FParam1_3 high)
        {
            Debug.Assert(ExtractMask(LessThan(low, high)) != 0, "Min (low) argument for clamp is less than max (high)", nameof(low));

            return Max(Min(vector, high), low);
        }


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Sqrt(in Vector4FParam1_3 vector)
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
        public static HwVectorAnyS Max(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Max(left, right);
            }

            return Max_Software(left, right);
        }

        // TODO Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded. We should provide a symmetric alternative to this

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Min(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Min(left, right);
            }

            return Min_Software(left, right);
        }

        public static HwVector2S Negate(HwVector2S vector)
            => Negate2D(vector);

        public static HwVector3S Negate(HwVector3S vector)
            => Negate3D(vector);

        public static HwVector4S Negate(HwVector4S vector)
            => Negate4D(vector);


        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Negate2D(in Vector4FParam1_3 vector)
            => Subtract(SingleConstants.Zero, vector);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Negate3D(in Vector4FParam1_3 vector)
            => Subtract(SingleConstants.Zero, vector);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Negate4D(in Vector4FParam1_3 vector)
            => Subtract(SingleConstants.Zero, vector);

        private static readonly HwVectorAnyS SinCoefficient0 = Vector128.Create(-0.16666667f, +0.0083333310f, -0.00019840874f, +2.7525562e-06f);
        private static readonly HwVectorAnyS SinCoefficient1 = Vector128.Create(-2.3889859e-08f, -0.16665852f, +0.0083139502f, -0.00018524670f);
        private static readonly HwVectorAnyS OneElems = Vector128.Create(1f, 1f, 1f, 1f);

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Sin(in Vector4FParam1_3 vector)
        {
            Vector4FParam1_3 vec = Mod2Pi(vector);

            HwVectorAnyS sign = And(vec, SingleConstants.SignFlip4D);
            HwVectorAnyS tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

            HwVectorAnyS abs = AndNot(sign, vec); // Gets the absolute of vector

            HwVectorAnyS neg = Subtract(tmp, vec);

            HwVectorAnyS comp = LessThanOrEqual(abs, SingleConstants.PiDiv2);

            HwVectorAnyS select0 = And(comp, vec);
            HwVectorAnyS select1 = AndNot(comp, neg);

            vec = Or(select0, select1);

            HwVectorAnyS vectorSquared = Multiply(vec, vec);

            // Polynomial approx
            HwVectorAnyS sc1 = SinCoefficient1;
            HwVectorAnyS constants = Permute(sc1, ShuffleValues._0_0_0_0);
            HwVectorAnyS result = Multiply(constants, vectorSquared);

            HwVectorAnyS sc0 = SinCoefficient0;
            constants = Permute(sc0, ShuffleValues._3_3_3_3);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);

            constants = Permute(sc0, ShuffleValues._2_2_2_2);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);

            constants = Permute(sc0, ShuffleValues._1_1_1_1);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);

            constants = Permute(sc0, ShuffleValues._0_0_0_0);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);
            result = Add(result, OneElems);
            result = Multiply(result, vec);

            return result;
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Mod2Pi(in Vector4FParam1_3 vector)
        {
            HwVectorAnyS result = Multiply(vector, SingleConstants.OneDiv2Pi);

            result = Round(result);
            result = Multiply(result, SingleConstants.Pi2);

            return Subtract(vector, result);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Round(in Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNearestInteger(vector);
            }
            // TODO accelerate with SSE

            return SoftwareFallback(vector);

            static HwVectorAnyS SoftwareFallback(in Vector4FParam1_3 vector)
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
        public static HwVectorAnyS Remainder(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            HwVectorAnyS n = Divide(left, right);
            n = Truncate(n);

            HwVectorAnyS y = Multiply(n, right);

            return Subtract(left, y);
        }

        public static HwVectorAnyS Remainder(in Vector4FParam1_3 left, float right)
            => Remainder(left, Vector128.Create(right));

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Truncate(in Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToZero(vector);
            }

            return SoftwareFallback(vector);

            static HwVectorAnyS SoftwareFallback(in Vector4FParam1_3 vector)
            {
                return Vector128.Create(
                    MathF.Truncate(X(vector)),
                    MathF.Truncate(Y(vector)),
                    MathF.Truncate(Z(vector)),
                    MathF.Truncate(W(vector))
                );
            }
        }

        #endregion
    }
}
