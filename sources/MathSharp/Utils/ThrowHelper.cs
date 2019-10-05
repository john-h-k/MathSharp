using System;
using System.Runtime.CompilerServices;

namespace MathSharp.Utils
{
    internal static class ThrowHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowAccessViolationException(string message)
            => throw new AccessViolationException(message);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentException(string message, string paramName, Exception? inner = null)
            => throw new ArgumentException(message, paramName, inner);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ThrowForUnaligned16BPointer(void* p, string message)
        {
            ulong pAligned = (ulong)p;
            if (pAligned % 16 != 0)
            {
                ThrowAccessViolationException(message);
            }
        }
    }
}
