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
using MathSharp.VectorFloat;
using OpenTK;
using OpenTK.Platform.Windows;

namespace MathSharp.Interactive
{
    using JohnVector = Vector128<float>;
    using JohnVector3 = System.Numerics.Vector3;
    using OpenTKVector = OpenTK.Vector3;
    using SysNumVector = System.Numerics.Vector3;

    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Normalize3DBenchmark>();
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
                x = VectorMaths.Normalize3D(x);
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
            Array.Fill(OpenTkVectorsSrc, new OpenTKVector(1f, 4f, 9f));

            SysNumVectorsSrc = new SysNumVector[Count];
            SysNumVectorsDest = new SysNumVector[Count];
            Array.Fill(SysNumVectorsDest, new SysNumVector(1f, 4f, 9f));

            JohnVectorsSrc = new JohnVector3[Count];
            JohnVectorsDest = new JohnVector3[Count];
            Array.Fill(JohnVectorsSrc, new JohnVector3(1f, 6f, 9f));
        }

        //[GlobalCleanup]
        //public void CleanupAndVerify()
        //{
        //    for (var i = 0; i < Count; i++)
        //    {
        //        // TODO
        //    }
        //}

        //[Benchmark]
        //public JohnVector JohnInline()
        //{
        //    JohnVector vector = Vector128.Create(1f, 2f, 4f, 3434f);
        //    // No software fallback needed, these methods cover it
        //    JohnVector len = Sse.Sqrt(Sse41.DotProduct(vector, vector, 0x7f));
        //    vector = Sse.Divide(vector, len);
        //    vector = Sse.Multiply(vector, vector);
        //    return vector;
        //    //vector = VectorMaths.Normalize3D(vector);
        //    //vector = Arithmetic.Subtract(vector, vector);
        //    //vector = VectorMaths.Normalize3D(vector);
        //    //vector = Arithmetic.Multiply(vector, VectorMaths.DotProduct3D(vector, vector));
        //    //vector = Arithmetic.Multiply(vector, VectorMaths.CrossProduct3D(vector, vector));
        //}

        //[Benchmark]
        //public JohnVector JohnNestedMethod()
        //{
        //    JohnVector vector = Vector128.Create(1f, 2f, 4f, 3434f);
        //    // No software fallback needed, these methods cover it
        //    JohnVector len = Sse.Sqrt(Sse41.DotProduct(vector, vector, 0x7f));
        //    vector = Sse.Divide(vector, len);
        //    vector = Sse.Multiply(vector, vector);
        //    return vector;
        //    //vector = VectorMaths.Normalize3D(vector);
        //    //vector = Arithmetic.Subtract(vector, vector);
        //    //vector = VectorMaths.Normalize3D(vector);
        //    //vector = Arithmetic.Multiply(vector, VectorMaths.DotProduct3D(vector, vector));
        //    //vector = Arithmetic.Multiply(vector, VectorMaths.CrossProduct3D(vector, vector));
        //}

        //[Benchmark]
        //public JohnVector JohnInlineMethod()
        //{
        //    JohnVector vector = Vector128.Create(1f, 2f, 4f, 3434f);
        //    // No software fallback needed, these methods cover it
        //    return VectorMaths.Normalize3D_NewTest(vector);
        //    //vector = VectorMaths.Normalize3D(vector);
        //    //vector = Arithmetic.Subtract(vector, vector);
        //    //vector = VectorMaths.Normalize3D(vector);
        //    //vector = Arithmetic.Multiply(vector, VectorMaths.DotProduct3D(vector, vector));
        //    //vector = Arithmetic.Multiply(vector, VectorMaths.CrossProduct3D(vector, vector));
        //}

        public struct FourFloatBlock
        {
            public float _1;
            public float _2;
            public float _3;
            public float _4;
        }

        public struct ThreeFloatBlock
        {
            public float _1;
            public float _2;
            public float _3;
            public float _4;
        }

        public static FourFloatBlock _value4 = new FourFloatBlock {_1 = 1, _2 = 2, _3 = 3, _4 = 0};
        public static ThreeFloatBlock _value3 = new ThreeFloatBlock { _1 = 1, _2 = 2, _3 = 3};

        [Benchmark]
        public JohnVector John()
        {
            JohnVector vector = Unsafe.As<FourFloatBlock, JohnVector>(ref _value4);
            vector = Arithmetic.Multiply(vector, vector);
            return vector;
        }

        [Benchmark]
        public OpenTKVector OpenTk()
        {
            var vector = Unsafe.As<FourFloatBlock, OpenTKVector>(ref _value4);
            vector = OpenTKVector.Multiply(vector, vector);
            return vector;
            //vector = OpenTKVector.Normalize(vector);
            //vector = OpenTKVector.Subtract(vector, vector);
            //vector = OpenTKVector.Normalize(vector);
            //vector = OpenTKVector.Multiply(vector, OpenTKVector.Dot(vector, vector));
            //vector = OpenTKVector.Multiply(vector, OpenTKVector.Cross(vector, vector));
        }

        [Benchmark]
        public SysNumVector SysNum()
        {
            var vector = Unsafe.As<FourFloatBlock, SysNumVector>(ref _value4);
            vector = SysNumVector.Multiply(vector, vector);
            return vector;
            //vector = SysNumVector.Normalize(vector);
            //vector = SysNumVector.Subtract(vector, vector);
            //vector = SysNumVector.Normalize(vector);
            //vector = SysNumVector.Multiply(vector, SysNumVector.Dot(vector, vector));
            //vector = SysNumVector.Multiply(vector, SysNumVector.Cross(vector, vector));
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
                // vector = VectorMaths.Normalize3D(vector);
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
