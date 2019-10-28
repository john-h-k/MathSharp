using System.ComponentModel.DataAnnotations;
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod

namespace MathSharp
{
    public static unsafe partial class Matrix
    {
        public static MatrixSingle FromMatrix4x4(float* p)
        {
            var v0 = Vector.FromVector4D(&p[0]);
            var v1 = Vector.FromVector4D(&p[4]);
            var v2 = Vector.FromVector4D(&p[8]);
            var v3 = Vector.FromVector4D(&p[12]);

            return new MatrixSingle(v0, v1, v2, v3);
        }

        public static void ToMatrix4x4(MatrixSingle matrix, Matrix4x4* destination)
        {
            float* p = (float*)destination;

            Vector.ToVector4D(matrix._v0, &p[0]);
            Vector.ToVector4D(matrix._v1, &p[4]);
            Vector.ToVector4D(matrix._v2, &p[8]);
            Vector.ToVector4D(matrix._v3, &p[12]);
        }

        public static void ToMatrix4x4(MatrixSingle matrix, out Matrix4x4 destination)
        {
            fixed (Matrix4x4* p = &destination)
            {
                ToMatrix4x4(matrix, p);
            }
        }

    }
}
