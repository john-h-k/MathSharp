using System;

namespace MathSharp.Attributes
{
    [Flags]
    public enum InstructionSets
    {
        None = 0,
        Lzcnt = 1 << 0,
        Popcnt = 1 << 1,
        Sse = 1 << 2,
        Sse2 = 1 << 3,
        Sse3 = 1 << 4,
        Sse41 = 1 << 5,
        Sse42 = 1 << 6,
        Ssse3 = 1 << 7,
        Avx = 1 << 8,
        Avx2 = 1 << 9,
        Avx512 = 1 << 10
    }
}