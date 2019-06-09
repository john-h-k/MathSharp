using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MathSharp.Utils
{
    internal static partial class Helpers
    {
        public static readonly double NoBitsSetDouble = 0d;
        public static readonly double AllBitsSetDouble = Unsafe.As<int, double>(ref Unsafe.AsRef(-1));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(Vector4 left, Vector256<double> right)
            => left.X.Equals(X(right)) && left.Y.Equals(Y(right)) && left.Z.Equals(Z(right)) && left.W.Equals(W(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(Vector3 left, Vector256<double> right)
            => left.X.Equals(X(right)) && left.Y.Equals(Y(right)) && left.Z.Equals(Z(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(Vector2 left, Vector256<double> right)
            => left.X.Equals(X(right)) && left.Y.Equals(Y(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(double left, Vector256<double> right)
            => left.Equals(X(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(double left, double right, double interval = 0.000000001d)
        {
            if (double.IsNaN(left) && double.IsNaN(right))
                return true;

            return Math.Abs(left - right) < interval;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(Vector4 left, Vector256<double> right, double interval = 0.000000001d)
        {
            return AreApproxEqual(left.X, X(right), interval)
                   && AreApproxEqual(left.Y, Y(right), interval)
                   && AreApproxEqual(left.Z, Z(right), interval)
                   && AreApproxEqual(left.W, W(right), interval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(Vector3 left, Vector256<double> right, double interval = 0.000000001d)
        {
            return AreApproxEqual(left.X, X(right), interval)
                   && AreApproxEqual(left.Y, Y(right), interval)
                   && AreApproxEqual(left.Z, Z(right), interval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(Vector2 left, Vector256<double> right, double interval = 0.000000001d)
        {
            return AreApproxEqual(left.X, X(right), interval)
                   && AreApproxEqual(left.Y, Y(right), interval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(double left, Vector256<double> right)
        {
            return AreApproxEqual(left, X(right));
        }
    }
}
