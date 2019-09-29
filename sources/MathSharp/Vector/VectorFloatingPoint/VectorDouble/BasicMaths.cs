using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class Vector
    {
        #region Vector

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD Permute(in Vector4DParam1_3 vector, byte control)
        {
            if (Avx.IsSupported)
            {
                return Avx.Permute(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD Shuffle(in Vector4DParam1_3 left, in Vector4DParam1_3 right, byte control)
        {
            if (Sse.IsSupported)
            {
                return Avx.Shuffle(left, right, control);
            }

            return Shuffle_Software(left, right, control);
        }

        private static void GetLowHigh(Vector4DParam1_3 vector, out Vector128<double> low, out Vector128<double> high)
        {
            low = vector.GetLower();
            high = vector.GetUpper();
        }

        private static Vector256<double> FromLowHigh(Vector128<double> low, Vector128<double> high)
            => Vector256.Create(low, high);

        [MethodImpl(MaxOpt)]
        public static Vector4D Abs(in Vector4DParam1_3 vector)
            => Max(Subtract(Vector4D.Zero, vector), vector);


        [MethodImpl(MaxOpt)]
        public static Vector4D HorizontalAdd(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.HorizontalAdd(left, right);
            }

            return HorizontalAdd_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Add(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Add(left, right);
            }

            return Add_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Add(in Vector4DParam1_3 vector, double scalar)
            => Add(vector, Vector256.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract(in Vector4DParam1_3 vector, double scalar)
            => Subtract(vector, Vector256.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply(in Vector4DParam1_3 vector, double scalar)
            => Multiply(vector, Vector256.Create(scalar));

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide(in Vector4DParam1_3 dividend, in Vector4DParam1_3 divisor)
        {
            if (Avx.IsSupported)
            {
                return Avx.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Divide(in Vector4DParam1_3 dividend, double scalarDivisor)
            => Subtract(dividend, Vector256.Create(scalarDivisor));


        public static Vector4D Clamp(in Vector4DParam1_3 vector, in Vector4DParam1_3 low, in Vector4DParam1_3 high)
            => Max(Min(vector, high), low);


        [MethodImpl(MaxOpt)]
        public static Vector4D Sqrt(in Vector4DParam1_3 vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.Sqrt(vector);
            }

            return Sqrt_Software(vector);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Max(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Max(left, right);
            }

            return Max_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Min(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Min(left, right);
            }

            return Min_Software(left, right);
        }

        public static HwVector2D Negate(HwVector2D vector)
            => Negate2D(vector);

        public static HwVector3D Negate(HwVector3D vector)
            => Negate3D(vector);

        public static HwVector4D Negate(HwVector4D vector)
            => Negate4D(vector);


        [MethodImpl(MaxOpt)]
        public static Vector4D Negate2D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip2DDouble);


        [MethodImpl(MaxOpt)]
        public static Vector4D Negate3D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip3DDouble);


        [MethodImpl(MaxOpt)]
        public static Vector4D Negate4D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip4DDouble);

        private static readonly HwVectorAnyD SinCoefficient0D = Vector256.Create(-0.16666667d, +0.0083333310d, -0.00019840874d, +2.7525562e-06d);
        private static readonly HwVectorAnyD SinCoefficient1D = Vector256.Create(-2.3889859e-08d, -0.16665852d, +0.0083139502d, -0.00018524670d);
        private static readonly HwVectorAnyD OneElemsD = Vector256.Create(1d, 1d, 1d, 1d);

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD Sin(in Vector4DParam1_3 vector)
        {
            Vector4DParam1_3 vec = Mod2Pi(vector);

            var sign = And(vec, DoubleConstants.SignFlip4D);
            var tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

            var abs = AndNot(sign, vec); // Gets the absolute of vector

            var neg = Subtract(tmp, vec);

            var comp = LessThanOrEqual(abs, DoubleConstants.PiDiv2);

            var select0 = And(comp, vec);
            var select1 = AndNot(comp, neg);

            vec = Or(select0, select1);

            var vectorSquared = Multiply(vec, vec);

            // Polynomial approx
            var sc1 = SinCoefficient1D;
            var constants = Permute(sc1, ShuffleValues._0_0_0_0);
            var result = Multiply(constants, vectorSquared);

            var sc0 = SinCoefficient0D;
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
            result = Add(result, OneElemsD);
            result = Multiply(result, vec);

            return result;
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD Mod2Pi(in Vector4DParam1_3 vector)
        {
            HwVectorAnyD result = Multiply(vector, DoubleConstants.OneDiv2Pi);

            result = Round(result);
            result = Multiply(result, DoubleConstants.Pi2);

            return Subtract(vector, result);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD Round(in Vector4DParam1_3 vector)
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

            static HwVectorAnyD SoftwareFallback(in Vector4DParam1_3 vector)
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
        public static Vector4D Remainder(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            Vector4D n = Divide(left, right);
            n = Truncate(n);

            Vector4D y = Multiply(n, right);

            return Subtract(left, y);
        }

        public static Vector4D Remainder(in Vector4DParam1_3 left, double right)
            => Remainder(left, Vector256.Create(right));

        [MethodImpl(MaxOpt)]
        public static Vector4D Truncate(in Vector4DParam1_3 vector)
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

            static Vector4D SoftwareFallback(in Vector4DParam1_3 vector)
            {
                return Vector256.Create(
                    Math.Truncate(X(vector)),
                    Math.Truncate(Y(vector)),
                    Math.Truncate(Z(vector)),
                    Math.Truncate(W(vector))
                );
            }
        }

        #endregion
    }
}
