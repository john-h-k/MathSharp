using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
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
            BenchmarkRunner.Run<FpEqualityBenchmark>();
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

    public static class FloatEquality
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool StandardEquality(this float left, float right)
        {
            if (left == right)
                return true;

            return float.IsNaN(left) && float.IsNaN(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool UnsafeEquality(this float left, float right)
        {
            return Unsafe.As<float, uint>(ref left) == Unsafe.As<float, uint>(ref right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool IntrinsicEquality(this float left, float right)
        {
            var vLeft = Vector128.CreateScalarUnsafe(left).AsInt32();
            var vRight = Vector128.CreateScalarUnsafe(right).AsInt32();

            vLeft = Sse2.CompareEqual(vLeft, vRight);

            int mask = Sse.MoveMask(vLeft.AsSingle());

            return mask == -1;
        }
    }

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    public class FpEqualityBenchmark
    {
        public float Value1;
        public float Value2;

        [Benchmark]
        public bool NormalEquals()
        {
            return Value1.Equals(Value2);
        }

        [Benchmark]
        public bool UnsafeEquals()
        {
            return Value1.UnsafeEquality(Value2);
        }

        [Benchmark]
        public bool StandardEquals()
        {
            return Value1.StandardEquality(Value2);
        }

        [Benchmark]
        public bool IntrinsicEquals()
        {
            return Value1.IntrinsicEquality(Value2);
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
