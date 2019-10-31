using System;
using System.Linq;
using System.Runtime.Intrinsics;
using static System.Runtime.InteropServices.RuntimeInformation;
// ReSharper disable PossibleNullReferenceException // You dont' know better than me R# ok

namespace MathSharp
{
    public static partial class Vector
    {
        private const string IsSupported = nameof(IsSupported);
        private const string IntrinsicNamespace = "System.Runtime.Intrinsics";

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
            var arch = $"{isa.Namespace!.Split('.').Last().ToUpper()}"; // Get ARM or x86 (e.g 'System.Runtime.Intrinsics.X86' -> 'X86')
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
