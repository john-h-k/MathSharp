// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod

namespace MathSharp
{
    public static unsafe partial class Matrix
    {
        public static MatrixSingle FromMatrix4x4(float* p)
        {
            var v0 = Vector.Load4(&p[0]);
            var v1 = Vector.Load4(&p[4]);
            var v2 = Vector.Load4(&p[8]);
            var v3 = Vector.Load4(&p[12]);

            return new MatrixSingle(v0, v1, v2, v3);
        }

        public static void ToMatrix4x4(MatrixSingle matrix, float* destination)
        {
            Vector.Store4(matrix._v0, &destination[0]);
            Vector.Store4(matrix._v1, &destination[4]);
            Vector.Store4(matrix._v2, &destination[8]);
            Vector.Store4(matrix._v3, &destination[12]);
        }
    }
}
