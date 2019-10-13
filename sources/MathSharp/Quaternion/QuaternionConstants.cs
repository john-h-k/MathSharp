using System.Runtime.Intrinsics;
// ReSharper disable InconsistentNaming

namespace MathSharp.Quaternion
{
    public static partial class Quaternion
    {
        public static class SingleConstants
        {
            public static readonly Vector128<float> Identity = Vector128.Create(0f, 0f, 0f, 1f);

            public static readonly Vector128<float> MatrixIdentityRow0 = Vector128.Create(1f, 0f, 0f, 0f);

            public static readonly Vector128<float> SlerpEpsilon = Vector128.Create(1e-6f);
            public static readonly Vector128<float> OneMinusSlerpEpsilon = Vector128.Create(1 - 1e-6f);

            public static readonly Vector128<float> SignMaskXYZ = Vector128.Create(int.MinValue, int.MinValue, int.MinValue, 0).AsSingle();
            public static readonly Vector128<float> SignMaskX = Vector128.Create(int.MinValue, 0, 0, 0).AsSingle();
        }

        public static class DoubleConstants
        {
            public static readonly Vector256<double> Identity = Vector256.Create(0d, 0d, 0d, 1d);

            public static readonly Vector256<double> SlerpEpsilon = Vector256.Create(1e-6d);
            public static readonly Vector256<double> OneMinusSlerpEpsilon = Vector256.Create(1 - 1e-6d);


            public static readonly Vector256<double> SignMaskXYZ = Vector256.Create(long.MinValue, long.MinValue, long.MinValue, 0).AsDouble();
        }
    }
}
