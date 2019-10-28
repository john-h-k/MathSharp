using System;
using System.Linq;
using System.Runtime.Intrinsics;
using static System.Runtime.InteropServices.RuntimeInformation;

namespace MathSharp
{
    public static partial class Vector
    {
        private const string IsSupported = nameof(IsSupported);
        private const string IntrinsicNamespace = "System.Runtime.Intrinsics";

        public static string SupportSummary { get; } =
            "System:\n" +
            $"64 bit: {Environment.Is64BitProcess}\n" +
            $"Architecture: {ProcessArchitecture}{(ProcessArchitecture == OSArchitecture ? string.Empty : $"-- NOTE: Process arch {{{ProcessArchitecture}}} differs from OS arch {{{OSArchitecture}}}")}\n" +
            $"OS: {OSDescription}" +
            "\n\n\n" +
            string.Join('\n', typeof(Vector128<>).Assembly.GetTypes().Where(IsIsaClass).Select(FormatIsa));

        private static bool IsIsaClass(Type isa) => (isa.Namespace?.StartsWith(IntrinsicNamespace)).GetValueOrDefault() && isa.GetProperty(IsSupported) is object;

        private static string FormatIsa(Type isa) => $"{isa.Namespace!.Split('.').Last().ToUpper()} -- " + // Get ARM or x86
                                                     $"{(isa.IsNested ? $"{isa.DeclaringType!.Name.ToUpper()}-" : string.Empty) + isa.Name.ToUpper()}: " + // E.g SSE or SSE-X64
                                                     $"{((bool)isa.GetProperty(IsSupported)!.GetMethod!.Invoke(null, null)! ? "Supported" : "Unsupported")}";
    }
}
