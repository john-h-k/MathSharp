using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp.SoftwareFallbacks
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    internal static partial class SoftwareFallbacksVector4D
    {
        #region Vector Maths

        #region Normalize

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize2D_Software(Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector4D magnitude = Length2D_Software(vector);
            return Divide_Software(vector, magnitude);
        }



        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize3D_Software(Vector4DParam1_3 vector)
        {
            Vector4D magnitude = Length3D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize4D_Software(Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector4D magnitude = Length4D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        #endregion

        #region Length

        [MethodImpl(MaxOpt)]
        public static Vector4D Length2D_Software(Vector4DParam1_3 vector)
        {
            return Sqrt_Software(DotProduct2D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Length3D_Software(Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct3D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Length4D_Software(Vector4DParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct4D_Software(vector, vector));
        }

        #endregion

        #region LengthSquared

        #endregion

        #region DotProduct

        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct2D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.X(left) * Helpers.X(right) +
                +Helpers.Y(left) * Helpers.Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct3D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.X(left) * Helpers.X(right)
                + Helpers.Y(left) * Helpers.Y(right)
                + Helpers.Z(left) * Helpers.Z(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct4D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.X(left) * Helpers.X(right)
                + Helpers.Y(left) * Helpers.Y(right)
                + Helpers.Z(left) * Helpers.Z(right)
                + Helpers.W(left) * Helpers.W(right)
            );
        }

        #endregion

        #region CrossProduct

        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct2D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {

            return Vector256.Create((Helpers.X(left) * Helpers.Y(right) - Helpers.Y(left) * Helpers.X(right)));
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct3D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            /* Cross product of A(x, y, z, _) and B(x, y, z, _) is
             *
             * '(X = (Ay * Bz) - (Az * By), Y = (Az * Bx) - (Ax * Bz), Z = (Ax * By) - (Ay * Bx)'
             */

            return Vector256.Create(
                Helpers.Y(left) * Helpers.Z(right) - Helpers.Z(left) * Helpers.Y(right),
                Helpers.Z(left) * Helpers.X(right) - Helpers.X(left) * Helpers.Z(right),
                Helpers.X(left) * Helpers.Y(right) - Helpers.Y(left) * Helpers.X(right),
                0
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct4D_Software(Vector4DParam1_3 one, Vector4DParam1_3 two, Vector4DParam1_3 three)
        {
            return Vector256.Create(
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
        public static Vector4D Distance2D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            Vector4DParam1_3 diff = Subtract_Software(left, right);

            return Length2D_Software(diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Distance3D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            Vector4DParam1_3 diff = Subtract_Software(left, right);

            return Length3D_Software(diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Distance4D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            Vector4DParam1_3 diff = Subtract_Software(left, right);

            return Length4D_Software(diff);
        }

        #endregion

        #region DistanceSquared

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared2D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            Vector4DParam1_3 diff = Subtract_Software(left, right);

            return DotProduct2D_Software(diff, diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared3D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            Vector4DParam1_3 diff = Subtract_Software(left, right);

            return DotProduct3D_Software(diff, diff);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared4D_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            Vector4DParam1_3 diff = Subtract_Software(left, right);

            return DotProduct4D_Software(diff, diff);
        }

        #endregion

        #endregion
    }
}
