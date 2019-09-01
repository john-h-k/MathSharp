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
            float x = (Z(two) * W(three) - W(two) * Z(three)) *
                    Y(one) -
                    (Y(two) * W(three) - W(two) * Y(three)) *
                    Z(one) +
                    (Y(two) * Z(three) - Z(two) * Y(three)) *
                    W(one);

            float y = (W(two) * Z(three) - Z(two) * W(three)) *
                    X(one) -
                    (W(two) * X(three) - X(two) * W(three)) *
                    Z(one) +
                    (Z(two) * X(three) - X(two) * Z(three)) *
                    W(one);

            float z = (Y(two) * W(three) - W(two) * Y(three)) *
                    X(one) -
                    (X(two) * W(three) - W(two) * X(three)) *
                    Y(one) +
                    (X(two) * Y(three) - Y(two) * X(three)) *
                    W(one);

            float w = (Z(two) * Y(three) - Y(two) * Z(three)) *
                    X(one) -
                    (Z(two) * X(three) - X(two) * Z(three)) *
                    Y(one) +
                    (Y(two) * X(three) - X(two) * Y(three)) *
                    Z(one);

            return Vector128.Create(x, y, z, w);
        }

        #endregion

        #endregion
    }
}
