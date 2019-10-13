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

        public static void GetLowHigh(Vector256<double> vector, out Vector128<double> low, out Vector128<double> high)
        {
            low = vector.GetLower();
            high = vector.GetUpper();
        }

        public static Vector256<double> FromLowHigh(Vector128<double> low, Vector128<double> high)
            => Vector256.Create(low, high);
    }
}
