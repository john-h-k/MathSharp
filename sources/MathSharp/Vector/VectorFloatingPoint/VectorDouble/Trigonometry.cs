using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4FParam1_3 = Vector256<double>;

    // The bane of every yr11's existence
    // TRIG
    public static partial class Vector
    {
        private static readonly Vector256<double> SinCoefficient0D = Vector256.Create(-0.16666667d, +0.0083333310d, -0.00019840874d, +2.7525562e-06d);
        private static readonly Vector256<double> SinCoefficient1D = Vector256.Create(-2.3889859e-08d, -0.16665852d, +0.0083139502d, -0.00018524670d);
        private const double SinCoefficient1DScalar = -2.3889859e-08d;

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Sin(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                Vector256<double> select0 = SelectWhereTrue(vec, comp);
                Vector256<double> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);

                Vector256<double> vectorSquared = Multiply(vec, vec);

                // Polynomial approx
                Vector256<double> sc0 = SinCoefficient0D;

                Vector256<double> constants = Vector256.Create(SinCoefficient1DScalar);
                Vector256<double> result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(sc0));

                constants = PermuteWithZ(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector256<double> SoftwareFallback(Vector4FParam1_3 vector)
            {
                return Vector256.Create(
                    Math.Sin(X(vector)),
                    Math.Sin(Y(vector)),
                    Math.Sin(Z(vector)),
                    Math.Sin(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> SinApprox(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                Vector256<double> select0 = SelectWhereTrue(vec, comp);
                Vector256<double> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);

                Vector256<double> vectorSquared = Multiply(vec, vec);

                // Fast polynomial approx
                var sc1 = SinCoefficient1D;

                var constants = PermuteWithW(sc1);
                var result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(sc1));

                constants = PermuteWithY(sc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return Sin(vector);
        }

        private static readonly Vector256<double> CosCoefficient0D = Vector256.Create(-0.5d, +0.041666638d, -0.0013888378d, +2.4760495e-05d);
        private static readonly Vector256<double> CosCoefficient1D = Vector256.Create(-2.6051615e-07d, -0.49992746d, +0.041493919d, -0.0012712436d);
        private const double CosCoefficient1DScalar = -2.6051615e-07f;

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Cos(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                Vector256<double> select0 = SelectWhereTrue(vec, comp);
                Vector256<double> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector256<double> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, DoubleConstants.One);
                select1 = AndNot(comp, DoubleConstants.NegativeOne);
                vec = Or(select0, select1);

                // Polynomial approx
                Vector256<double> cc0 = CosCoefficient0D;

                Vector256<double> constants = Vector256.Create(CosCoefficient1DScalar);
                Vector256<double> result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(cc0));

                constants = PermuteWithZ(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector256<double> SoftwareFallback(Vector4FParam1_3 vector)
            {
                return Vector256.Create(
                    Math.Cos(X(vector)),
                    Math.Cos(Y(vector)),
                    Math.Cos(Z(vector)),
                    Math.Cos(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CosApprox(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                Vector256<double> select0 = SelectWhereTrue(vec, comp);
                Vector256<double> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector256<double> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, DoubleConstants.One);
                select1 = AndNot(comp, DoubleConstants.NegativeOne);
                vec = Or(select0, select1);

                // Fast polynomial approx
                var cc1 = CosCoefficient1D;

                var constants = PermuteWithW(cc1);
                var result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(cc1));

                constants = PermuteWithY(cc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return Cos(vector);
        }

        [MethodImpl(MaxOpt)]
        public static void SinCos(Vector4FParam1_3 vector, out Vector256<double> sin, out Vector256<double> cos)
        {
            if (Sse.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                Vector256<double> select0 = SelectWhereTrue(vec, comp);
                Vector256<double> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector256<double> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, DoubleConstants.One);
                select1 = AndNot(comp, DoubleConstants.NegativeOne);
                var cosVec = Or(select0, select1);

                // Polynomial approx
                Vector256<double> sc0 = SinCoefficient0D;

                Vector256<double> constants = Vector256.Create(SinCoefficient1DScalar);
                Vector256<double> result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(sc0));

                constants = PermuteWithZ(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                sin = result;

                // Polynomial approx
                Vector256<double> cc0 = CosCoefficient0D;

                constants = Vector256.Create(CosCoefficient1DScalar);
                result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(cc0));

                constants = PermuteWithZ(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, cosVec);

                cos = result;

                return;
            }

            SoftwareFallback(vector, out sin, out cos);

            static void SoftwareFallback(Vector4FParam1_3 vector, out Vector256<double> sin, out Vector256<double> cos)
            {
                sin = Sin(vector);
                cos = Cos(vector);
            }
        }

        [MethodImpl(MaxOpt)]
        public static void SinCosApprox(Vector4FParam1_3 vector, out Vector256<double> sin, out Vector256<double> cos)
        {
            if (Sse.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                Vector256<double> select0 = SelectWhereTrue(vec, comp);
                Vector256<double> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector256<double> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, DoubleConstants.One);
                select1 = AndNot(comp, DoubleConstants.NegativeOne);
                var cosVec = Or(select0, select1);

                // Fast polynomial approx
                var sc1 = SinCoefficient1D;

                var constants = PermuteWithW(sc1);
                var result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(sc1));

                constants = PermuteWithY(sc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                sin = result;

                // Fast polynomial approx
                var cc1 = CosCoefficient1D;

                constants = PermuteWithW(cc1);
                result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(cc1));

                constants = PermuteWithY(cc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, cosVec);

                cos = result;

                return;
            }

            SinCos(vector, out sin, out cos);
        }
    }
}
