using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Xml;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using MathSharp.Utils;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(IntrinsicSupport.SupportSummary);
        }

        private static readonly Vector3 Direction = new Vector3(1, 2, 3);
        private static readonly Vector3 Offset = new Vector3(10, 20, 30);

        public static HwVector3 Example()
        {
            var dir = Direction.Load();
            var offset = Offset.Load();

            return dir * offset;
        }
    }
}