using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.StorageTypes;
using MathSharp;

namespace MathSharp.Interactive
{
    internal unsafe class Program
    {
        public static void Main(string[] args)
        {
        }

        public static MatrixSingle Add(MatrixSingle* left, MatrixSingle* right) => Matrix.Add(left, right);
    }
}