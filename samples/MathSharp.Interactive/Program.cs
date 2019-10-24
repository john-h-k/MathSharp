using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using static System.AttributeTargets;

namespace MathSharp.Interactive
{
    internal unsafe class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<SineWaveBenchmark>();
        }
    }
}