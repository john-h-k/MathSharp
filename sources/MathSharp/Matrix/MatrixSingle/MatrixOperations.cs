using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Matrix
    {
        public static MatrixSingle Identity { get; } = new MatrixSingle(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        private static readonly Vector128<float> IdentityRow0 = Vector128.Create(1f, 0f, 0f, 0f);
        private static readonly Vector128<float> IdentityRow1 = Vector128.Create(0f, 1f, 0f, 0f);
        private static readonly Vector128<float> IdentityRow2 = Vector128.Create(0f, 0f, 1f, 0f);
        private static readonly Vector128<float> IdentityRow3 = Vector128.Create(0f, 0f, 0f, 1f);

        public static bool IsIdentity(MatrixSingle matrix)
        {
            var row0 = Vector.Equality(matrix._v0, IdentityRow0);
            var row1 = Vector.Equality(matrix._v1, IdentityRow1);
            var row2 = Vector.Equality(matrix._v2, IdentityRow2);
            var row3 = Vector.Equality(matrix._v3, IdentityRow3);

            row0 = Vector.And(row0, row1);
            row2 = Vector.And(row2, row3);
            row0 = Vector.And(row0, row2);

            return Vector.ExtractMask(row0) == 0b_0000_1111;
        }

        public static Vector128<float> GetTranslation(MatrixSingle matrix)
        {
            Vector128<float> vec = matrix._v3;
            return Vector.And(vec, Vector.MaskW);
        }

        public static MatrixSingle SetTranslation(MatrixSingle matrix, Vector128<float> translation)
        {
            // (X, Y, Z, W) - we must keep W
            Vector128<float> old = matrix._v3;

            // Make W of translation zero
            translation = Vector.And(translation, Vector.MaskW);
            // Mask out everything but W
            old = Vector.And(old, Vector.MaskXYZ);

            // Or them together to get X Y Z from translation and W from old
            translation = Vector.Or(translation, old);

            matrix._v3 = translation;

            return matrix;
        }  
    }
}
