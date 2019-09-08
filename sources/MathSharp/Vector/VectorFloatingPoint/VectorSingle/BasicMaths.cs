using System;
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
    using Vector4F = HwVectorAny;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector4F Permute(in Vector4FParam1_3 vector, byte control)
        {
            if (Avx.IsSupported)
            {
                return Avx.Permute(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Shuffle(in Vector4FParam1_3 left, in Vector4FParam1_3 right, byte control)
        {
            if (Sse.IsSupported)
            {
                return Sse.Shuffle(left, right, control);
            }

            return Shuffle_Software(left, right, control);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Abs(in Vector4FParam1_3 vector)
            => Max(Subtract(Zero, vector), vector);


        [MethodImpl(MaxOpt)]
        public static Vector4F HorizontalAdd(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
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
                Vector4F vector1 = Sse.Shuffle(left, right, ShuffleValues._2_0_2_0);
                Vector4F vector2 = Sse.Shuffle(left, right, ShuffleValues._3_1_3_1);

                return Sse.Add(vector1, vector2);
            }

            return HorizontalAdd_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4F Add(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Add(left, right);
            }

            return Add_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4F Add(in Vector4FParam1_3 vector, float scalar)
            => Add(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector4F Subtract(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4F Subtract(in Vector4FParam1_3 vector, float scalar)
            => Subtract(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector4F Multiply(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Multiply(in Vector4FParam1_3 vector, float scalar)
            => Multiply(vector, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector4F Divide(in Vector4FParam1_3 dividend, in Vector4FParam1_3 divisor)
        {
            if (Sse.IsSupported)
            {
                return Sse.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4F Divide(in Vector4FParam1_3 dividend, float scalarDivisor)
            => Multiply(dividend, Vector128.Create(scalarDivisor));



        [MethodImpl(MaxOpt)]
        public static Vector4F Clamp(in Vector4FParam1_3 vector, in Vector4FParam1_3 low, in Vector4FParam1_3 high)
            => Max(Min(vector, high), low);


        [MethodImpl(MaxOpt)]
        public static Vector4F Sqrt(in Vector4FParam1_3 vector)
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
        public static Vector4F Max(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Max(left, right);
            }

            return Max_Software(left, right);
        }

        // TODO Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded. We should provide a symmetric alternative to this

        [MethodImpl(MaxOpt)]
        public static Vector4F Min(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Min(left, right);
            }

            return Min_Software(left, right);
        }

        public static HwVector2 Negate(HwVector2 vector)
            => Negate2D(vector);

        public static HwVector3 Negate(HwVector3 vector)
            => Negate3D(vector);

        public static HwVector4 Negate(HwVector4 vector)
            => Negate4D(vector);


        [MethodImpl(MaxOpt)]
        internal static Vector4F Negate2D(in Vector4FParam1_3 vector)
            => Xor(vector, SignFlip2D);


        [MethodImpl(MaxOpt)]
        internal static Vector4F Negate3D(in Vector4FParam1_3 vector)
            => Xor(vector, SignFlip3D);



        [MethodImpl(MaxOpt)]
        internal static Vector4F Negate4D(in Vector4FParam1_3 vector)
            => Xor(vector, SignFlip4D);

        private static readonly Vector4F SinCoefficient0 = Vector128.Create(-0.16666667f, +0.0083333310f, -0.00019840874f, +2.7525562e-06f);
        private static readonly Vector4F SinCoefficient1 = Vector128.Create(-2.3889859e-08f, -0.16665852f, +0.0083139502f, -0.00018524670f);
        private static readonly Vector4F OneElems = Vector128.Create(1f, 1f, 1f, 1f);

        [MethodImpl(MaxOpt)]
        public static Vector4F Sin(in Vector4FParam1_3 vector)
        {
            Vector4FParam1_3 vec = Mod2Pi(vector);

            Vector4F sign = And(vec, SignFlip4D);
            Vector4F tmp = Or(Pi, sign); // Pi with the sign from vector

            Vector4F abs = AndNot(sign, vec); // Gets the absolute of vector

            Vector4F neg = Subtract(tmp, vec);

            Vector4F comp = LessThanOrEqual(abs, PiDiv2);

            Vector4F select0 = And(comp, vec);
            Vector4F select1 = AndNot(comp, neg);

            vec = Or(select0, select1);

            Vector4F vectorSquared = Multiply(vec, vec);

            // Polynomial approx
            Vector4F sc1 = SinCoefficient1;
            Vector4F constants = Permute(sc1, ShuffleValues._0_0_0_0);
            Vector4F result = Multiply(constants, vectorSquared);

            Vector4F sc0 = SinCoefficient0;
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
    
        private static readonly Vector4F OneDiv2Pi = Vector128.Create(SingleConstants.OneDiv2Pi);
        private static readonly Vector4F Pi2 = Vector128.Create(SingleConstants.Pi2);
        private static readonly Vector4F Pi = Vector128.Create(SingleConstants.Pi);
        private static readonly Vector4F PiDiv2 = Vector128.Create(SingleConstants.PiDiv2);

        [MethodImpl(MaxOpt)]
        public static Vector4F Mod2Pi(in Vector4FParam1_3 vector)
        {
            Vector4F result = Multiply(vector, OneDiv2Pi);

            result = Round(result);
            result = Multiply(result, Pi2);

            return Subtract(vector, result);
        }
        [MethodImpl(MaxOpt)]
        public static Vector4F Round(in Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNearestInteger(vector);
            }
            // TODO accelerate with SSE

            return SoftwareFallback(vector);

            static Vector4F SoftwareFallback(in Vector4FParam1_3 vector)
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
        public static Vector4F Floor(in Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNegativeInfinity(vector);
            }

            return SoftwareFallback(vector);

            static Vector4F SoftwareFallback(in Vector4FParam1_3 vector)
            {
                return Vector128.Create(
                   MathF.Round(X(vector)),
                   MathF.Round(Y(vector)),
                   MathF.Round(Z(vector)),
                   MathF.Round(W(vector))
               );
            }
        }

        #endregion
    }
}
