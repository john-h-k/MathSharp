using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Vector
    {
        /// <summary>
        /// Converts each element of a <see cref="Vector128{Int32}"/> to the equivalent <see cref="Single"/>.
        /// Equivalent to a per-element C#-style cast
        /// </summary>
        /// <param name="vector">The vector of <see cref="Int32"/>s</param>
        /// <returns>A vector that contains each element of <paramref name="vector"/> casted to a <see cref="Single"/></returns>
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

        /// <summary>
        /// Converts each element of a <see cref="Vector128{Single}"/> to the equivalent <see cref="Int32"/>.
        /// Equivalent to a per-element C#-style cast
        /// </summary>
        /// <param name="vector">The vector of <see cref="Single"/>s</param>
        /// <returns>A vector that contains each element of <paramref name="vector"/> casted to a <see cref="Int32"/></returns>
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
