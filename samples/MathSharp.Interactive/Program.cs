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

namespace MathSharp.Interactive
{
    internal unsafe class Program
    {
        public static void Main(string[] args)
        {
            var rand = new Random();
            var inside = 0;

            const int sampleSize = 1_000_000_000;

            for (var i = 0; i < sampleSize; i++)
            {
                if (IsInCircle(0.25, rand.NextDouble() - 0.5, rand.NextDouble() - 0.5)) inside++;
            }

            Avx2.LoadV

            Console.WriteLine(4 * (double)inside / sampleSize);

            static bool IsInCircle(double radiusSquared, double x, double y)
            {
                return x * x + y * y < radiusSquared;
            }
        }

        public static Vector128<float> IsInCircle(Vector128<float> radius, Vector128<float> x, Vector128<float> y)
        {
            return Vector.CompareLessThanOrEqual(Vector.Sqrt(Vector.Add(Vector.Square(x), Vector.Square(y))), radius);
        }
    }
}