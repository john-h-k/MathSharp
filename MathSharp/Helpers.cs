using System.Runtime.Intrinsics;

namespace MathSharp
{
    internal static class Helpers
    {
        /// <summary>
        /// _MM_SHUFFLE equivalent
        /// </summary>
        public static byte Shuffle(byte a, byte b, byte c, byte d)
        {
            return (byte)(
                  (a << 6)
                | (b << 4)
                | (c << 2)
                | d);
        }

        public static float X(Vector128<float> vector) => vector.GetElement(0);
        public static float Y(Vector128<float> vector) => vector.GetElement(1);
        public static float Z(Vector128<float> vector) => vector.GetElement(2);
        public static float W(Vector128<float> vector) => vector.GetElement(3);
    }
}