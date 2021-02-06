using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.StorageTypes;
using MathSharp;
using BenchmarkDotNet.Attributes;
using System.Reflection;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.Vector.Single;

[module: SkipLocalsInit]

namespace MathSharp.Interactive
{
    internal unsafe class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<VectorArithmeticBenchmark>();
        }


        public static Vector2<float> Add(in Vector2<float> left, in Vector2<float> right)
            => left + right;

        public static Vector128<float> IsInCircle(Vector128<float> radius, Vector128<float> x, Vector128<float> y)
        {
            return Vector.CompareLessThanOrEqual(Vector.Sqrt(Vector.Add(Vector.Square(x), Vector.Square(y))), radius);
        }
    }
}