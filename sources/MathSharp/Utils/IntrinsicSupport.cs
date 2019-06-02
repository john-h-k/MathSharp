#if NETCOREAPP3_0

#define AVX2
#define AVX
#define SSE42_X64
#define SSE42
#define SSE41_X64
#define SSE41
#define SSE3
#define SSE2_X64
#define SSE2
#define SSE_X64
#define SSE

#define LZCNT_X64
#define LZCNT
#define POPCNT_X64
#define POPCNT
#define BMI2_X64
#define BMI2
#define BMI1_X64
#define BMI1

#define FMA

#define PCLMULQDQ

#define AES

#elif NETPLATFORMEXTENSIONS2_1

#define AVX2
#define AVX
#define SSE42
#define SSE41
#define SSE3
#define SSE2
#define SSE

#define LZCNT
#define POPCNT

#endif

#if NO_INTRINSICS || SOFTWARE_ONLY
#undef AVX512
#undef AVX2
#undef AVX
#undef SSE42_X64
#undef SSE42
#undef SSE41_X64
#undef SSE41
#undef SSE3
#undef SSE2_X64
#undef SSE2
#undef SSE_X64
#undef SSE

#undef LZCNT_X64
#undef LZCNT
#undef POPCNT_X64
#undef POPCNT
#undef BMI2_X64
#undef BMI2
#undef BMI1_X64
#undef BMI1

#undef FMA

#undef PCLMULQDQ

#undef AES

#undef NEON

#endif


// ReSharper disable All

namespace MathSharp
{
    /// <summary>
    /// Checks whether it is allowed for the program to use certain intrinsic supports
    /// </summary>
    // TODO investigate inlining - ALL of these *should* be inlined to be zero-cost ( do we need AggressiveInlining? - unlikely but possible )
    internal static class IntrinsicSupport
    {
        #region X86

        #region Vector ISAs

        public static bool Avx512 =>
#if !AVX512
            false;
#else
            false;
#error AVX512 unsupported at the moment
#endif

        public static bool Avx2 =>
#if !AVX2
            false;
#else
            System.Runtime.Intrinsics.X86.Avx2.IsSupported;
#endif

        public static bool Avx =>
#if !AVX
            false;
#else
            System.Runtime.Intrinsics.X86.Avx.IsSupported;
#endif


        public static bool Sse42 =>
#if !SSE42
            false;
#else
            System.Runtime.Intrinsics.X86.Sse42.IsSupported;
#endif

        public static bool Sse41 =>
#if !SSE41
            false;
#else
            System.Runtime.Intrinsics.X86.Sse41.IsSupported;
#endif

        public static bool Sse3 =>
#if !SSE3
            false;
#else
            System.Runtime.Intrinsics.X86.Sse3.IsSupported;
#endif

        public static bool Sse2 =>
#if !SSE2
            false;
#else
            System.Runtime.Intrinsics.X86.Sse2.IsSupported;
#endif

        public static bool Sse =>
#if !SSE
            false;
#else
            System.Runtime.Intrinsics.X86.Sse.IsSupported;
#endif

        public static bool Sse42_X64 =>
#if !SSE42_X64
            false;
#else
            System.Runtime.Intrinsics.X86.Sse42.X64.IsSupported;
#endif

        public static bool Sse41_X64 =>
#if !SSE41_X64
            false;
#else
            System.Runtime.Intrinsics.X86.Sse41.X64.IsSupported;
#endif

        public static bool Sse2_X64 =>
#if !SSE2_X64
            false;
#else
        System.Runtime.Intrinsics.X86.Sse2.X64.IsSupported;
#endif

        public static bool Sse_X64 =>
#if !SSE_X64
            false;
#else
            System.Runtime.Intrinsics.X86.Sse.X64.IsSupported;
#endif

        public static bool Ssse3 =>
#if !SSSE3
            false;
#else
            System.Runtime.Intrinsics.X86.Ssse3.IsSupported;
#endif

        #endregion

        #region Bit Manipulation ISAs

        public static bool Bmi1 =>
#if !BMI1
            false;
#else
            System.Runtime.Intrinsics.X86.Bmi1.IsSupported;
#endif

        public static bool Bmi2 =>
#if !BMI2
            false;
#else
            System.Runtime.Intrinsics.X86.Bmi2.IsSupported;
#endif

        public static bool Bmi1_X64 =>
#if !BMI1_X64
            false;
#else
            System.Runtime.Intrinsics.X86.Bmi1.X64.IsSupported;
#endif

        public static bool Bmi2_X64 =>
#if !BMI2_X64
            false;
#else
            System.Runtime.Intrinsics.X86.Bmi2.X64.IsSupported;
#endif

        public static bool Lzcnt =>
#if !LZCNT
            false;
#else
            System.Runtime.Intrinsics.X86.Lzcnt.IsSupported;
#endif

        public static bool Popcnt =>
#if !POPCNT
            false;
#else
            System.Runtime.Intrinsics.X86.Popcnt.IsSupported;
#endif

        public static bool Lzcnt_X64 =>
#if !LZCNT_X64
            false;
#else
            System.Runtime.Intrinsics.X86.Lzcnt.X64.IsSupported;
#endif

        public static bool Popcnt_X64 =>
#if !POPCNT_X64
            false;
#else
            System.Runtime.Intrinsics.X86.Popcnt.X64.IsSupported;
#endif

        #endregion

        #region Floating point ISAs

        public static bool Fma =>
#if !FMA
            false;
#else
            System.Runtime.Intrinsics.X86.Fma.IsSupported;
#endif

        #endregion

        #region Integer ISAs

        public static bool Pclmulqdq =>
#if !PCLMULQDQ
            false;
#else
            System.Runtime.Intrinsics.X86.Pclmulqdq.IsSupported;
#endif

        #endregion

        #region Encryption ISAs

        public static bool Aes =>
#if !AES
            false;
#else
            System.Runtime.Intrinsics.X86.Aes.IsSupported;
#endif

        #endregion

        #endregion

        #region ARM

        #region Vector ISAs

        public static bool Neon =>
#if !NEON
            false;
#else
    false;
#error NEON unsupported at the moment
#endif

        #endregion

        #endregion
    }
}
