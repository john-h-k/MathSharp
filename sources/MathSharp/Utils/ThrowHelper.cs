using System;
using System.Runtime.CompilerServices;

namespace MathSharp.Utils
{
    public static class ThrowHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowAccessViolationException(string message)
            => throw new AccessViolationException(message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ThrowForUnaligned16BPointer(void* p, string message)
        {
            ulong pAligned = (ulong)p;
            if (pAligned % 16 != 0)
            {
                ThrowAccessViolationException("Pointer not 16 byte aligned"); // TODO get right message
            }
        }
    }
}
