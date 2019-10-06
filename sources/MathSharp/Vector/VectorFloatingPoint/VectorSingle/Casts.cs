using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Vector
    {
        public static Vector128<float> ConvertToSingle(Vector128<int> vector)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.ConvertToVector128Single(vector);
            }

            return SoftwareFallback(vector);

            static Vector128<float> SoftwareFallback(Vector128<int> vector)
            {
                return Vector128.Create(
                    (float)X(vector),
                    (float)Y(vector),
                    (float)Z(vector),
                    (float)W(vector)
                );
            }
        }

        public static Vector128<int> ConvertToInt32(Vector128<float> vector)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.ConvertToVector128Int32WithTruncation(vector);
            }

            return SoftwareFallback(vector);

            static Vector128<int> SoftwareFallback(Vector128<float> vector)
            {
                return Vector128.Create(
                    (int)X(vector),
                    (int)Y(vector),
                    (int)Z(vector),
                    (int)W(vector)
                );
            }
        }
    }
}
