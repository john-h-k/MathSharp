using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;
using static MathSharp.Vector;

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

        private static readonly HwVector4S IdentityRow0 = Vector128.Create(1f, 0f, 0f, 0f);
        private static readonly HwVector4S IdentityRow1 = Vector128.Create(0f, 1f, 0f, 0f);
        private static readonly HwVector4S IdentityRow2 = Vector128.Create(0f, 0f, 1f, 0f);
        private static readonly HwVector4S IdentityRow3 = Vector128.Create(0f, 0f, 0f, 1f);

        public static bool IsIdentity(MatrixSingle matrix)
        {
            Vector128<float> row0 = CompareEqual(matrix._v0, IdentityRow0);
            Vector128<float> row1 = CompareEqual(matrix._v1, IdentityRow1);
            Vector128<float> row2 = CompareEqual(matrix._v2, IdentityRow2);
            Vector128<float> row3 = CompareEqual(matrix._v3, IdentityRow3);

            row0 = And(row0, row1);
            row2 = And(row2, row3);
            row0 = And(row0, row2);

            return row0.AllTrue();
        }

        public static HwVectorAnyS GetTranslation(MatrixSingle matrix)
        {
            Vector128<float> vec = matrix._v3;
            return And(vec, SingleConstants.MaskW);
        }

        public static MatrixSingle SetTranslation(MatrixSingle matrix, Vector4FParam1_3 translation)
        {
            // (X, Y, Z, W) - we must keep W
            Vector4F old = matrix._v3;

            // Make W of translation zero

            Vector4F newTranslation = And(translation, SingleConstants.MaskW);
            // Mask out everything but W
            old = And(old, SingleConstants.MaskXYZ);

            // Or them together to get X Y Z from translation and W from old
            newTranslation = Or(newTranslation, old);

            matrix._v3 = newTranslation;

            return matrix;
        }

        private static readonly Vector4F BillboardEpsilon = Vector128.Create(1e-4f);

        //public static MatrixSingle CreateBillboard(Vector4FParam1_3 objectPosition, Vector4FParam1_3 cameraPosition, Vector4FParam1_3 cameraUpVector, Vector4FParam1_3 cameraForwardVector)
        //{
        //    Vector4F z = Vector.Subtract(objectPosition, cameraPosition);

        //    Vector4F norm = LengthSquared3D(z);

        //    z = MoveMask(CompareLessThan(norm, BillboardEpsilon)) != 0 ?
        //        Negate3D(cameraForwardVector)
        //        : Multiply(z, Divide(SingleConstants.AllBitsSet, Sqrt(norm)));

        //    Vector4F x = Normalize3D(CrossProduct3D(cameraUpVector, z));

        //    Vector4F y = CrossProduct3D(z, x);

        //    // We need W to be zero for x, y, and z, and 1.0f for objectPosition. They are currently undefined
        //    x = Vector.ZeroW(x);
        //    y = Vector.ZeroW(y);
        //    z = Vector.ZeroW(z);

        //    // Get objectPosition to be (X, Y, Z, 0) and the mask to be (0, 0, 0, 1.0f) and OR them
        //    Vector4F newObjectPosition = Vector.ZeroW(objectPosition);
        //    newObjectPosition = Or(newObjectPosition, And(SingleConstants.MaskXYZ, SingleConstants.AllBitsSet));

        //    return new MatrixSingle(x, y, z, newObjectPosition);
        //}
    }
}
