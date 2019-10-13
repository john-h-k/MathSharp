using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using Microsoft.VisualBasic.CompilerServices;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Matrix
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        [MethodImpl(MaxOpt)]
        public static MatrixSingle Add(in MatrixSingle left, in MatrixSingle right) =>
            new MatrixSingle(
                Vector.Add(left._v0, right._v0),
                Vector.Add(left._v1, right._v1),
                Vector.Add(left._v2, right._v2),
                Vector.Add(left._v3, right._v3)
            );


        [MethodImpl(MaxOpt)]
        public static MatrixSingle Subtract(in MatrixSingle left, in MatrixSingle right) =>
            new MatrixSingle(
                Vector.Subtract(left._v0, right._v0),
                Vector.Subtract(left._v1, right._v1),
                Vector.Subtract(left._v2, right._v2),
                Vector.Subtract(left._v3, right._v3)
            );


        [MethodImpl(MaxOpt)]
        public static MatrixSingle Negate(in MatrixSingle matrix) =>
            new MatrixSingle(
                Vector.Negate(matrix._v0),
                Vector.Negate(matrix._v1),
                Vector.Negate(matrix._v2),
                Vector.Negate(matrix._v3)
            );

        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply(in MatrixSingle left, Vector128<float> vectorOfScalar) =>
            new MatrixSingle(
                Vector.Multiply(left._v0, vectorOfScalar),
                Vector.Multiply(left._v1, vectorOfScalar),
                Vector.Multiply(left._v2, vectorOfScalar),
                Vector.Multiply(left._v3, vectorOfScalar)
            );


        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply(in MatrixSingle left, float scalar)
            => ScalarMultiply(left, Vector128.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static MatrixSingle Transpose(in MatrixSingle matrix)
        {
            /*
             * ---------------
             * | x1 y1 z1 w1 |
             * | x2 y2 z2 w2 |
             * | x3 y3 z3 w3 |
             * | x4 y4 z4 w4 |
             * ---------------
             *
             * become
             *
             * ---------------
             * | x1 x2 x3 x4 |
             * | y1 y2 y3 y4 |
             * | z1 z2 z3 z4 |
             * | w1 w2 w3 w4 |
             * ---------------
             *
             * We achieve this with an intermediate step of
             *
             * ---------------
             * | x1 y1 x2 y2 |
             * | z1 w1 z2 w2 |
             * | x3 y3 x4 y4 |
             * | z3 w3 z4 w4 |
             * ---------------
             *
             * We use Shuffle(Vector FirstVec, Vector SecondVec, byte control)
             * Shuffle takes a byte control which consists of 4 numbers (here part of the field). They work back to front like this
             * _a_b_c_d
             * d - the element from FirstVec, which should be in the first element of the returned vector
             * c - the element from FirstVec, which should be in the second element of the returned vector
             * b - the element from SecondVec, which should be in the third element of the returned vector
             * a - the element from SecondVec, which should be in the fourth element of the returned vector
             *
             */


            // x1, y1, x2, y2
            Vector128<float> xAndY1 = Vector.Shuffle(matrix._v0, matrix._v1, ShuffleValues._0_1_0_1);

            // z1, w1, z2, w2
            Vector128<float> zAndW1 = Vector.Shuffle(matrix._v0, matrix._v1, ShuffleValues._2_3_2_3);

            // x3, y3, x4, y4
            Vector128<float> xAndY2 = Vector.Shuffle(matrix._v2, matrix._v3, ShuffleValues._0_1_0_1);

            // z3, w3, z4, w4
            Vector128<float> zAndW2 = Vector.Shuffle(matrix._v2, matrix._v3, ShuffleValues._2_3_2_3);

            return new MatrixSingle(
                // x1, x2, x3, x4
                Vector.Shuffle(xAndY1, xAndY2, ShuffleValues._0_2_0_2),
                // y1, y2, y3, y4
                Vector.Shuffle(xAndY1, xAndY2, ShuffleValues._1_3_1_3),
                // z1, z2, z3, z4
                Vector.Shuffle(zAndW1, zAndW2, ShuffleValues._0_2_0_2),
                // w1, w2, w3, w4
                Vector.Shuffle(zAndW1, zAndW2, ShuffleValues._1_3_1_3)
            );
        }
    }
}
