using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{ 
    public static partial class Matrix
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static MatrixSingle Add(in MatrixSingle left, in MatrixSingle right)
        {
            if (Sse.IsSupported)
            {
                MatrixSingle result;
                
                result._v1 = Sse.Add(left._v1, right._v1);
                result._v2 = Sse.Add(left._v2, right._v2);
                result._v3 = Sse.Add(left._v3, right._v3);
                result._v4 = Sse.Add(left._v4, right._v4);

                return result;
            }

            return Add_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply(in MatrixSingle left, Vector128<float> vectorOfScalar)
        {
            if (Sse.IsSupported)
            {
                MatrixSingle result;

                result._v1 = Sse.Multiply(left._v1, vectorOfScalar);
                result._v2 = Sse.Multiply(left._v2, vectorOfScalar);
                result._v3 = Sse.Multiply(left._v3, vectorOfScalar);
                result._v4 = Sse.Multiply(left._v4, vectorOfScalar);

                return result;
            }

            return ScalarMultiply_Software(left, vectorOfScalar);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static MatrixSingle ScalarMultiply(in MatrixSingle left, float scalar)
        {
            if (Sse.IsSupported)
            {
                MatrixSingle result;

                Vector128<float> vector = Vector128.Create(scalar);
                result._v1 = Sse.Multiply(left._v1, vector);
                result._v2 = Sse.Multiply(left._v2, vector);
                result._v3 = Sse.Multiply(left._v3, vector);
                result._v4 = Sse.Multiply(left._v4, vector);

                return result;
            }

            return ScalarMultiply_Software(left, scalar);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static MatrixSingle Transpose(in MatrixSingle matrix)
        {
            if (Sse.IsSupported)
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
                Vector128<float> xAndY1 = Sse.Shuffle(matrix._v1, matrix._v2, ShuffleValues._1_0_1_0);

                // z1, w1, z2, w2
                Vector128<float> zAndW1 = Sse.Shuffle(matrix._v1, matrix._v2, ShuffleValues._3_2_3_2);

                // x3, y3, x4, y4
                Vector128<float> xAndY2 = Sse.Shuffle(matrix._v3, matrix._v4, ShuffleValues._1_0_1_0);

                // z3, w3, z4, w4
                Vector128<float> zAndW2 = Sse.Shuffle(matrix._v3, matrix._v4, ShuffleValues._3_2_3_2);

                MatrixSingle result;

                // x1, x2, x3, x4
                result._v1 = Sse.Shuffle(xAndY1, xAndY2, ShuffleValues._2_0_2_0);
                
                // y1, y2, y3, y4
                result._v2 = Sse.Shuffle(xAndY1, xAndY2, ShuffleValues._3_1_3_1);

                // z1, z2, z3, z4
                result._v3 = Sse.Shuffle(zAndW1, zAndW2, ShuffleValues._2_0_2_0);

                // w1, w2, w3, w4
                result._v4 = Sse.Shuffle(zAndW1, zAndW2, ShuffleValues._3_1_3_1);
                
                return result;
            }

            return Transpose_Software(matrix);
        }
    }
}
