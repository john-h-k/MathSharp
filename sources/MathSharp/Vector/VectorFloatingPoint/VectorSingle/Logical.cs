using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Vector
    {
        /// <summary>
        /// Retrieves the X component from a vector
        /// </summary>
        /// <param name="vector">The vector to retrieve the component from</param>
        /// <returns>The X component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static float GetX(Vector128<float> vector) => vector.GetElement(0);

        /// <summary>
        /// Retrieves the Y component from a vector
        /// </summary>
        /// <param name="vector">The vector to retrieve the component from</param>
        /// <returns>The Y component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static float GetY(Vector128<float> vector) => vector.GetElement(1);

        /// <summary>
        /// Retrieves the Z component from a vector
        /// </summary>
        /// <param name="vector">The vector to retrieve the component from</param>
        /// <returns>The Z component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static float GetZ(Vector128<float> vector) => vector.GetElement(2);

        /// <summary>
        /// Retrieves the W component from a vector
        /// </summary>
        /// <param name="vector">The vector to retrieve the component from</param>
        /// <returns>The W component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static float GetW(Vector128<float> vector) => vector.GetElement(3);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Shuffle(Vector128<float> vector, byte control)
        {
            if (Avx.IsSupported)
            {
                return Avx.Permute(vector, control);
            }

            return Shuffle(vector, vector, control);
        }

        /// <summary>
        /// Creates a new vector filled with the X component of a given vector
        /// </summary>
        /// <param name="vector">The vector to use as the source</param>
        /// <returns>A vector filled with the X component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FillWithX(Vector128<float> vector)
            => Shuffle(vector, ShuffleValues.XXXX);



        /// <summary>
        /// Creates a new vector filled with the Y component of a given vector
        /// </summary>
        /// <param name="vector">The vector to use as the source</param>
        /// <returns>A vector filled with the Y component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FillWithY(Vector128<float> vector)
                => Shuffle(vector, ShuffleValues.YYYY);


        /// <summary>
        /// Creates a new vector filled with the Z component of a given vector
        /// </summary>
        /// <param name="vector">The vector to use as the source</param>
        /// <returns>A vector filled with the Z component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FillWithZ(Vector128<float> vector)
                    => Shuffle(vector, ShuffleValues.ZZZZ);



        /// <summary>
        /// Creates a new vector filled with the W component of a given vector
        /// </summary>
        /// <param name="vector">The vector to use as the source</param>
        /// <returns>A vector filled with the W component of <paramref name="vector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FillWithW(Vector128<float> vector)
            => Shuffle(vector, ShuffleValues.WWWW);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Shuffle(Vector128<float> left, Vector128<float> right, byte control)
        {
            if (Sse.IsSupported)
            {
                return Sse.Shuffle(left, right, control);
            }

            return Shuffle_Software(left, right, control);
        }
    }
}
