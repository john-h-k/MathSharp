using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class SoftwareFallbacks
    {
        #region Vector Maths

        #region Normalize

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize2D_Software(in Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector4D magnitude = Length2D_Software(vector);
            return Divide_Software(vector, magnitude);
        }



        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize3D_Software(in Vector4DParam1_3 vector)
        {
            Vector4D magnitude = Length3D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize4D_Software(in Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector4D magnitude = Length4D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        #endregion

        #region Length

        [MethodImpl(MaxOpt)]
        public static Vector4D Length2D_Software(in Vector4DParam1_3 vector)
        {
            return Sqrt_Software(DotProduct2D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Length3D_Software(in Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct3D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Length4D_Software(in Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct4D_Software(vector, vector));
        }

        #endregion

        #region LengthSquared

        #endregion

        #region DotProduct

        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct2D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                X(left) * X(right) +
                +Y(left) * Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct3D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                X(left) * X(right)
                + Y(left) * Y(right)
                + Z(left) * Z(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct4D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                X(left) * X(right)
                + Y(left) * Y(right)
                + Z(left) * Z(right)
                + W(left) * W(right)
            );
        }

        #endregion

        #region CrossProduct

        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct2D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {

            return Vector256.Create((X(left) * Y(right) - Y(left) * X(right)));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct3D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            /* Cross product of A(x, y, z, _) and B(x, y, z, _) is
             *
             * '(X = (Ay * Bz) - (Az * By), Y = (Az * Bx) - (Ax * Bz), Z = (Ax * By) - (Ay * Bx)'
             */

            return Vector256.Create(
                Y(left) * Z(right) - Z(left) * Y(right),
                Z(left) * X(right) - X(left) * Z(right),
                X(left) * Y(right) - Y(left) * X(right),
                0
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct4D_Software(in Vector4DParam1_3 one, in Vector4DParam1_3 two, in Vector4DParam1_3 three)
        {
            double x = (Z(two) * W(three) - W(two) * Z(three)) *
                      Y(one) -
                      (Y(two) * W(three) - W(two) * Y(three)) *
                      Z(one) +
                      (Y(two) * Z(three) - Z(two) * Y(three)) *
                      W(one);

            double y = (W(two) * Z(three) - Z(two) * W(three)) *
                      X(one) -
                      (W(two) * X(three) - X(two) * W(three)) *
                      Z(one) +
                      (Z(two) * X(three) - X(two) * Z(three)) *
                      W(one);

            double z = (Y(two) * W(three) - W(two) * Y(three)) *
                      X(one) -
                      (X(two) * W(three) - W(two) * X(three)) *
                      Y(one) +
                      (X(two) * Y(three) - Y(two) * X(three)) *
                      W(one);

            double w = (Z(two) * Y(three) - Y(two) * Z(three)) *
                      X(one) -
                      (Z(two) * X(three) - X(two) * Z(three)) *
                      Y(one) +
                      (Y(two) * X(three) - X(two) * Y(three)) *
                      Z(one);

            return Vector256.Create(x, y, z, w);
        }

        #endregion

        #region Distance

        [MethodImpl(MaxOpt)]
        public static Vector4D Distance2D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            Vector4D diff = Subtract_Software(left, right);

            return Length2D_Software(diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Distance3D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            Vector4D diff = Subtract_Software(left, right);

            return Length3D_Software(diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Distance4D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            Vector4D diff = Subtract_Software(left, right);

            return Length4D_Software(diff);
        }

        #endregion

        #region DistanceSquared

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared2D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            Vector4D diff = Subtract_Software(left, right);

            return DotProduct2D_Software(diff, diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared3D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            Vector4D diff = Subtract_Software(left, right);

            return DotProduct3D_Software(diff, diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared4D_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            Vector4D diff = Subtract_Software(left, right);

            return DotProduct4D_Software(diff, diff);
        }

        #endregion

        #region Lerp

        private static double LerpHelper(double left, double right, double weight)
        {
            return left + (right - left) * weight;
        }

        public static Vector4D Lerp_Software(Vector4D from, Vector4D to, double weight)
        {
            return Vector256.Create(
                LerpHelper(X(from), X(to), weight),
                LerpHelper(Y(from), Y(to), weight),
                LerpHelper(Z(from), Z(to), weight),
                LerpHelper(W(from), W(to), weight)
            );
        }

        #endregion

        #region Reflect

        public static Vector4D Reflect2D_Software(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4D tmp = DotProduct2D_Software(incident, normal);
            tmp = Add_Software(tmp, tmp);
            tmp = Multiply_Software(tmp, normal);
            return Subtract_Software(incident, tmp);
        }

        public static Vector4D Reflect3D_Software(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4D tmp = DotProduct3D_Software(incident, normal);
            tmp = Add_Software(tmp, tmp);
            tmp = Multiply_Software(tmp, normal);
            return Subtract_Software(incident, tmp);
        }

        public static Vector4D Reflect4D_Software(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4D tmp = DotProduct4D_Software(incident, normal);
            tmp = Add_Software(tmp, tmp);
            tmp = Multiply_Software(tmp, normal);
            return Subtract_Software(incident, tmp);
        }

        #endregion

        #endregion
    }
}
