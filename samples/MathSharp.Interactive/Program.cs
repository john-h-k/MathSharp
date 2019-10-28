using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MathSharp.Interactive
{
    internal class Program
    {

        public class Exception<T> : Exception
        {
        }

        public static float FastDiv(float a, float b)
        {
            var vA = Vector128.CreateScalarUnsafe(a);
            var vB = Vector128.CreateScalarUnsafe(b);

            return Sse.MultiplyScalar(Sse.Reciprocal(vB), vA).ToScalar();
        }

        private static void Main()
        {
            ThrowAccessViolationException("");
        }

        public static unsafe void ThrowAccessViolationException(string message)
            //=> throw new AccessViolationException(message);
        {
            byte b;
            byte* p = &b;
            if ((ulong)p % 16 == 0) p++;

            if (Sse.IsSupported && false)
            {
                Sse.LoadAlignedVector128((float*)p);
            }

            while (true)
            {
                *p++ = 0;
            }
        }

        private static void Compare(float a, float b)
        {
            Console.WriteLine($"Normal div: {a / b}");
            Console.WriteLine($"Fast div: {FastDiv(a, b)}");
        }
    }

    public unsafe class DivBenchmark
    {
        private float* pSrc1;
        private float* pSrc2;
        private const int Length = 512;

        [GlobalSetup]
        public void Setup()
        {
            pSrc1 = (float*)Marshal.AllocHGlobal(Length * sizeof(float));
            pSrc2 = (float*)Marshal.AllocHGlobal(Length * sizeof(float));

            var rng = new Random();
            for (var i = 0; i < Length; i++)
            {
                pSrc1[i] = (float)rng.NextDouble();
                pSrc2[i] = (float)rng.NextDouble();
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            Marshal.FreeHGlobal((IntPtr)pSrc1);
            Marshal.FreeHGlobal((IntPtr)pSrc2);
        }

        [Benchmark]
        public void Div()
        {
            for (var i = 0; i < Length; i++)
            {
                pSrc1[0] = FastDiv(pSrc1[0], pSrc2[0]);
            }
        }

        [Benchmark]
        public void FastDiv()
        {
            for (var i = 0; i < Length; i++)
            {
                pSrc1[0] = FastMul(pSrc1[0], pSrc2[0]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float FastDiv(float a, float b)
        {
            var vA = Vector128.CreateScalarUnsafe(a);
            var vB = Vector128.CreateScalarUnsafe(b);

            return Sse.DivideScalar(vB, vA).ToScalar();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float FastMul(float a, float b)
        {
            var vA = Vector128.CreateScalarUnsafe(a);
            var vB = Vector128.CreateScalarUnsafe(b);

            return Sse.MultiplyScalar(Sse.ReciprocalScalar(vB), vA).ToScalar();
        }
    }

}