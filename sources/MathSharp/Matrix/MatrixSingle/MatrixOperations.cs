using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
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

            return Vector.MoveMask(row0) == 0b_0000_1111;
        }

        public static bool IsIdentity_CreateLocal_2(MatrixSingle matrix)
        {
            var identityRow0 = Vector128.CreateScalar(1f);
            var identityRow1 = Vector4F.Zero;
            var identityRow2 = Vector4F.Zero;
            var identityRow3 = Vector4F.Zero;

            identityRow1 = Sse41.Insert(identityRow1, identityRow0, 0x10);
            identityRow2 = Sse41.Insert(identityRow2, identityRow0, 0x20);
            identityRow3 = Sse41.Insert(identityRow3, identityRow0, 0x30);

            var row0 = Vector.Equality(matrix._v0, identityRow0);
            var row1 = Vector.Equality(matrix._v1, identityRow1);
            var row2 = Vector.Equality(matrix._v2, identityRow2);
            var row3 = Vector.Equality(matrix._v3, identityRow3);

            row0 = Vector.And(row0, row1);
            row2 = Vector.And(row2, row3);
            row0 = Vector.And(row0, row2);

            return Vector.MoveMask(row0) == 0b_0000_1111;
        }

        public static HwVectorAnyS GetTranslation(MatrixSingle matrix)
        {
            Vector128<float> vec = matrix._v3;
            return Vector.And(vec, Vector.SingleConstants.MaskW);
        }

        public static MatrixSingle SetTranslation(MatrixSingle matrix, Vector4FParam1_3 translation)
        {
            // (X, Y, Z, W) - we must keep W
            Vector4F old = matrix._v3;

            // Make W of translation zero
            
            Vector4F newTranslation = Vector.And(translation, Vector.SingleConstants.MaskW);
            // Mask out everything but W
            old = Vector.And(old, Vector.SingleConstants.MaskXYZ);

            // Or them together to get X Y Z from translation and W from old
            newTranslation = Vector.Or(newTranslation, old);

            matrix._v3 = newTranslation;

            return matrix;
        }

        private static readonly Vector4F BillboardEpsilon = Vector128.Create(1e-4f);

        public static MatrixSingle CreateBillboard(Vector4FParam1_3 objectPosition, Vector4FParam1_3 cameraPosition, Vector4FParam1_3 cameraUpVector, Vector4FParam1_3 cameraForwardVector)
        {
            Vector4F z = Vector.Subtract(objectPosition, cameraPosition);

            Vector4F norm = Vector.LengthSquared3D(z);

            z = Vector.MoveMask(Vector.LessThan(norm, BillboardEpsilon)) != 0 ? 
                Vector.Negate3D(cameraForwardVector) 
                : Vector.Multiply(z, Vector.Divide(Vector.SingleConstants.AllBitsSet, Vector.Sqrt(norm)));

            Vector4F x = Vector.Normalize3D(Vector.CrossProduct3D(cameraUpVector, z));

            Vector4F y = Vector.CrossProduct3D(z, x);

            // We need W to be zero for x, y, and z, and 1.0f for objectPosition. They are currently undefined
            x = Vector.ZeroW(x);
            y = Vector.ZeroW(y);
            z = Vector.ZeroW(z);

            // Get objectPosition to be (X, Y, Z, 0) and the mask to be (0, 0, 0, 1.0f) and OR them
            Vector4F newObjectPosition = Vector.ZeroW(objectPosition);
            newObjectPosition = Vector.Or(newObjectPosition, Vector.And(Vector.SingleConstants.MaskXYZ, Vector.SingleConstants.AllBitsSet));

            return new MatrixSingle(x, y, z, newObjectPosition);
        }
    }
}
