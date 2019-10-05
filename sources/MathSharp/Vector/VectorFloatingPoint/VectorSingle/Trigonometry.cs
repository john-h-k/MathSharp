using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
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
        public static Vector128<float> SinEstimate(Vector4FParam1_3 vector)
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

        private static readonly Vector128<float> 
            CosCoefficient0 = Vector128.Create(-0.5f, +0.041666638f, -0.0013888378f, +2.4760495e-05f);
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
        public static Vector128<float> CosEstimate(Vector4FParam1_3 vector)
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

        public static void SinCosEstimate(Vector4FParam1_3 vector, out Vector128<float> sin, out Vector128<float> cos)
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
