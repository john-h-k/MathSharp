using System;
using System.Runtime.Intrinsics;
using MathSharp.StorageTypes;

namespace MathSharp.Interactive
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine(Vector.ToString(Vector128.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16), elemCount: 15));
        }

        public static bool Equal(Vector2F left, Vector2F right) => left.Equals(right);
    }
}