using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MathSharp.Matrix;
using OpenTK;

namespace MathSharp.Interactive
{
    using JohnVector = Vector128<float>;
    using OpenTKVector = OpenTK.Vector3;
    using SysNumVector = System.Numerics.Vector3;

    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<MathBenchmark>();
        }

        public static unsafe bool IsAligned()
        {
            byte x;
            byte y;
            MatrixF matrix;

            x = 11;
            y = 11;

            return ((ulong)&matrix) % 16 == 0 && (ulong)&x > (ulong)&matrix && Math.Abs(&x - &y) == 1;
        }
    }

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    public class MathBenchmark
    {
        private const int Count = 128;

        public OpenTKVector[]? OpenTkVectorsSrc;
        public JohnVector[]? JohnVectorsSrc;
        public SysNumVector[]? SysNumVectorsSrc;

        public OpenTKVector[]? OpenTkVectorsDest;
        public JohnVector[]? JohnVectorsDest;
        public SysNumVector[]? SysNumVectorsDest;

        [GlobalSetup]
        public void Setup()
        {

            OpenTkVectorsSrc = new OpenTKVector[Count];
            OpenTkVectorsDest = new OpenTKVector[Count];
            Array.Fill(OpenTkVectorsSrc, new OpenTKVector(1, 4, 9));

            SysNumVectorsSrc = new SysNumVector[Count];
            SysNumVectorsDest = new SysNumVector[Count];
            Array.Fill(SysNumVectorsDest, new SysNumVector(1, 4, 9));

            JohnVectorsSrc = new JohnVector[Count];
            JohnVectorsDest = new JohnVector[Count];
            JohnVector vec = Vector128.Create(1f, 4f, 9f, 0f);
            Array.Fill(JohnVectorsSrc, vec);
        }

        [Benchmark]
        public void JohnMul()
        {
            for (var i = 0; i < Count; i++)
            {
                JohnVectorsDest![i] = VectorFloat.Arithmetic.PerElementMultiply(JohnVectorsSrc![i], JohnVectorsSrc![i]);
            }
        }

        [Benchmark]
        public void OpenTkMul()
        {
            for (var i = 0; i < Count; i++)
            {
                OpenTkVectorsDest![i] =  OpenTkVectorsSrc![i] * OpenTkVectorsSrc![i];
            }
        }

        [Benchmark]
        public void SysNumMul()
        {
            for (var i = 0; i < Count; i++)
            {
                SysNumVectorsDest![i] = SysNumVectorsSrc![i] * SysNumVectorsSrc![i];
            }
        }
    }
}
