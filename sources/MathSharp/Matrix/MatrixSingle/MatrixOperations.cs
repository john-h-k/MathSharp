using System;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Matrix
    {
        public static MatrixSingle Identity { get; } = new MatrixSingle(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        private static readonly Vector4F IdentityRow0 = Vector128.Create(1f, 0f, 0f, 0f);
        private static readonly Vector4F IdentityRow1 = Vector128.Create(0f, 1f, 0f, 0f);
        private static readonly Vector4F IdentityRow2 = Vector128.Create(0f, 0f, 1f, 0f);
        private static readonly Vector4F IdentityRow3 = Vector128.Create(0f, 0f, 0f, 1f);

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

        public static MatrixSingle SetTranslation(MatrixSingle matrix, in Vector4FParam1_3 translation)
        {
            // (X, Y, Z, W) - we must keep W
            Vector4F old = matrix._v3;

            // Make W of translation zero
            
            Vector4F newTranslation = Vector.And(translation, Vector.MaskW);
            // Mask out everything but W
            old = Vector.And(old, Vector.MaskXYZ);

            // Or them together to get X Y Z from translation and W from old
            newTranslation = Vector.Or(newTranslation, old);

            matrix._v3 = newTranslation;

            return matrix;
        }

        private static readonly Vector4F BillboardEpsilon = Vector128.Create(1e-4f);

        public static MatrixSingle CreateBillboard(in Vector4FParam1_3 objectPosition, in Vector4FParam1_3 cameraPosition, in Vector4FParam1_3 cameraUpVector, in Vector4FParam1_3 cameraForwardVector)
        {
            Vector4F z = Vector.Subtract(objectPosition, cameraPosition);

            Vector4F norm = Vector.LengthSquared3D(z);

            z = Vector.ExtractMask(Vector.LessThan(norm, BillboardEpsilon)) != 0 ? 
                Vector.Negate3D(cameraForwardVector) 
                : Vector.Multiply(z, Vector.Divide(Vector.One, Vector.Sqrt(norm)));

            Vector4F x = Vector.Normalize3D(Vector.CrossProduct3D(cameraUpVector, z));

            Vector4F y = Vector.CrossProduct3D(z, x);

            // We need W to be zero for x, y, and z, and 1.0f for objectPosition. They are currently undefined
            x = Vector.ZeroW(x);
            y = Vector.ZeroW(y);
            z = Vector.ZeroW(z);

            // Get objectPosition to be (X, Y, Z, 0) and the mask to be (0, 0, 0, 1.0f) and OR them
            Vector4F newObjectPosition = Vector.ZeroW(objectPosition);
            newObjectPosition = Vector.Or(newObjectPosition, Vector.And(Vector.MaskXYZ, Vector.One));

            return new MatrixSingle(x, y, z, newObjectPosition);
        }
    }
}
