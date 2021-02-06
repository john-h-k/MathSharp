using System.Runtime.CompilerServices;

namespace MathSharp
{
    public static partial class Vector
    {
        internal const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;
    }
}
