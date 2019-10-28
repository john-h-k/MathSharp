using System.Runtime.CompilerServices;

namespace MathSharp.Utils
{
    internal static partial class Helpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static float BoolToSimdBoolSingle(bool val) => val ? AllBitsSetSingle : NoBitsSetSingle;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double BoolToSimdBoolDouble(bool val) => val ? AllBitsSetDouble : NoBitsSetDouble;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int BoolToSimdBoolInt32(bool val) => val ? -1 : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static long BoolToSimdBoolInt64(bool val) => val ? -1 : 0;
    }
}
