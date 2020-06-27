using System;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Transactions;
using static System.Runtime.InteropServices.RuntimeInformation;
// ReSharper disable PossibleNullReferenceException // You dont' know better than me R# ok

namespace MathSharp
{
    public static partial class Vector
    {
        private const string IsSupported = nameof(IsSupported);
        private const string IntrinsicNamespace = "System.Runtime.Intrinsics";

        /// <summary>
        /// Indicates whether a type is a supported type for use in SIMD vectors
        /// </summary>
        /// <typeparam name="T">The vectorSize element type</typeparam>
        /// <returns><see langword="true"/> if the type is a valid vectorSize element, else <see langword="false"/></returns>
        public static bool IsSupportedVectorType<T>()
            => typeof(T) == typeof(byte)
               || typeof(T) == typeof(sbyte)
               || typeof(T) == typeof(ushort)
               || typeof(T) == typeof(short)
               || typeof(T) == typeof(uint)
               || typeof(T) == typeof(int)
               || typeof(T) == typeof(ulong)
               || typeof(T) == typeof(long)
               || typeof(T) == typeof(float)
               || typeof(T) == typeof(double);

        /// <summary>
        /// Whether <see cref="Vector64{T}"/> is hardware accelerated in any way
        /// </summary>
        public static bool IsVector64HwAccelerated => false; // TODO ARM

        /// <summary>
        /// Whether <see cref="Vector128{T}"/> is hardware accelerated in any way
        /// </summary>
        public static bool IsVector128HwAccelerated => Sse.IsSupported; // TODO ARM

        /// <summary>
        /// Whether <see cref="Vector256{T}"/> is hardware accelerated in any way
        /// </summary>
        public static bool IsVector256HwAccelerated => Avx.IsSupported; // TODO ARM

        /// <summary>
        /// Represents the size, in bits, of a vectorSize
        /// </summary>
        public enum VectorSize { V64, V128, V256 } // do not change

        /// <summary>
        /// Indicates whether hardware acceleration is available for a certain type and vectorSize size
        /// </summary>
        /// <typeparam name="T">The type to check hardware support for</typeparam>
        /// <param name="vectorSize">The size of the vector to check hardware support for</param>
        /// <returns><see langword="true"/> if the type is a valid vectorSize element, and is hardware accelerated for the size <paramref name="vectorSize"/>, else <see langword="false"/></returns>
        public static bool IsHwAccelerated<T>(VectorSize vectorSize)
        {
            if ((int)vectorSize > 2 || vectorSize < 0)
            {
                throw new ArgumentException("Invalid vectorSize size");
            }

            if (!IsSupportedVectorType<T>()) return false;

            if (typeof(T) == typeof(float))
            {
#pragma warning disable CS8509 // The switch expression does not handle all possible inputs (it is not exhaustive).
                return vectorSize switch
                {
                    VectorSize.V64 => false,
                    VectorSize.V128 => Sse.IsSupported /* || AdvSimd.IsSupported */,
                    VectorSize.V256 => Avx.IsSupported
                };
            }

            if (typeof(T) == typeof(double))
            {
                return vectorSize switch
#pragma warning restore CS8509 // The switch expression does not handle all possible inputs (it is not exhaustive).
                {
                    VectorSize.V64 => false,
                    VectorSize.V128 => Sse2.IsSupported,
                    VectorSize.V256 => Avx.IsSupported
                };
            }

            if (vectorSize == VectorSize.V64)
            {
                return false;
            }

            if (vectorSize == VectorSize.V128)
            {
                return Sse2.IsSupported;
            }

            if (vectorSize == VectorSize.V256)
            {
                return Avx2.IsSupported;
            }

            return default;
        }

        /// <summary>
        /// A string that provides a human-understandable interpretation of hardware details
        /// </summary>
        public static string SupportSummary { get; } =
            "System:\n" +
            $"    64 bit: {Environment.Is64BitProcess}\n" +
            $"    Architecture: {ProcessArchitecture}{(ProcessArchitecture == OSArchitecture ? string.Empty : $"-- NOTE: Process arch {{{ProcessArchitecture}}} differs from OS arch {{{OSArchitecture}}}")}\n" +
            $"    OS: {OSDescription}" +
            "\n\n\n" +
            "Instruction Sets:\n    " +
            string.Join("\n    ", typeof(Vector128<>).Assembly.GetTypes().Where(IsIsaClass).Select(FormatIsa));

        private static bool IsIsaClass(Type isa) => (isa.Namespace?.StartsWith(IntrinsicNamespace)).GetValueOrDefault() && isa.GetProperty(IsSupported) is object;
        private const string EnvVarPrefix = "COMPlus_Enable";

        private static string FormatIsa(Type isa)
        {
            var arch = $"{isa.Namespace!.Split('.')[^1].ToUpper()}"; // Get ARM or x86 (e.g 'System.Runtime.Intrinsics.X86' -> 'X86')
            var name = $"{(isa.IsNested ? $"{isa.DeclaringType!.Name.ToUpper()}-" : string.Empty) + isa.Name.ToUpper()}"; // E.g 'Sse' -> SSE or 'Sse.X64' -> SSE-X64

            var envVar = EnvVarPrefix + isa.Name.ToUpper();

            var res = (bool)isa.GetProperty(IsSupported)!.GetMethod!.Invoke(null, null)!;

            string isSupported = res ? "Supported" : "Unsupported";

            var env = Environment.GetEnvironmentVariable(envVar);

            if (env == "0")
            {
                if (!res)
                {
                    isSupported += $" (ISA disabled by Environment Variable; {envVar} set to \"0\")";
                }
                else
                {
                    isSupported += $" (Environment Variable {envVar} set to \"0\" is set but ISA is still supported; restart the runtime or ensure it is a user or system wide variable)";
                }
            }

            return $"{arch} -- {name}: {isSupported}";
        }
    }
}
