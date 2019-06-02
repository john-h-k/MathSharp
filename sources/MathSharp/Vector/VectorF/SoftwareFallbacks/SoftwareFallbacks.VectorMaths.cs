using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp.SoftwareFallbacks
{
    using VectorFParam1_3 = Vector128<float>;

    internal static unsafe partial class SoftwareFallbacks
    {
        #region Vector Maths

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DotProduct2D_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) * Helpers.X(right) +
                +Helpers.Y(left) * Helpers.Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DotProduct3D_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) * Helpers.X(right)
                + Helpers.Y(left) * Helpers.Y(right)
                + Helpers.Z(left) * Helpers.Z(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DotProduct4D_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) * Helpers.X(right)
                + Helpers.Y(left) * Helpers.Y(right)
                + Helpers.Z(left) * Helpers.Z(right)
                + Helpers.W(left) * Helpers.W(right)
            );
        }

        private static readonly Vector128<float> MaskWToZero = Vector128.Create(-1, -1, -1, 0).AsSingle();
        private static readonly Vector128<float> MaskWAndZToZero = Vector128.Create(-1, -1, 0, 0).AsSingle();

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CrossProduct2D_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {

            return Vector128.Create((Helpers.X(left) * Helpers.Y(right) - Helpers.Y(left) * Helpers.X(right)));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CrossProduct3D_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            /* Cross product of A(x, y, z, _) and B(x, y, z, _) is
             *
             * '(X = (Ay * Bz) - (Az * By), Y = (Az * Bx) - (Ax * Bz), Z = (Ax * By) - (Ay * Bx)'
             */

            return Vector128.Create(
                Helpers.Y(left) * Helpers.Z(right) - Helpers.Z(left) * Helpers.Y(right),
                Helpers.Z(left) * Helpers.X(right) - Helpers.X(left) * Helpers.Z(right),
                Helpers.X(left) * Helpers.Y(right) - Helpers.Y(left) * Helpers.X(right),
                0
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CrossProduct4D_Software(VectorFParam1_3 one, VectorFParam1_3 two, VectorFParam1_3 three)
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

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Length2D_Software(VectorFParam1_3 vector)
        {
            return Sqrt_Software(DotProduct2D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Normalize2D_Software(VectorFParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector128<float> magnitude = Length2D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Length3D_Software(VectorFParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct3D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Normalize3D_Software(VectorFParam1_3 vector)
        {
            Vector128<float> magnitude = Length3D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Length4D_Software(VectorFParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt_Software(DotProduct4D_Software(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Normalize4D_Software(VectorFParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            Vector128<float> magnitude = Length4D_Software(vector);
            return Divide_Software(vector, magnitude);
        }

        #endregion
    }
}
