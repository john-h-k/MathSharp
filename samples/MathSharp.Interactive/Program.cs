using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using static MathSharp.Vector;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main()
        {
            Test(0);
            Test(30);
            Test(45);
            Test(60);
            Test(90);
        }

        private static void Test(float f)
        {
            Console.WriteLine($"For value {f}: ");

            var v = Vector128.Create(f);
            Console.WriteLine($"MathF Sin:             {MathF.Sin(f)}");
            Console.WriteLine($"MathSharp Sin:         {Sin(v).ToScalar()}");
            Console.WriteLine($"MathSharp SinEstimate: {SinEstimate(v).ToScalar()}");

            Console.WriteLine($"MathF Cos:             {MathF.Cos(f)}");
            Console.WriteLine($"MathSharp Cos:         {Cos(v).ToScalar()}");
            Console.WriteLine($"MathSharp CosEstimate: {CosEstimate(v).ToScalar()}");

            SinCos(v, out Vector128<float> sin, out Vector128<float> cos);
            SinCosEstimate(v, out Vector128<float> sinEst, out Vector128<float> cosEst);

            Console.WriteLine($"MathF SinCos:             {MathF.Sin(f)}, {MathF.Cos(f)}");
            Console.WriteLine($"MathSharp SinCos:         {sin.ToScalar()}, {cos.ToScalar()}");
            Console.WriteLine($"MathSharp SinCosEstimate: {sinEst.ToScalar()}, {cosEst.ToScalar()}");

            Console.WriteLine("\n");
        }
    }
}