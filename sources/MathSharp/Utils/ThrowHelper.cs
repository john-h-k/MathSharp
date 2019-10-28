using System;
using System.Runtime.CompilerServices;

namespace MathSharp.Utils
{
    internal static class ThrowHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowPlatformNotSupportedException(string message)
            => throw new PlatformNotSupportedException(message);
    }
}
