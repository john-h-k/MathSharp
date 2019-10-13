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
        }

        public static Vector128<float> Foo()
        {
            var v = default(Vector128<float>);
            return Multiply(v, v);
        }
    }
}