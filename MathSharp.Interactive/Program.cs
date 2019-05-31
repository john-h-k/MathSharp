using System;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(IsAligned());
        }

        public static unsafe bool IsAligned()
        {
            byte x;
            byte y;
            MatrixF matrix;

            x = 11;
            y = 11;

            return ((ulong)&matrix) % 16 == 0 && (ulong)&x > (ulong)&matrix && Math.Abs(&x - &y) == 1;
        }
    }
}
