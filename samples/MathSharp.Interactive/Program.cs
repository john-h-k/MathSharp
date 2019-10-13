using System;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.MatrixTests.Single;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using static MathSharp.Vector;

using static MathSharp.Interactive.TrigTest;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main()
        {
            Test(0, doATan: true, doATan2: true);
            Test(30, doATan: true, doATan2: true);
            Test(60, doATan: true, doATan2: true);
            Test(100f, doATan: true, doATan2: true);
        }

        public static Vector128<float> Foo()
        {
            var v = default(Vector128<float>);
            return Multiply(v, v);
        }
    }
}