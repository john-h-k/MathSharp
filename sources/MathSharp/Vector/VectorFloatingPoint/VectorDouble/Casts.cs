using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Vector
    {
        public static Vector256<long> ConvertToInt64(Vector256<double> vector)
        {
            // TODO poorly in need of acceleration

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
