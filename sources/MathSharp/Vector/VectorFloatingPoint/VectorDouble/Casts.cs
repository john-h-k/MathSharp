using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Vector
    {
        public static Vector256<long> ConvertToInt64(Vector256<double> vector)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.ConvertToVector256Int64(Avx.ConvertToVector128Int32WithTruncation(vector));
            }

            return SoftwareFallback(vector);

            static Vector256<long> SoftwareFallback(Vector256<double> vector)
            {
                return Vector256.Create(
                    (long)X(vector),
                    (long)Y(vector),
                    (long)Z(vector),
                    (long)W(vector)
                );
            }
        }

        public static Vector256<int> ConvertToInt32(Vector256<double> vector)
        {
            return SoftwareFallback(vector);

            static Vector256<int> SoftwareFallback(Vector256<double> vector)
            {
                var v = Vector128.Create(
                    (int)X(vector),
                    (int)Y(vector),
                    (int)Z(vector),
                    (int)W(vector)
                );

                return Vector256.Create(v, v);
            }
        }
    }
}
