using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    
    

    internal static partial class SoftwareFallbacks
    {
        #region Vector Maths

        #region DotProduct

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DotProduct2D_Software(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                X(left) * X(right) +
                +Y(left) * Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DotProduct3D_Software(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                X(left) * X(right)
                + Y(left) * Y(right)
                + Z(left) * Z(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DotProduct4D_Software(Vector128<float> left, Vector128<float> right)
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
        public static Vector128<float> CrossProduct2D_Software(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create((X(left) * Y(right) - Y(left) * X(right)));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CrossProduct3D_Software(Vector128<float> left, Vector128<float> right)
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
        public static Vector128<float> CrossProduct4D_Software(Vector128<float> one, Vector128<float> two, Vector128<float> three)
        {
            var x1 = X(one);
            var x2 = X(two);
            var x3 = X(three);
            var y1 = Y(one);
            var y2 = Y(two);
            var y3 = Y(three);
            var z1 = Z(one);
            var z2 = Z(two);
            var z3 = Z(three);
            var w1 = W(one);
            var w2 = W(two);
            var w3 = W(three);

            float xTmp1 = z2 * w3 - w2 * z3;
            float yTmp1 = w2 * z3 - z2 * w3;
            float zTmp1 = y2 * w3 - w2 * y3;
            float wTmp1 = z2 * y3 - y2 * z3;


            float xTmp2 = y1 - (y2 * w3 - w2 * y3);
            float yTmp2 = x1 - (w2 * x3 - x2 * w3);
            float zTmp2 = x1 - (x2 * w3 - w2 * x3);
            float wTmp2 = x1 - (z2 * x3 - x2 * z3);


            float xTmp3 = z1 + (y2 * z3 - z2 * y3);
            float yTmp3 = z1 + (z2 * x3 - x2 * z3);
            float zTmp3 = y1 + (x2 * y3 - y2 * x3);
            float wTmp3 = y1 + (y2 * x3 - x2 * y3);

            
            float x = xTmp1 * xTmp2 * xTmp3 * w1;
            float y = yTmp1 * yTmp2 * yTmp3 * w1;
            float z = zTmp1 * zTmp2 * zTmp3 * w1;
            float w = wTmp1 * wTmp2 * wTmp3 * z1;


            return Vector128.Create(x, y, z, w);
        }

        #endregion

        #endregion
    }
}
