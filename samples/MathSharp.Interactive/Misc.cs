using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Running;
using MathSharp.Utils;
using static MathSharp.Vector;
using OpenTK;
using OpenTK.Platform.Windows;

namespace MathSharp.Interactive
{
    using JohnVector = Vector128<float>;
    using JohnVector3 = System.Numerics.Vector4;
    using OpenTKVector = OpenTK.Vector4;
    using OpenTKVector3 = OpenTK.Vector4;
    using SysNumVector = System.Numerics.Vector4;
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    internal class Helpers
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static byte Shuffle(byte a, byte b, byte c, byte d)
        //{
        //    return (byte)(d
        //                  | (c << 2)
        //                  | (b << 4)
        //                  | (a << 6));
        //}
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

            return mask == unchecked((int)0b_1111_1111_0000_0000_0000_0000_0000_0000);
        }
    }

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    [Orderer]
    public class DotProductV256Benchmark
    {
        private Vector256<double> _a, _b;
        private static readonly Vector256<double> MaskWDouble = Vector256.Create(-1, -1, -1, 0).AsDouble();


        [GlobalSetup]
        public void Setup()
        {
            _a = Vector256.Create(1f, 2f, 3f, 4f);
            _a = Vector256.Create(1000f, 2000f, 3000f, 4000f);
        }

        [Benchmark]
        public Vector256<double> Hadd_Shuffle_Hadd()
        {
            return DoubleHadd(_a, _b);
        }

        [Benchmark]
        public Vector256<double> Hadd_Permute2x128_Add()
        {
            return Permute(_a, _b);
        }

        [Benchmark]
        public Vector256<double> Hadd_Permute4x64_HAdd()
        {
            return DoubleHaddPermute(_a, _b);
        }

        public Vector256<double> Permute(Vector256<double> left, Vector256<double> right)
        {
            Vector256<double> mul = Avx.Multiply(left, right);

            // Set W to zero
            Vector256<double> result = Avx.And(mul, MaskWDouble);

            // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
            result = Avx.HorizontalAdd(result, result);

            // Now we have (X + Y, X + Y, Z + 0, Z + 0)
            result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
            // We switch the 2 halves, and add that to the original, getting the result in all elems

            // Set W to zero
            result = Avx.And(result, MaskWDouble);

            return result;
        }

        public Vector256<double> DoubleHadd(Vector256<double> left, Vector256<double> right)
        {
            Vector256<double> mul = Avx.Multiply(left, right);

            // Set W to zero
            Vector256<double> result = Avx.And(mul, MaskWDouble);

            // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
            result = Avx.HorizontalAdd(result, result);

            // Now we have (X + Y, X + Y, Z + 0, Z + 0)
            result = Avx.Shuffle(result, result, ShuffleValues._3_1_2_0);

            result = Avx.HorizontalAdd(result, result);
            // We switch the 2 halves, and add that to the original, getting the result in all elems

            // Set W to zero
            result = Avx.And(result, MaskWDouble);

            return result;
        }

        public Vector256<double> DoubleHaddPermute(Vector256<double> left, Vector256<double> right)
        {
            Vector256<double> mul = Avx.Multiply(left, right);

            // Set W to zero
            Vector256<double> result = Avx.And(mul, MaskWDouble);

            // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
            result = Avx.HorizontalAdd(result, result);

            // Now we have (X + Y, X + Y, Z + 0, Z + 0)
            result = Avx2.Permute4x64(result, ShuffleValues._3_1_2_0);

            result = Avx.HorizontalAdd(result, result);
            // We switch the 2 halves, and add that to the original, getting the result in all elems

            // Set W to zero
            result = Avx.And(result, MaskWDouble);

            return result;
        }
    }

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    [Orderer]
    public class PermuteVsDuplicateBenchmark
    {
        private Vector128<double> _vector;

        [Benchmark]
        public Vector256<double> Duplicate()
        {
            return Vector256.Create(_vector, _vector);
        }

        [Benchmark]
        public Vector256<double> Permute()
        {
            return Vector256.Create(_vector.ToScalar());
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
                HwVector4S vector = JohnVectorsSrc![i].Load();

                vector = Multiply(vector, vector);
                vector = Normalize(vector);
                vector = Subtract(vector, vector);
                vector = Normalize(vector);
                vector = Multiply(vector, DotProduct(vector, vector));
                vector = DotProduct((HwVector4S)CrossProduct((HwVector3S)vector, (HwVector3S)vector), Abs(vector));
                for (var j = 0; j < Count / 2; j++)
                {
                    vector = Add(vector, vector);
                    vector = Subtract(vector, vector);
                }

                //vector.Store(out JohnVectorsDest![i]);
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
                for (var j = 0; j < Count / 2; j++)
                {
                    vector = OpenTKVector.Add(vector, vector);
                    vector = OpenTKVector.Subtract(vector, vector);
                }

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
                for (var j = 0; j < Count / 2; j++)
                {
                    vector = SysNumVector.Add(vector, vector);
                    vector = SysNumVector.Subtract(vector, vector);
                }

                SysNumVectorsDest![i] = vector;
            }
        }
    }

    [CoreJob]
    [RPlotExporter]
    [RankColumn]
    public unsafe class VectorisationBenchmark
    {
        private const int Count = 1024;

        private readonly OpenTKVector[] _openTkSrc = new OpenTKVector[Count / sizeof(OpenTKVector)];
        private readonly OpenTKVector[] _openTkDest = new OpenTKVector[Count / sizeof(OpenTKVector)];

        private readonly Vector256<float>[] _mathSharpSrc = new Vector256<float>[Count / sizeof(Vector256<float>)];
        private readonly Vector256<float>[] _mathSharpDest = new Vector256<float>[Count / sizeof(Vector256<float>)];


        [GlobalSetup]
        public void Setup()
        {
            Span<float> openTkAsFloat = MemoryMarshal.Cast<OpenTKVector, float>(_openTkSrc);
            Span<float> mathSharpAsFloat = MemoryMarshal.Cast<Vector256<float>, float>(_mathSharpSrc);

            Trace.Assert(openTkAsFloat.Length == mathSharpAsFloat.Length);


            float f = float.MinValue;
            for (var i = 0; i < openTkAsFloat.Length; i++)
            {
                openTkAsFloat[i] = f;
                mathSharpAsFloat[i] = f;

                f = f > (float.MaxValue - (float.MaxValue - 1024)) ? float.MinValue : f + 1;
            }
        }

        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
        public void OpenTk()
        {
            for (var i = 0; i < Count / sizeof(OpenTKVector); i++)
            {
                _openTkDest[i] = _openTkSrc[i] * _openTkSrc[i] * _openTkSrc[i] * _openTkSrc[i];
            }
        }

        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
        public void MathSharp()
        {
            for (var i = 0; i < Count / sizeof(Vector256<float>) / 4; i++)
            {
                _mathSharpDest[i + 0] = Avx.Multiply(Avx.Multiply(Avx.Multiply(_mathSharpSrc[i + 0], _mathSharpSrc[i + 0]), _mathSharpSrc[i + 0]), _mathSharpSrc[i + 0]);
                _mathSharpDest[i + 1] = Avx.Multiply(Avx.Multiply(Avx.Multiply(_mathSharpSrc[i + 1], _mathSharpSrc[i + 1]), _mathSharpSrc[i + 1]), _mathSharpSrc[i + 1]);
                _mathSharpDest[i + 2] = Avx.Multiply(Avx.Multiply(Avx.Multiply(_mathSharpSrc[i + 2], _mathSharpSrc[i + 2]), _mathSharpSrc[i + 2]), _mathSharpSrc[i + 2]);
                _mathSharpDest[i + 3] = Avx.Multiply(Avx.Multiply(Avx.Multiply(_mathSharpSrc[i + 3], _mathSharpSrc[i + 3]), _mathSharpSrc[i + 3]), _mathSharpSrc[i + 3]);
            }
        }
    }
}