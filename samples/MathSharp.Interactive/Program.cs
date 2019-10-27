using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using MathSharp.Interactive.Benchmarks.Vector.Single;

namespace MathSharp.Interactive
{
    internal unsafe class Program
    {
        private static void Main()
        {
        }

        private static bool Equal(MatrixSingle left, MatrixSingle right) => Matrix.CompareEqual(left, right);
    }

    public class Cross4DBenchmark
    {
        public Vector128<float> One, Two, Three;

        [GlobalSetup]
        public void Setup()
        {
            var rng = new Random(0x21438420);
            One = Vector128.Create(Next(), Next(), Next(), Next());
            Two = Vector128.Create(Next(), Next(), Next(), Next());
            Three = Vector128.Create(Next(), Next(), Next(), Next());

            float Next() => (float)rng.NextDouble();
        }

        [Benchmark]
        public Vector128<float> HardwareIntrinsics() => Vector.CrossProduct4D(One, Two, Three);

        [Benchmark]
        public Vector128<float> Software() => Vector.CrossProduct4D_Software(One, Two, Three);
    }
        
}