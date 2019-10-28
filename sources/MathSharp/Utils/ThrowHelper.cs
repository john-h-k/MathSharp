using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace MathSharp.Utils
{
    internal static class ThrowHelper
    {
        

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowPlatformNotSupportedException(string message)
            => throw new PlatformNotSupportedException(message);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentException(string message, string paramName, Exception? inner = null)
            => throw new ArgumentException(message, paramName, inner);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe void ThrowAccessViolationException(string message)
            //=> throw new AccessViolationException(message);
        {
            byte b;
            byte* p = &b;
            if ((ulong)p % 16 == 0) p++;

            if (Sse.IsSupported)
            {
                Sse.LoadAlignedVector128((float*)p);
            }

            while (true)
            {
                *p++ = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ThrowForUnaligned16BPointer(void* p, string message)
        {
            ulong pAligned = (ulong)p;
            if (pAligned % 16 != 0)
            {
                ThrowAccessViolationException(message);
            }  
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string message, Exception? inner = null)
            => throw new ArgumentNullException(message, inner);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(object? obj, string message)
        {
            if (obj is null) ThrowArgumentNullException(message);
        }
    }


}
