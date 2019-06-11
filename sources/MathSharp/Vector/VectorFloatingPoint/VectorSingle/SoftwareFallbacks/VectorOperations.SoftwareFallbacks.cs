using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class SoftwareFallbacks
    {
        #region Vector Maths

        #region Normalize

        [MethodImpl(MaxOpt)]
        public static Vector4F Normalize2D_Software(Vector4FParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector4F magnitude = Length2D_Software(vector);
            return Divide_Software(vector, magnitude);
        }



        [MethodImpl(MaxOpt)]
        public static Vector4F Normalize3D_Software(Vector4FParam1_3 vector)
        {
            Vector4F magnitude = Length3D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Normalize4D_Software(Vector4FParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector4F magnitude = Length4D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        #endregion

        #region Length

        [MethodImpl(MaxOpt)]
        public static Vector4F Length2D_Software(Vector4FParam1_3 vector)
        {
            return Sqrt_Software(DotProduct2D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Length3D_Software(Vector4FParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct3D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Length4D_Software(Vector4FParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct4D_Software(vector, vector));
        }

        #endregion

        #region LengthSquared

        #endregion

        #region DotProduct

        [MethodImpl(MaxOpt)]
        public static Vector4F DotProduct2D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right) +
                +Y(left) * Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F DotProduct3D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right)
                + Y(left) * Y(right)
                + Z(left) * Z(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F DotProduct4D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right)
                + Y(left) * Y(right)
                + Z(left) * Z(right)
                + W(left) * W(right)
            );
        }

        #endregion

        #region CrossProduct

        [MethodImpl(MaxOpt)]
        public static Vector4F CrossProduct2D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {

            return Vector128.Create((X(left) * Y(right) - Y(left) * X(right)));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F CrossProduct3D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            /* Cross product of A(x, y, z, _) and B(x, y, z, _) is
             *
             * '(X = (Ay * Bz) - (Az * By), Y = (Az * Bx) - (Ax * Bz), Z = (Ax * By) - (Ay * Bx)'
             */

            return Vector128.Create(
                Y(left) * Z(right) - Z(left) * Y(right),
                Z(left) * X(right) - X(left) * Z(right),
                X(left) * Y(right) - Y(left) * X(right),
                0
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F CrossProduct4D_Software(Vector4FParam1_3 one, Vector4FParam1_3 two, Vector4FParam1_3 three)
        {
            return Vector128.Create(
                (two.GetElement(2) * three.GetElement(3) - two.GetElement(3) * three.GetElement(2)) *
                one.GetElement(1) -
                (two.GetElement(1) * three.GetElement(3) - two.GetElement(3) * three.GetElement(1)) *
                one.GetElement(2) +
                (two.GetElement(1) * three.GetElement(2) - two.GetElement(2) * three.GetElement(1)) *
                one.GetElement(3),

                (two.GetElement(3) * three.GetElement(2) - two.GetElement(2) * three.GetElement(3)) *
                one.GetElement(0) -
                (two.GetElement(3) * three.GetElement(0) - two.GetElement(0) * three.GetElement(3)) *
                one.GetElement(2) +
                (two.GetElement(2) * three.GetElement(0) - two.GetElement(0) * three.GetElement(2)) *
                one.GetElement(3),

                (two.GetElement(1) * three.GetElement(3) - two.GetElement(3) * three.GetElement(1)) *
                one.GetElement(0) -
                (two.GetElement(0) * three.GetElement(3) - two.GetElement(3) * three.GetElement(0)) *
                one.GetElement(1) +
                (two.GetElement(0) * three.GetElement(1) - two.GetElement(1) * three.GetElement(0)) *
                one.GetElement(3),

                (two.GetElement(2) * three.GetElement(1) - two.GetElement(1) * three.GetElement(2)) *
                one.GetElement(0) -
                (two.GetElement(2) * three.GetElement(0) - two.GetElement(0) * three.GetElement(2)) *
                one.GetElement(1) +
                (two.GetElement(1) * three.GetElement(0) - two.GetElement(0) * three.GetElement(1)) *
                one.GetElement(2)
            );
        }

        #endregion

        #region Distance

        [MethodImpl(MaxOpt)]
        public static Vector4F Distance2D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            Vector4FParam1_3 diff = Subtract_Software(left, right);

            return Length2D_Software(diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Distance3D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            Vector4FParam1_3 diff = Subtract_Software(left, right);

            return Length3D_Software(diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Distance4D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            Vector4FParam1_3 diff = Subtract_Software(left, right);

            return Length4D_Software(diff);
        }

        #endregion

        #region DistanceSquared

        [MethodImpl(MaxOpt)]
        public static Vector4F DistanceSquared2D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            Vector4FParam1_3 diff = Subtract_Software(left, right);

            return DotProduct2D_Software(diff, diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F DistanceSquared3D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            Vector4FParam1_3 diff = Subtract_Software(left, right);

            return DotProduct3D_Software(diff, diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F DistanceSquared4D_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            Vector4FParam1_3 diff = Subtract_Software(left, right);

            return DotProduct4D_Software(diff, diff);
        }

        #endregion

        #region Lerp

        private static float LerpHelper(float left, float right, float weight)
        {
            return left + (right - left) * weight;
        }

        public static Vector4F Lerp_Software(Vector4F from, Vector4F to, float weight)
        {
            return Vector128.Create(
                LerpHelper(X(from), X(to), weight),
                LerpHelper(Y(from), Y(to), weight),
                LerpHelper(Z(from), Z(to), weight),
                LerpHelper(W(from), W(to), weight)
            );
        }

        #endregion

        #region Reflect

        public static Vector4F Reflect2D_Software(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4F tmp = DotProduct2D_Software(incident, normal);
            tmp = Add_Software(tmp, tmp);
            tmp = Multiply_Software(tmp, normal);
            return Subtract_Software(incident, tmp);
        }

        public static Vector4F Reflect3D_Software(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4F tmp = DotProduct3D_Software(incident, normal);
            tmp = Add_Software(tmp, tmp);
            tmp = Multiply_Software(tmp, normal);
            return Subtract_Software(incident, tmp);
        }

        public static Vector4F Reflect4D_Software(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4F tmp = DotProduct4D_Software(incident, normal);
            tmp = Add_Software(tmp, tmp);
            tmp = Multiply_Software(tmp, normal);
            return Subtract_Software(incident, tmp);
        }

        #endregion

        #endregion
    }
}
