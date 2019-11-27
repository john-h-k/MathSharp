using System.Runtime.CompilerServices;

namespace MathSharp.Utils
{
    internal static partial class Helpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static float AsMaskSingle(bool val) => val ? AllBitsSetSingle : NoBitsSetSingle;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double AsMaskDouble(bool val) => val ? AllBitsSetDouble : NoBitsSetDouble;
    }
}
