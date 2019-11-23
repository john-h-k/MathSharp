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
        private static readonly string StaticReadonly = "A";
        private const string Const = "C";

        public static void Main(string[] args)
        {
            Console.WriteLine(StaticReadonly);
            Unsafe.AsRef(StaticReadonly.GetPinnableReference()) = 'B';
            Console.WriteLine(StaticReadonly);

            Console.WriteLine(Const);
            Unsafe.AsRef(Const.GetPinnableReference()) = 'B';
            Console.WriteLine(Const);
        }

        public static ref byte Ref(Span<byte> span) => ref span.GetPinnableReference();
    }
}