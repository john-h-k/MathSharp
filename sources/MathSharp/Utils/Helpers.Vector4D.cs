using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MathSharp.Utils
{
    internal static partial class Helpers
    {
        public static readonly double NoBitsSetDouble = 0d;
        public static readonly double AllBitsSetDouble = Unsafe.As<int, double>(ref Unsafe.AsRef(-1));
    }
}
