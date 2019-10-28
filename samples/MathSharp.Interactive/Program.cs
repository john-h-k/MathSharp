using System;
using System.Runtime.Intrinsics;
using MathSharp.StorageTypes;

namespace MathSharp.Interactive
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine(Vector.SupportSummary);
        }

        public static bool Equal(Vector2F left, Vector2F right) => left.Equals(right);
    }
}