using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp.Utils
{
    internal static partial class Helpers
    {
        // Has christ awful performance because JIT doesn't properly recognise it as producing a constant
        // when all 4 args are constant so generates a jump table. Do NOT use
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static byte Shuffle(byte a, byte b, byte c, byte d)
        //{
        //    return (byte)(
        //        (a << 6)
        //        | (b << 4)
        //        | (c << 2)
        //        | d);
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static float BoolToSimdBoolSingle(bool val) => val ? NoBitsSetSingle : AllBitsSetSingle;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double BoolToSimdBoolDouble(bool val) => val ? NoBitsSetDouble : AllBitsSetDouble;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int BoolToSimdBoolInt32(bool val) => val ? -1 : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static long BoolToSimdBoolInt64(bool val) => val ? -1 : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint BoolToSimdBoolUInt32(bool val) => val ? (uint)int.MaxValue + 1 : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong BoolToSimdBoolUInt64(bool val) => val ? (ulong)long.MaxValue + 1 : 0;
    }
}
