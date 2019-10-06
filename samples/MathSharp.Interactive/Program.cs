using System;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.MatrixTests.Single;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using static MathSharp.Vector;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main()
        {
            Test(0f);
            Test(15f);
            Test(30f);
            Test(45f);
            Test(60f);
            Test(75f);
            Test(90f);
        }

        private static void Test(float f)
            => Test(f, true, true, true, true);

        private static void Test(float f, bool doSin = false, bool doCos = false, bool doTan = false, bool doSinCos = false)
        {
            Console.WriteLine($"For value {f}: ");

            var v = Vector128.Create(f);

            if (doSin)
            {
                Console.WriteLine($"MathF Sin:             {MathF.Sin(f)}");
                Console.WriteLine($"MathSharp Sin:         {Sin(v).ToScalar()}");
                Console.WriteLine($"MathSharp SinApprox:   {SinApprox(v).ToScalar()}");
            }

            if (doCos)
            {
                Console.WriteLine($"MathF Cos:             {MathF.Cos(f)}");
                Console.WriteLine($"MathSharp Cos:         {Cos(v).ToScalar()}");
                Console.WriteLine($"MathSharp CosApprox:   {CosApprox(v).ToScalar()}");
            }

            if (doTan)
            {
                Console.WriteLine($"MathF Tan:             {MathF.Tan(f)}");
                Console.WriteLine($"MathSharp Tan:         {Tan(v).ToScalar()}");
                Console.WriteLine($"MathSharp TanApprox:   {TanApprox(v).ToScalar()}");
            }

            if (doSinCos)
            {
                SinCos(v, out Vector128<float> sin, out Vector128<float> cos);
                SinCosApprox(v, out Vector128<float> sinEst, out Vector128<float> cosEst);

                Console.WriteLine($"MathF SinCos:             {MathF.Sin(f)}, {MathF.Cos(f)}");
                Console.WriteLine($"MathSharp SinCos:         {sin.ToScalar()}, {cos.ToScalar()}");
                Console.WriteLine($"MathSharp SinCosApprox:   {sinEst.ToScalar()}, {cosEst.ToScalar()}");
            }

            Console.WriteLine("\n");
        }
    }
}