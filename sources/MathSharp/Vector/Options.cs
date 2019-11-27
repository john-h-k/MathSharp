using System.Runtime.CompilerServices;

namespace MathSharp
{
    public static class Options
    {
        // Used to allow things like changing (x + y) * z to FMA(x, y, z)
        // or 1 / n to ReciprocalApprox(n)
        /// <summary>
        /// Indicates whether math used in methods is strict and IEEE compliant.
        /// </summary>
        public static bool StrictMath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;  
        }

        // Used to allow things like Max(a, b) discarding NaNs
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Indicates whether behaviour around +inf, -inf, and NaN must be respected as IEEE dictates.
        /// </summary>
        public static bool StrictNonFiniteBehaviour
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }
    }
}
