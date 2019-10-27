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
             *
             */


            // x1, y1, x2, y2
            Vector128<float> xAndY1 = Vector.Shuffle(matrix._v0, matrix._v1, ShuffleValues.XYXY);

            // z1, w1, z2, w2
            Vector128<float> zAndW1 = Vector.Shuffle(matrix._v0, matrix._v1, ShuffleValues.ZWZW);

            // x3, y3, x4, y4
            Vector128<float> xAndY2 = Vector.Shuffle(matrix._v2, matrix._v3, ShuffleValues.XYXY);

            // z3, w3, z4, w4
            Vector128<float> zAndW2 = Vector.Shuffle(matrix._v2, matrix._v3, ShuffleValues.ZWZW);

            return new MatrixSingle(
                // x1, x2, x3, x4
                Vector.Shuffle(xAndY1, xAndY2, ShuffleValues.XZXZ),
                // y1, y2, y3, y4
                Vector.Shuffle(xAndY1, xAndY2, ShuffleValues.YWYW),
                // z1, z2, z3, z4
                Vector.Shuffle(zAndW1, zAndW2, ShuffleValues.XZXZ),
                // w1, w2, w3, w4
                Vector.Shuffle(zAndW1, zAndW2, ShuffleValues.YWYW)
            );
        }
    }
}
