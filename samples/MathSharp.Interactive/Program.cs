using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.StorageTypes;
using MathSharp;
using MathSharp.Interactive.Benchmarks.Vector.Single;

using Vector4 = System.Numerics.Vector4;
using BenchmarkDotNet.Running;

namespace MathSharp.Interactive
{
    internal unsafe class Program
    {
        private static readonly string StaticReadonly = "A";
        private const string Const = "C";

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<SelectionBenchmark>();
        }

        public static ref byte Ref(Span<byte> span) => ref span.GetPinnableReference();
    }
}