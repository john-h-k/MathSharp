using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    public static partial class Matrix
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        /// <summary>
        /// Performs an element-wise matrix addition on the 2 <see cref="MatrixSingle"/>s
        /// </summary>
        /// <param name="left">The left matrix to add</param>
        /// <param name="right">The right matrix to add</param>
        /// <returns>A <see cref="MatrixSingle"/> with its components added</returns>
        [MethodImpl(MaxOpt)]
        public static MatrixSingle Add(in MatrixSingle left, in MatrixSingle right) =>
            new MatrixSingle(
                Vector.Add(left._v0, right._v0),
                Vector.Add(left._v1, right._v1),
                Vector.Add(left._v2, right._v2),
                Vector.Add(left._v3, right._v3)
            );

        /// <summary>
        /// Performs an element-wise matrix addition on the 2 <see cref="MatrixSingle"/>s
        /// </summary>
        /// <param name="left">The left matrix to add</param>
        /// <param name="right">The right matrix to add</param>
        /// <returns>A <see cref="MatrixSingle"/> with its components added</returns>
        [MethodImpl(MaxOpt)]
        public static unsafe MatrixSingle Add(MatrixSingle* left, MatrixSingle* right) =>
            new MatrixSingle(
                Vector.Add(left->_v0, right->_v0),
                Vector.Add(left->_v1, right->_v1),
                Vector.Add(left->_v2, right->_v2),
                Vector.Add(left->_v3, right->_v3)
            );

        /// <summary>
        /// Performs an element-wise matrix subtraction on the 2 <see cref="MatrixSingle"/>s
        /// </summary>
        /// <param name="left">The left matrix to subtract</param>
        /// <param name="right">The right matrix to subtract</param>
        /// <returns>A <see cref="MatrixSingle"/> with its components subtracted</returns>
        [MethodImpl(MaxOpt)]
        public static MatrixSingle Subtract(in MatrixSingle left, in MatrixSingle right) =>
            new MatrixSingle(
                Vector.Subtract(left._v0, right._v0),
                Vector.Subtract(left._v1, right._v1),
                Vector.Subtract(left._v2, right._v2),
                Vector.Subtract(left._v3, right._v3)
            );


        /// <summary>
        /// Performs an element-wise negation on a <see cref="MatrixSingle"/>s
        /// </summary>
        /// <param name="matrix">The matrix to negate</param>
        /// <returns>A <see cref="MatrixSingle"/> with its components negated</returns>
        [MethodImpl(MaxOpt)]
        public static MatrixSingle Negate(in MatrixSingle matrix) =>
            new MatrixSingle(
                Vector.Negate(matrix._v0),
                Vector.Negate(matrix._v1),
                Vector.Negate(matrix._v2),
                Vector.Negate(matrix._v3)
            );

        /// <summary>
        /// Performs an element-wise matrix multiplication on the 2 <see cref="MatrixSingle"/>s
        /// </summary>
        /// <param name="left">The left matrix to multiply</param>
        /// <param name="right">The right matrix to multiply</param>
        /// <returns>A <see cref="MatrixSingle"/> with its components multiplied</returns>
        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply(in MatrixSingle left, Vector128<float> vectorOfScalar) =>
            new MatrixSingle(
                Vector.Multiply(left._v0, vectorOfScalar),
                Vector.Multiply(left._v1, vectorOfScalar),
                Vector.Multiply(left._v2, vectorOfScalar),
                Vector.Multiply(left._v3, vectorOfScalar)
            );


        /// <summary>
        /// Performs an element-wise matrix multiplication on a <see cref="MatrixSingle"/> and a <see cref="float"/>
        /// </summary>
        /// <param name="left">The left matrix to multiply</param>
        /// <param name="scalar">The scalar to multiply it by</param>
        /// <returns>A new <see cref="MatrixSingle"/> with its components added</returns>
        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply(in MatrixSingle left, float scalar)
            => ScalarMultiply(left, Vector128.Create(scalar));

        /// <summary>
        /// Transposes a <see cref="MatrixSingle"/>
        /// </summary>
        /// <param name="matrix">The matrix to transpose</param>
        /// <returns>A new <see cref="MatrixSingle"/>, transposed</returns>
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
