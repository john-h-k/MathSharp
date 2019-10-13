using System.Runtime.CompilerServices;

namespace MathSharp
{
    public static class Options
    {
        public static bool AllowImpreciseMath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => true;
        }
    }
}
