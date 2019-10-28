using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Constants;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    

    // The bane of every yr11's existence
    // TRIG
    public static partial class Vector
    {
        private static readonly Vector128<float> SinCoefficient0 = Vector128.Create(-0.16666667f, +0.0083333310f, -0.00019840874f, +2.7525562e-06f);
        private static readonly Vector128<float> SinCoefficient1 = Vector128.Create(-2.3889859e-08f, -0.16665852f, +0.0083139502f, -0.00018524670f);
        private const float SinCoefficient1Scalar = -2.3889859e-08f;
        private static readonly Vector128<float> SinCoefficient1Broadcast = Vector128.Create(SinCoefficient1Scalar);


        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sin(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);
                vec = Select(neg, vec, comp);

                Vector128<float> vectorSquared = Square(vec);

                // Polynomial approx
                Vector128<float> sc0 = SinCoefficient0;

                Vector128<float> constants = SinCoefficient1Broadcast;
                Vector128<float> result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(sc0));

                constants = PermuteWithZ(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector128<float> vector)
            {
                return Vector128.Create(
                    MathF.Sin(X(vector)),
                    MathF.Sin(Y(vector)),
                    MathF.Sin(Z(vector)),
                    MathF.Sin(W(vector))
                );
            }
        }
        [MethodImpl(MaxOpt)]
        public static Vector128<float> SinApprox(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);
                vec = Select(neg, vec, comp);

                Vector128<float> vectorSquared = Square(vec);

                // Fast polynomial approx
                var sc1 = SinCoefficient1;

                var constants = PermuteWithW(sc1);
                var result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(sc1));

                constants = PermuteWithY(sc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return Sin(vector);
        }


        private static readonly Vector128<float> CosCoefficient0 = Vector128.Create(-0.5f, +0.041666638f, -0.0013888378f, +2.4760495e-05f);
        private static readonly Vector128<float> CosCoefficient1 = Vector128.Create(-2.6051615e-07f, -0.49992746f, +0.041493919f, -0.0012712436f);
        private const float CosCoefficient1Scalar = -2.6051615e-07f;

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Cos(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                vec = Select(neg, vec, comp);

                Vector128<float> vectorSquared = Square(vec);

                vec = Select(SingleConstants.NegativeOne, SingleConstants.One, comp);

                // Polynomial approx
                Vector128<float> cc0 = CosCoefficient0;

                Vector128<float> constants = Vector128.Create(CosCoefficient1Scalar);
                Vector128<float> result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(cc0));

                constants = PermuteWithZ(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector128<float> vector)
            {
                return Vector128.Create(
                    MathF.Cos(X(vector)),
                    MathF.Cos(Y(vector)),
                    MathF.Cos(Z(vector)),
                    MathF.Cos(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CosApprox(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                vec = Select(neg, vec, comp);

                Vector128<float> vectorSquared = Square(vec);

                vec = Select(SingleConstants.NegativeOne, SingleConstants.One, comp);

                // Fast polynomial approx
                var cc1 = CosCoefficient1;

                var constants = PermuteWithW(cc1);
                var result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(cc1));

                constants = PermuteWithY(cc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, vec);

                return result;
            }

            return Cos(vector);
        }

        private static readonly Vector128<float> TanCoefficients0 = Vector128.Create(1.0f, -4.667168334e-1f, 2.566383229e-2f, -3.118153191e-4f);
        private static readonly Vector128<float> TanCoefficients1 = Vector128.Create(4.981943399e-7f, -1.333835001e-1f, 3.424887824e-3f, -1.786170734e-5f);
        private static readonly Vector128<float> TanConstants = Vector128.Create(1.570796371f, 6.077100628e-11f, 0.000244140625f, 0.63661977228f);
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Tan(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                var twoDivPi = PermuteWithW(TanConstants);

                var tc0 = PermuteWithX(TanConstants);
                var tc1 = PermuteWithY(TanConstants);
                var epsilon = PermuteWithZ(TanConstants);

                var va = Multiply(vector, twoDivPi);
                va = Round(va);

                var vc = FastNegateMultiplyAdd(va, tc0, vector);

                var vb = Abs(va);

                vc = FastNegateMultiplyAdd(va, tc1, vc);

                vb = ConvertToInt32(vb).AsSingle();

                var vc2 = Square(vc);

                var t7 = PermuteWithW(TanCoefficients1);
                var t6 = PermuteWithZ(TanCoefficients1);
                var t4 = PermuteWithX(TanCoefficients1);
                var t3 = PermuteWithW(TanCoefficients0);
                var t5 = PermuteWithY(TanCoefficients1);
                var t2 = PermuteWithZ(TanCoefficients0);
                var t1 = PermuteWithY(TanCoefficients0);
                var t0 = PermuteWithX(TanCoefficients0);

                var vbIsEven = And(vb, SingleConstants.Epsilon).AsInt32();
                vbIsEven = CompareBitwiseEqualInt32(vbIsEven, Vector128<int>.Zero);

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
                d = Select(d, SingleConstants.One, nearZero);

                var r0 = Negate(n);
                var r1 = Divide(n, d);
                r0 = Divide(d, r0);

                var isZero = CompareEqual(vector, Vector128<float>.Zero);

                var result = Select(r0, r1, vbIsEven);

                result = Select(result, Vector128<float>.Zero, isZero);

                return result;
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector128<float> vector)
            {
                return Vector128.Create(
                    MathF.Tan(X(vector)),
                    MathF.Tan(Y(vector)),
                    MathF.Tan(Z(vector)),
                    MathF.Tan(W(vector))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        private static Vector128<int> CompareBitwiseEqualInt32(Vector128<int> left, Vector128<int> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector128<int> SoftwareFallback(Vector128<int> left, Vector128<int> right)
            {
                return Vector128.Create(
                    BoolToSimdBoolInt32(X(left) == X(right)),
                    BoolToSimdBoolInt32(Y(left) == Y(right)),
                    BoolToSimdBoolInt32(Z(left) == Z(right)),
                    BoolToSimdBoolInt32(W(left) == W(right))
                );
            }
        }

        private static readonly Vector128<float> TanEstCoefficients = Vector128.Create(2.484f, -1.954923183e-1f, 2.467401101f, ScalarSingleConstants.OneDivPi);
        [MethodImpl(MaxOpt)]
        public static Vector128<float> TanApprox(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                var oneDivPi = PermuteWithW(TanEstCoefficients);

                var v1 = Multiply(vector, oneDivPi);
                v1 = Round(v1);

                v1 = FastNegateMultiplyAdd(SingleConstants.Pi, v1, vector);

                var t0 = PermuteWithX(TanEstCoefficients);
                var t1 = PermuteWithY(TanEstCoefficients);
                var t2 = PermuteWithZ(TanEstCoefficients);

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

        [MethodImpl(MaxOpt)]
        public static void SinCos(Vector128<float> vector, out Vector128<float> sin, out Vector128<float> cos)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                vec = Select(neg, vec, comp);

                Vector128<float> vectorSquared = Square(vec);

                var cosVec = Select(SingleConstants.NegativeOne, SingleConstants.One, comp);


                // Polynomial approx
                Vector128<float> sc0 = SinCoefficient0;

                Vector128<float> constants = Vector128.Create(SinCoefficient1Scalar);
                Vector128<float> result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(sc0));

                constants = PermuteWithZ(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(sc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, vec);

                sin = result;

                // Polynomial approx
                Vector128<float> cc0 = CosCoefficient0;

                constants = Vector128.Create(CosCoefficient1Scalar);
                result = FastMultiplyAdd(constants, vectorSquared, PermuteWithW(cc0));

                constants = PermuteWithZ(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithY(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                constants = PermuteWithX(cc0);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, cosVec);

                cos = result;

                return;
            }

            SoftwareFallback(vector, out sin, out cos);

            static void SoftwareFallback(Vector128<float> vector, out Vector128<float> sin, out Vector128<float> cos)
            {
                sin = Sin(vector);
                cos = Cos(vector);
            }
        }

        [MethodImpl(MaxOpt)]
        public static void SinCosApprox(Vector128<float> vector, out Vector128<float> sin, out Vector128<float> cos)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                vec = Select(neg, vec, comp);
                Vector128<float> vectorSquared = Square(vec);

                var cosVec = Select(SingleConstants.NegativeOne, SingleConstants.One, comp);


                // Fast polynomial approx
                var sc1 = SinCoefficient1;

                var constants = PermuteWithW(sc1);
                var result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(sc1));

                constants = PermuteWithY(sc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, vec);

                sin = result;

                // Fast polynomial approx
                var cc1 = CosCoefficient1;

                constants = PermuteWithW(cc1);
                result = FastMultiplyAdd(constants, vectorSquared, PermuteWithZ(cc1));

                constants = PermuteWithY(cc1);
                result = FastMultiplyAdd(result, vectorSquared, constants);

                result = FastMultiplyAdd(result, vectorSquared, SingleConstants.One);

                result = Multiply(result, cosVec);

                cos = result;

                return;
            }

            SinCos(vector, out sin, out cos);
        }

        private static readonly Vector128<float> ATan2Constants = Vector128.Create(
                ScalarSingleConstants.Pi,
                ScalarSingleConstants.PiDiv2,
                ScalarSingleConstants.PiDiv4,
                ScalarSingleConstants.Pi * 3.0f / 4.0f
        );

        public static Vector128<float> ATan2(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {
                var aTanResultValid = SingleConstants.AllBitsSet;

                var pi = SingleConstants.Pi;
                var piDiv2 = SingleConstants.PiDiv2;
                var piDiv4 = SingleConstants.PiDiv4;
                var threePiDiv4 = SingleConstants.ThreePiDiv4;

                var yEqualsZero = CompareEqual(left, SingleConstants.Zero);
                var xEqualsZero = CompareEqual(right, SingleConstants.Zero);
                var rightIsPositive = ExtractSign(right);
                rightIsPositive = CompareBitwiseEqualInt32(rightIsPositive.AsInt32(), Vector128<int>.Zero).AsSingle();
                var yEqualsInfinity = IsInfinite(left);
                var xEqualsInfinity = IsInfinite(right);

                var ySign = And(left, SingleConstants.NegativeZero);
                pi = Or(pi, ySign);
                piDiv2 = Or(piDiv2, ySign);
                piDiv4 = Or(piDiv4, ySign);
                threePiDiv4 = Or(threePiDiv4, ySign);

                var r1 = Select(pi, ySign, rightIsPositive);
                var r2 = Select(aTanResultValid, piDiv2, xEqualsZero);
                var r3 = Select(r2, r1, yEqualsZero);
                var r4 = Select(threePiDiv4, piDiv4, rightIsPositive);
                var r5 = Select(piDiv2, r4, xEqualsInfinity);
                var result = Select(r3, r5, yEqualsInfinity);
                aTanResultValid = CompareBitwiseEqualInt32(result.AsInt32(), aTanResultValid.AsInt32()).AsSingle();

                var v = Divide(left, right);

                var r0 = ATan(v);

                r1 = Select(pi, SingleConstants.NegativeZero, rightIsPositive);
                r2 = Add(r0, r1);

                return Select(result, r2, aTanResultValid);
            }

            return SoftwareFallback(left, right);

            static Vector128<float> SoftwareFallback(Vector128<float> left, Vector128<float> right)
            {
                return Vector128.Create(
                    MathF.Atan2(X(left), X(right)),
                    MathF.Atan2(Y(left), Y(right)),
                    MathF.Atan2(Z(left), Z(right)),
                    MathF.Atan2(W(left), W(right))
                );
            }
        }

        public static readonly Vector128<float> ATanCoefficients0 = Vector128.Create(-0.3333314528f, +0.1999355085f, -0.1420889944f, +0.1065626393f);
        public static readonly Vector128<float> ATanCoefficients1 = Vector128.Create(-0.0752896400f, +0.0429096138f, -0.0161657367f, +0.0028662257f);
        public static Vector128<float> ATan(Vector128<float> vector)
        {
            var abs = Abs(vector);
            var inv = Divide(SingleConstants.One, vector);
            var comp = CompareGreaterThan(vector, SingleConstants.One);
            var sign = Select(SingleConstants.NegativeOne, SingleConstants.One, comp);

            comp = CompareLessThanOrEqual(abs, SingleConstants.One);

            sign = Select(sign, SingleConstants.Zero, comp);

            var vec = Select(inv, vector, comp);

            var vecSquared = Square(vec);

            var tc1 = ATanCoefficients1;

            var constants1 = PermuteWithZ(tc1);

            var result = FastMultiplyAdd(PermuteWithW(tc1), vecSquared, constants1);

            constants1 = PermuteWithY(tc1);

            result = FastMultiplyAdd(result, vecSquared, constants1);

            constants1 = PermuteWithX(tc1);

            result = FastMultiplyAdd(result, vecSquared, constants1);

            var tC0 = ATanCoefficients0;
            constants1 = PermuteWithW(tC0);

            result = FastMultiplyAdd(result, vecSquared, constants1);

            constants1 = PermuteWithZ(tC0);

            result = FastMultiplyAdd(result, vecSquared, constants1);

            constants1 = PermuteWithY(tC0);

            result = FastMultiplyAdd(result, vecSquared, constants1);

            constants1 = PermuteWithX(tC0);

            result = FastMultiplyAdd(result, vecSquared, constants1);

            result = FastMultiplyAdd(result, vecSquared, SingleConstants.One);

            result = Multiply(result, vec);

            var result1 = FastMultiplySubtract(sign, SingleConstants.PiDiv2, result);

            comp = CompareEqual(sign, SingleConstants.Zero);
            result = Select(result1, result, comp);
            return result;
        }
    }
}
