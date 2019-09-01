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

        private static Vector3 Direction;
        private static Vector3 Offset;

        public static HwVector3 Example()
        {
            var dir = Direction.Load();
            var offset = Offset.Load();

            return dir * offset;
        }
    }
}