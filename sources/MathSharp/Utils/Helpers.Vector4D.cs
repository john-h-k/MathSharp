using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MathSharp.Utils
{
    internal static partial class Helpers
    {
        public static readonly double NoBitsSetDouble = 0d;
        public static readonly double AllBitsSetDouble = BitConverter.Int64BitsToDouble(-1);

        public static void GetLowHigh<T>(Vector256<T> vector, out Vector128<T> low, out Vector128<T> high) where T : struct
        {
            low = vector.GetLower();
            high = vector.GetUpper();
        }

        public static Vector256<T> FromLowHigh<T>(Vector128<T> low, Vector128<T> high) where T : struct 
            => low.ToVector256Unsafe().WithUpper(high);
    }
}
