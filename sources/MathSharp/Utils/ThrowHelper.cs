using System;
using System.Runtime.CompilerServices;

namespace MathSharp.Utils
{
    internal static class ThrowHelper
    {
        public static void ThrowPlatformNotSupportedException(string message)
            => throw new PlatformNotSupportedException(message);


        public static void ThrowNotSupportedException(string message)
            => throw new NotSupportedException(message);
    }
}
