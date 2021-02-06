using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

using MVector = MathSharp.Vector;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    public class SelectionBenchmark
    {
        private Vector4 _value = new Vector4(99, -5, 6, -2.3f);
        private Vector4 _multiply = new Vector4(99, 7, 0.1f, -2.3f);


        private Vector128<float> _value1 = Vector128.Create(99, -5, 6, -2.3f);
        private Vector128<float> _multiply1 = Vector128.Create(99, 7, 0.1f, -2.3f);

        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public Vector4 SysNumeric() => Foo(_value, _multiply);

        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public Vector128<float> MathSharp() => Foo(_value1, _multiply1);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public Vector4 Foo(Vector4 left, Vector4 right)
        {
            var mul = left * right;
            var sqrt = Vector4.SquareRoot(mul);
            // No way to vectorise this :(
            var x = left.X == right.X ? left.X * right.X : sqrt.X;
            var y = left.Y == right.Y ? left.Y * right.Y : sqrt.Y;
            var z = left.Z == right.Z ? left.Z * right.Z : sqrt.Z;
            var w = left.W == right.W ? left.W * right.W : sqrt.W;
            return new Vector4(x, y, z, w);
        }

        //[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        //public static Vector128<float> MulNegAndAbs_MathSharp(Vector128<float> value, Vector128<float> multiply)
        //{
        //    var mul = Sse.Multiply(Sse.Xor(value, Vector128.Create(int.MinValue).AsSingle()), multiply);
        //    mul = Sse.Max(mul, value);
        //    return Sse.And(mul, Vector128.Create(~int.MinValue).AsSingle());
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public Vector128<float> Foo(Vector128<float> left, Vector128<float> right)
        {
            var comp = MVector.CompareEqual(left, right);
            var mul = MVector.Multiply(left, right);
            var sqrt = MVector.Sqrt(mul);

            return MVector.Select(comp, mul, sqrt);
        }
    }
}
