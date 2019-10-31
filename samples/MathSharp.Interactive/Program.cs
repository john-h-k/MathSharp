using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using MathSharp.StorageTypes;

namespace MathSharp.Interactive
{
    internal class Program
    {
        public static void Main()
        {
            Rand();
            Rand();
            Rand();
            Rand();
            Rand();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Rand()
        {
            Console.WriteLine(string.Join(", ", Rng().ToArray().Select(x => x.ToString())));
        }

        public static unsafe Span<int> Rng()
        {
            var scalar = Vector256.CreateScalarUnsafe(0);

            return new Span<int>(&scalar + 1, Vector256<int>.Count - 1);
        }
    }
}