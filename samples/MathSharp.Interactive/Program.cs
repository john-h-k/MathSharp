using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.Vector.Single;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<AnchoredScaleBenchmark>();
        }
    }
}