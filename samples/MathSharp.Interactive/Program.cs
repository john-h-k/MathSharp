using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.MatrixTests.Single;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using MathSharp.Utils;
using static MathSharp.Vector;

using static MathSharp.Interactive.TrigTest;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine(float.NaN > 1);
            Console.WriteLine(
                SoftwareFallbacks.CompareLessThanOrEqual_Software(Vector128.Create(float.NaN), Vector128.Create(0f)).AsInt32());
            Console.WriteLine(
                Sse.CompareLessThanOrEqual(Vector128.Create(float.NaN), Vector128.Create(0f)).AsInt32());

        }

        public static Vector128<float> Foo()
        {
            var v = default(Vector128<float>);
            return Multiply(v, v);
        }
    }
}