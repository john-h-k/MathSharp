using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Vector
    {
        // Used by trig. Low quality because it narrows to Int32.
        internal static Vector256<long> ConvertToInt64(Vector256<double> vector)
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
    }
}
