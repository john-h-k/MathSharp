using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MathSharp.Matrix;
using static MathSharp.VectorF;
using OpenTK;
using OpenTK.Platform.Windows;

namespace MathSharp.Interactive
{
    using JohnVector = Vector128<float>;
    using JohnVector3 = System.Numerics.Vector4;
    using OpenTKVector = OpenTK.Vector4;
    using SysNumVector = System.Numerics.Vector4;

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
    public class Normalize3DBenchmark
    {
        private readonly Vector128<float> _value = Vector128.Create(1f, 2f, 3f, 4f);
        private readonly int _iter = 1;

        [Benchmark]
        public Vector128<float> Normalize_Nested()
        {
            JohnVector x = _value;
            for (var i = 0; i < _iter; i++)
            {
                x = VectorF.Normalize3D(x);
            }

            return x;
        }

        [Benchmark]
        public Vector128<float> Normalize_Inline()
        {
            JohnVector x = _value;
            for (var i = 0; i < _iter; i++)
            {
            }

            return x;
        }
    }

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    public class MathBenchmark
    {
        private const int Count = 128;

        public OpenTKVector[]? OpenTkVectorsSrc;
        public JohnVector3[]? JohnVectorsSrc;
        public SysNumVector[]? SysNumVectorsSrc;

        public OpenTKVector[]? OpenTkVectorsDest;
        public JohnVector3[]? JohnVectorsDest;
        public SysNumVector[]? SysNumVectorsDest;

        [GlobalSetup]
        public void Setup()
        {
            Trace.Assert(Avx2.IsSupported);
            OpenTkVectorsSrc = new OpenTKVector[Count];
            OpenTkVectorsDest = new OpenTKVector[Count];
            Array.Fill(OpenTkVectorsSrc, new OpenTKVector(1f, 4f, 9f, 16f));

            SysNumVectorsSrc = new SysNumVector[Count];
            SysNumVectorsDest = new SysNumVector[Count];
            Array.Fill(SysNumVectorsDest, new SysNumVector(1f, 4f, 9f, 16f));

            JohnVectorsSrc = new JohnVector3[Count];
            JohnVectorsDest = new JohnVector3[Count];
            Array.Fill(JohnVectorsSrc, new JohnVector3(1f, 6f, 9f, 16f));
        }

        /* Benchmark:
         * * 128 Vector4<float> 's, 
         * * Then the operations for each element (where A is the vector)
         *      - a =  Multiply(a, a)
         *      - a =  Normalize(a)
         *      - a =  Subtract(a, a)
         *      - a =  Normalize(a)
         *      - a =  Multiply(a, DotProduct(a))
         */


        [Benchmark]
        public void John()
        {
            for (var i = 0; i < Count; i++)
            {
                JohnVector vector = JohnVectorsSrc![i].Load();
                vector = Multiply(vector, vector);
                vector = Normalize4D(vector);
                vector = Subtract(vector, vector);
                vector = Normalize4D(vector);
                vector = Multiply(vector, DotProduct4D(vector, vector));
                vector.Store(out JohnVectorsDest![i]);
            }
        }

        [Benchmark]
        public void OpenTk()
        {
            for (var i = 0; i < Count; i++)
            {
                OpenTKVector vector = OpenTkVectorsSrc![i];
                vector = OpenTKVector.Multiply(vector, vector);
                vector = OpenTKVector.Normalize(vector);
                vector = OpenTKVector.Subtract(vector, vector);
                vector = OpenTKVector.Normalize(vector);
                vector = OpenTKVector.Multiply(vector, OpenTKVector.Dot(vector, vector));
                OpenTkVectorsDest![i] = vector;
            }
        }

        [Benchmark]
        public void SysNum()
        {
            for (var i = 0; i < Count; i++)
            {
                JohnVector3 vector = SysNumVectorsSrc![i];
                vector = SysNumVector.Multiply(vector, vector);
                vector = SysNumVector.Normalize(vector);
                vector = SysNumVector.Subtract(vector, vector);
                vector = SysNumVector.Normalize(vector);
                vector = SysNumVector.Multiply(vector, SysNumVector.Dot(vector, vector));
                SysNumVectorsDest![i] = vector;
            }
        }
    }

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    public class ExtendedMathBenchmark
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
            //Array.Fill(OpenTkVectorsSrc, new OpenTKVector(1, 4, 9, 0));

            SysNumVectorsSrc = new SysNumVector[Count];
            SysNumVectorsDest = new SysNumVector[Count];
            //Array.Fill(SysNumVectorsDest, new SysNumVector(1, 4, 9, 0));

            JohnVectorsSrc = new JohnVector[Count];
            JohnVectorsDest = new JohnVector[Count];
            // JohnVector vec = Vector128.Create(1f, 4f, 9f, 0f);
            // Array.Fill(JohnVectorsSrc, vec);
        }

        [Benchmark]
        public void John()
        {
            for (var i = 0; i < Count; i++)
            {
                JohnVector vector = JohnVectorsSrc![i];
                // vector = VectorF.Normalize3D(vector);
            }
        }

        [Benchmark]
        public void OpenTk()
        {
            for (var i = 0; i < Count; i++)
            {
                OpenTKVector vector = OpenTkVectorsSrc![i];
            }
        }

        [Benchmark]
        public void SysNum()
        {
            for (var i = 0; i < Count; i++)
            {
                SysNumVector vector = SysNumVectorsSrc![i];
            }
        }
    }
}
