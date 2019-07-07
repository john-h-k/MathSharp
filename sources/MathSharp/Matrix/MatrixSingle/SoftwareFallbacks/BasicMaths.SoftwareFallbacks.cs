using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class SoftwareFallbacks
    {

        [MethodImpl(MaxOpt)]
        public static MatrixSingle Add_Software(in MatrixSingle left, in MatrixSingle right)
        {
            return MatrixSingle.Create(
                Vector.Add(left._v1, right._v1),
                Vector.Add(left._v2, right._v2),
                Vector.Add(left._v3, right._v3),
                Vector.Add(left._v4, right._v4)
            );
        }

        [MethodImpl(MaxOpt)]
        public static MatrixSingle Subtract_Software(in MatrixSingle left, in MatrixSingle right)
        {
            return MatrixSingle.Create(
                Vector.Subtract(left._v1, right._v1),
                Vector.Subtract(left._v2, right._v2),
                Vector.Subtract(left._v3, right._v3),
                Vector.Subtract(left._v4, right._v4)
            );
        }

        [MethodImpl(MaxOpt)]
        public static MatrixSingle Negate_Software(in MatrixSingle matrix)
        {
            return MatrixSingle.Create(
                Vector.Negate4D(matrix._v1),
                Vector.Negate4D(matrix._v2),
                Vector.Negate4D(matrix._v3),
                Vector.Negate4D(matrix._v4)
            );
        }

        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply_Software(in MatrixSingle left, Vector128<float> vectorOfScalar)
        {
            return MatrixSingle.Create(
                Vector.Multiply(left._v1, vectorOfScalar),
                Vector.Multiply(left._v2, vectorOfScalar),
                Vector.Multiply(left._v3, vectorOfScalar),
                Vector.Multiply(left._v4, vectorOfScalar)
            );
        }

        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply_Software(in MatrixSingle left, float scalar)
        {
            return MatrixSingle.Create(
                Vector.Multiply(left._v1, scalar),
                Vector.Multiply(left._v2, scalar),
                Vector.Multiply(left._v3, scalar),
                Vector.Multiply(left._v4, scalar)
            );
        }

        [MethodImpl(MaxOpt)]

        public static MatrixSingle Transpose_Software(in MatrixSingle matrix)
        {
            return MatrixSingle.Create(
                X(matrix._v1), X(matrix._v2), X(matrix._v3), X(matrix._v4),
                Y(matrix._v1), Y(matrix._v2), Y(matrix._v3), Y(matrix._v4),
                Z(matrix._v1), Z(matrix._v2), Z(matrix._v3), Z(matrix._v4),
                W(matrix._v1), W(matrix._v2), W(matrix._v3), W(matrix._v4)
            );
        }
    }
}
