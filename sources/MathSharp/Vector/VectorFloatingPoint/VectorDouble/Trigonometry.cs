using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Constants;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    

    // The bane of every yr11's existence
    // TRIG
    public static partial class Vector
    {
        private static readonly Vector256<double> SinCoefficient0D = Vector256.Create(-0.16666667d, +0.0083333310d, -0.00019840874d, +2.7525562e-06d);
        private static readonly Vector256<double> SinCoefficient1D = Vector256.Create(-2.3889859e-08d, -0.16665852d, +0.0083139502d, -0.00018524670d);
        private const double SinCoefficient1DScalar = -2.3889859e-08d;

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <returns>The sine of the given angle.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Sin(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);
                vec = Select(neg, vec, comp);

                Vector256<double> vectorSquared = Square(vec);

                // Polynomial approx
                Vector256<double> sc0 = SinCoefficient0D;

                Vector256<double> constants = Vector256.Create(SinCoefficient1DScalar);
                Vector256<double> result = FastMultiplyAdd(constants, vectorSquared, FillWithW(sc0));

                constants = FillWithZ(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithY(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithX(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector256<double> SoftwareFallback(Vector256<double> vector)
            {
                return Vector256.Create(
                    Math.Sin(X(vector)),
                    Math.Sin(Y(vector)),
                    Math.Sin(Z(vector)),
                    Math.Sin(W(vector))
                );
            }
        }

        /// <summary>
        /// Returns the approximate sine of the specified angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <returns>The approximate sine of the given angle.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> SinApprox(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);
                vec = Select(neg, vec, comp);

                Vector256<double> vectorSquared = Square(vec);

                // Fast polynomial approx
                var sc1 = SinCoefficient1D;

                var constants = FillWithW(sc1);
                var result = FastMultiplyAdd(constants, vectorSquared, FillWithZ(sc1));

                constants = FillWithY(sc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return Sin(vector);
        }

        private static readonly Vector256<double> CosCoefficient0D = Vector256.Create(-0.5d, +0.041666638d, -0.0013888378d, +2.4760495e-05d);
        private static readonly Vector256<double> CosCoefficient1D = Vector256.Create(-2.6051615e-07d, -0.49992746d, +0.041493919d, -0.0012712436d);
        private const double CosCoefficient1DScalar = -2.6051615e-07d;

        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <returns>The cosine of the given angle.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Cos(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                vec = Select(neg, vec, comp);

                Vector256<double> vectorSquared = Square(vec);

                vec = Select(DoubleConstants.NegativeOne, DoubleConstants.One, comp);

                // Polynomial approx
                Vector256<double> cc0 = CosCoefficient0D;

                Vector256<double> constants = Vector256.Create(CosCoefficient1DScalar);
                Vector256<double> result = FastMultiplyAdd(constants, vectorSquared, FillWithW(cc0));

                constants = FillWithZ(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithY(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithX(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector256<double> SoftwareFallback(Vector256<double> vector)
            {
                return Vector256.Create(
                    Math.Cos(X(vector)),
                    Math.Cos(Y(vector)),
                    Math.Cos(Z(vector)),
                    Math.Cos(W(vector))
                );
            }
        }

        /// <summary>
        /// Returns the approximate cosine of the specified angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <returns>The approximate cosine of the given angle.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CosApprox(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                vec = Select(neg, vec, comp);

                Vector256<double> vectorSquared = Square(vec);

                vec = Select(DoubleConstants.NegativeOne, DoubleConstants.One, comp);

                // Fast polynomial approx
                var cc1 = CosCoefficient1D;

                var constants = FillWithW(cc1);
                var result = FastMultiplyAdd(constants, vectorSquared, FillWithZ(cc1));

                constants = FillWithY(cc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return Cos(vector);
        }

        private static readonly Vector256<double> TanCoefficients0D = Vector256.Create(1.0d, -4.667168334e-1d, 2.566383229e-2d, -3.118153191e-4d);
        private static readonly Vector256<double> TanCoefficients1D = Vector256.Create(4.981943399e-7d, -1.333835001e-1d, 3.424887824e-3d, -1.786170734e-5d);
        private static readonly Vector256<double> TanConstantsD = Vector256.Create(1.570796371d, 6.077100628e-11d, 0.000244140625d, 0.63661977228d);

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <returns>The tangent of the given angle.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Tan(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                var twoDivPi = FillWithW(TanConstantsD);

                var tc0 = FillWithX(TanConstantsD);
                var tc1 = FillWithY(TanConstantsD);
                var epsilon = FillWithZ(TanConstantsD);

                var va = Multiply(vector, twoDivPi);
                va = Round(va);

                var vc = FastNegateMultiplyAdd(va, tc0, vector);

                var vb = Abs(va);

                vc = FastNegateMultiplyAdd(va, tc1, vc);

                vb = ConvertToInt64(vb).AsDouble();

                var vc2 = Square(vc);

                var t7 = FillWithW(TanCoefficients1D);
                var t6 = FillWithZ(TanCoefficients1D);
                var t4 = FillWithX(TanCoefficients1D);
                var t3 = FillWithW(TanCoefficients0D);
                var t5 = FillWithY(TanCoefficients1D);
                var t2 = FillWithZ(TanCoefficients0D);
                var t1 = FillWithY(TanCoefficients0D);
                var t0 = FillWithX(TanCoefficients0D);

                var vbIsEven = And(vb, DoubleConstants.Epsilon).AsInt64();
                vbIsEven = CompareEqual(vbIsEven, Vector256<long>.Zero);

                var n = FastMultiplyAdd(vc2, t7, t6);
                var d = FastMultiplyAdd(vc2, t4, t3);
                n = FastMultiplyAdd(vc2, n, t5);
                d = FastMultiplyAdd(vc2, d, t2);
                n = Multiply(vc2, n);
                d = FastMultiplyAdd(vc2, d, t1);
                n = FastMultiplyAdd(vc, n, vc);

                var nearZero = InBounds(vc, epsilon);

                d = FastMultiplyAdd(vc2, d, t0);

                n = Select(n, vc, nearZero);
                d = Select(d, DoubleConstants.One, nearZero);

                var r0 = Negate(n);
                var r1 = Divide(n, d);
                r0 = Divide(d, r0);

                var isZero = CompareEqual(vector, Vector256<double>.Zero);

                var result = Select(r0, r1, vbIsEven);

                result = Select(result, Vector256<double>.Zero, isZero);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector256<double> SoftwareFallback(Vector256<double> vector)
            {
                return Vector256.Create(
                    Math.Tan(X(vector)),
                    Math.Tan(Y(vector)),
                    Math.Tan(Z(vector)),
                    Math.Tan(W(vector))
                );
            }
        }

        private static readonly Vector256<double> TanEstCoefficientsD = Vector256.Create(2.484d, -1.954923183e-1d, 2.467401101d, ScalarDoubleConstants.OneDivPi);        

        /// <summary>
        /// Returns the approximate tangent of the specified angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <returns>The approximate tangent of the given angle.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> TanApprox(Vector256<double> vector)
        {
            if (Avx.IsSupported)
            {
                var oneDivPi = FillWithW(TanEstCoefficientsD);

                var v1 = Multiply(vector, oneDivPi);
                v1 = Round(v1);

                v1 = FastNegateMultiplyAdd(DoubleConstants.Pi, v1, vector);

                var t0 = FillWithX(TanEstCoefficientsD);
                var t1 = FillWithY(TanEstCoefficientsD);
                var t2 = FillWithZ(TanEstCoefficientsD);

                var v2T2 = FastNegateMultiplyAdd(v1, v1, t2);
                var v2 = Square(v1);
                var v1T0 = Multiply(v1, t0);
                var v1T1 = Multiply(v1, t1);

                var d = ReciprocalApprox(v2T2);
                var n = FastMultiplyAdd(v2, v1T1, v1T0);

                return Multiply(n, d);
            }

            return Tan(vector);
        }

        /// <summary>
        /// Calculates both the sine and cosine of the given angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <param name="sin">The sine of the angle.</param>
        /// <param name="cos">The cosine of the angle.</param>
        [MethodImpl(MaxOpt)]
        public static void SinCos(Vector256<double> vector, out Vector256<double> sin, out Vector256<double> cos)
        {
            if (Avx.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                vec = Select(neg, vec, comp);

                Vector256<double> vectorSquared = Square(vec);

                var cosVec = Select(DoubleConstants.NegativeOne, DoubleConstants.One, comp);


                // Polynomial approx
                Vector256<double> sc0 = SinCoefficient0D;

                Vector256<double> constants = Vector256.Create(SinCoefficient1DScalar);
                Vector256<double> result = FastMultiplyAdd(constants, vectorSquared, FillWithW(sc0));

                constants = FillWithZ(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithY(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithX(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                sin = result;

                // Polynomial approx
                Vector256<double> cc0 = CosCoefficient0D;

                constants = Vector256.Create(CosCoefficient1DScalar);
                result = FastMultiplyAdd(constants, vectorSquared, FillWithW(cc0));

                constants = FillWithZ(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithY(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = FillWithX(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, cosVec);

                cos = result;

                return;
            }

            SoftwareFallback(vector, out sin, out cos);

            static void SoftwareFallback(Vector256<double> vector, out Vector256<double> sin, out Vector256<double> cos)
            {
                sin = Sin(vector);
                cos = Cos(vector);
            }
        }
        /// <summary>
        /// Calculates both the approximate sine and cosine of the given angle.
        /// </summary>
        /// <param name="vector">The angle.</param>
        /// <param name="sin">The approximate sine of the angle.</param>
        /// <param name="cos">The approximate cosine of the angle.</param>
        [MethodImpl(MaxOpt)]
        public static void SinCosApprox(Vector256<double> vector, out Vector256<double> sin, out Vector256<double> cos)
        {
            if (Avx.IsSupported)
            {
                Vector256<double> vec = Mod2Pi(vector);

                Vector256<double> sign = ExtractSign(vec);
                Vector256<double> tmp = Or(DoubleConstants.Pi, sign); // Pi with the sign from vector

                Vector256<double> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector256<double> neg = Subtract(tmp, vec);

                Vector256<double> comp = CompareLessThanOrEqual(abs, DoubleConstants.PiDiv2);

                vec = Select(neg, vec, comp);
                Vector256<double> vectorSquared = Square(vec);

                var cosVec = Select(DoubleConstants.NegativeOne, DoubleConstants.One, comp);


                // Fast polynomial approx
                var sc1 = SinCoefficient1D;

                var constants = FillWithW(sc1);
                var result = FastMultiplyAdd(constants, vectorSquared, FillWithZ(sc1));

                constants = FillWithY(sc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, DoubleConstants.One);

                result = Multiply(result, vec);

                sin = result;

                // Fast polynomial approx
                var cc1 = CosCoefficient1D;

                constants = FillWithW(cc1);
                result = FastMultiplyAdd(constants, vectorSquared, FillWithZ(cc1));

                constants = FillWithY(cc1);
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
