using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using MathSharp.Interactive.Benchmarks.Vector.Single;
using MathSharp.StorageTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MathSharp.Interactive
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(new Vector2F(1f, 2f));
        }

        public static bool Equal(Vector2F left, Vector2F right) => left.Equals(right);
    }
}