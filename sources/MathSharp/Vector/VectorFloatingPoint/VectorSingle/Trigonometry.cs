using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Constants;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4FParam1_3 = Vector128<float>;

    // The bane of every yr11's existence
    // TRIG
    public static partial class Vector
    {
        private static readonly Vector128<float> SinCoefficient0 = Vector128.Create(-0.16666667f, +0.0083333310f, -0.00019840874f, +2.7525562e-06f);
        private static readonly Vector128<float> SinCoefficient1 = Vector128.Create(-2.3889859e-08f, -0.16665852f, +0.0083139502f, -0.00018524670f);
        private const float SinCoefficient1Scalar = -2.3889859e-08f;

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sin(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                Vector128<float> select0 = SelectWhereTrue(vec, comp);
                Vector128<float> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);

                Vector128<float> vectorSquared = Multiply(vec, vec);

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

                return result;
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 vector)
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
        public static Vector128<float> SinApprox(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                Vector128<float> select0 = SelectWhereTrue(vec, comp);
                Vector128<float> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);

                Vector128<float> vectorSquared = Multiply(vec, vec);

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
        public static Vector128<float> Cos(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                Vector128<float> select0 = SelectWhereTrue(vec, comp);
                Vector128<float> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector128<float> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, SingleConstants.One);
                select1 = AndNot(comp, SingleConstants.NegativeOne);
                vec = Or(select0, select1);

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

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 vector)
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
        public static Vector128<float> CosApprox(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                Vector128<float> select0 = SelectWhereTrue(vec, comp);
                Vector128<float> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector128<float> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, SingleConstants.One);
                select1 = AndNot(comp, SingleConstants.NegativeOne);
                vec = Or(select0, select1);

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

        private static readonly Vector128<float> TanCoefficients0 =  Vector128.Create(1.0f, -4.667168334e-1f, 2.566383229e-2f, -3.118153191e-4f);
        private static readonly Vector128<float> TanCoefficients1 = Vector128.Create(4.981943399e-7f, -1.333835001e-1f, 3.424887824e-3f, -1.786170734e-5f);
        private static readonly Vector128<float> TanConstants = Vector128.Create(1.570796371f, 6.077100628e-11f, 0.000244140625f, 0.63661977228f);
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Tan(Vector4FParam1_3 vector)
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

            var vc2 = Multiply(vc, vc);

            var t7 = PermuteWithW(TanCoefficients1);
            var t6 = PermuteWithZ(TanCoefficients1);
            var t4 = PermuteWithX(TanCoefficients1);
            var t3 = PermuteWithW(TanCoefficients0);
            var t5 = PermuteWithY(TanCoefficients1);
            var t2 = PermuteWithZ(TanCoefficients0);
            var t1 = PermuteWithY(TanCoefficients0);
            var t0 = PermuteWithX(TanCoefficients0);

            var vbIsEven = And(vb, SingleConstants.Epsilon).AsInt32();
            vbIsEven = CompareEqual(vbIsEven, Vector128<int>.Zero);

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

        private static readonly Vector128<float> TanEstCoefficients = Vector128.Create(2.484f, -1.954923183e-1f, 2.467401101f, ScalarSingleConstants.OneDivPi);
        [MethodImpl(MaxOpt)]
        public static Vector128<float> TanApprox(Vector4FParam1_3 vector)
        {
            var oneDivPi = PermuteWithZ(TanEstCoefficients);

            var v1 = Multiply(vector, oneDivPi);
            v1 = Round(v1);

            v1 = FastNegateMultiplyAdd(SingleConstants.Pi, v1, vector);

            var t0 = PermuteWithX(TanEstCoefficients);
            var t1 = PermuteWithY(TanEstCoefficients);
            var t2 = PermuteWithZ(TanEstCoefficients);

            var v2T2 = FastNegateMultiplyAdd(v1, v1, t2);
            var v2 = Multiply(v1, v1);
            var v1T0 = Multiply(v1, t0);
            var v1T1 = Multiply(v1, t1);

            var d = ReciprocalApprox(v2T2);
            var n = FastMultiplyAdd(v2, v1T1, v1T0);

            return Multiply(n, d);
        }

        [MethodImpl(MaxOpt)]
        public static void SinCos(Vector4FParam1_3 vector, out Vector128<float> sin, out Vector128<float> cos)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                Vector128<float> select0 = SelectWhereTrue(vec, comp);
                Vector128<float> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector128<float> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, SingleConstants.One);
                select1 = AndNot(comp, SingleConstants.NegativeOne);
                var cosVec = Or(select0, select1);

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

            static void SoftwareFallback(Vector4FParam1_3 vector, out Vector128<float> sin, out Vector128<float> cos)
            {
                sin = Sin(vector);
                cos = Cos(vector);
            }
        }

        [MethodImpl(MaxOpt)]
        public static void SinCosApprox(Vector4FParam1_3 vector, out Vector128<float> sin, out Vector128<float> cos)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> vec = Mod2Pi(vector);

                Vector128<float> sign = ExtractSign(vec);
                Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

                Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

                Vector128<float> neg = Subtract(tmp, vec);

                Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

                Vector128<float> select0 = SelectWhereTrue(vec, comp);
                Vector128<float> select1 = SelectWhereFalse(neg, comp);

                vec = Or(select0, select1);
                Vector128<float> vectorSquared = Multiply(vec, vec);

                select0 = And(comp, SingleConstants.One);
                select1 = AndNot(comp, SingleConstants.NegativeOne);
                var cosVec = Or(select0, select1);

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
    }
}
