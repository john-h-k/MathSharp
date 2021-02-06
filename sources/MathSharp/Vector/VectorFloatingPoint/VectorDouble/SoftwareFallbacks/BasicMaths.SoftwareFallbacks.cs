using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    internal static unsafe partial class SoftwareFallbacks
    {
        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Abs_Software(Vector256<double> vector)
        {
            return Vector256.Create(
                Math.Abs(X(vector)),
                Math.Abs(Y(vector)),
                Math.Abs(Z(vector)),
                Math.Abs(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> HorizontalAdd_Software(Vector256<double> left, Vector256<double> right)
        {
            return Vector256.Create(
                X(left) + Y(left),
                X(right) + Y(right),
                Z(left) + W(left),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<double> Add_Software(Vector128<double> left, Vector128<double> right)
        {
            return Vector128.Create(
                X(left) + X(right),
                Y(left) + Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Add_Software(Vector256<double> left, Vector256<double> right)
        {
            return Vector256.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<int> Add_Software(Vector128<int> left, Vector128<int> right)
        {
            return Vector128.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Add_Software(Vector256<double> vector, double scalar)
        {
            return Vector256.Create(
                X(vector) + scalar,
                Y(vector) + scalar,
                Z(vector) + scalar,
                W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Subtract_Software(Vector256<double> left, Vector256<double> right)
        {
            return Vector256.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<int> Subtract_Software(Vector128<int> left, Vector128<int> right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<double> Subtract_Software(Vector128<double> left, Vector128<double> right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Subtract_Software(Vector256<double> vector, double scalar)
        {
            return Vector256.Create(
                X(vector) - scalar,
                Y(vector) - scalar,
                Z(vector) - scalar,
                W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<double> Multiply_Software(Vector128<double> left, Vector128<double> right)
        {
            return Vector128.Create(
                X(left) * X(right),
                Y(left) * Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Multiply_Software(Vector256<double> left, Vector256<double> right)
        {
            return Vector256.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Multiply_Software(Vector256<double> left, double scalar)
        {
            return Vector256.Create(
                X(left) * scalar,
                Y(left) * scalar,
                Z(left) * scalar,
                W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Divide_Software(Vector256<double> dividend, Vector256<double> divisor)
        {
            return Vector256.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<double> Divide_Software(Vector128<double> dividend, Vector128<double> divisor)
        {
            return Vector128.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Divide_Software(Vector256<double> dividend, double scalarDivisor)
        {
            return Vector256.Create(
                X(dividend) / scalarDivisor,
                Y(dividend) / scalarDivisor,
                Z(dividend) / scalarDivisor,
                W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Clamp_Software(Vector256<double> vector, Vector256<double> low, Vector256<double> high)
        {
            return Vector256.Create(
                Math.Clamp(X(vector), X(low), X(high)),
                Math.Clamp(Y(vector), Y(low), Y(high)),
                Math.Clamp(Z(vector), Z(low), Z(high)),
                Math.Clamp(W(vector), W(low), W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Sqrt_Software(Vector256<double> vector)
        {
            return Vector256.Create(
                Math.Sqrt(X(vector)),
                Math.Sqrt(Y(vector)),
                Math.Sqrt(Z(vector)),
                Math.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Max_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            if (double.IsNaN(lX)) lX = rX;
            if (double.IsNaN(lY)) lY = rY;
            if (double.IsNaN(lZ)) lZ = rZ;
            if (double.IsNaN(lW)) lW = rW;

            if (double.IsNaN(rX)) rX = lX;
            if (double.IsNaN(rY)) rY = lY;
            if (double.IsNaN(rZ)) rZ = lZ;
            if (double.IsNaN(rW)) rW = lW;

            return Vector256.Create(
                Math.Max(lX, rX),
                Math.Max(lY, rY),
                Math.Max(lZ, rZ),
                Math.Max(lW, rW)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Min_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            if (double.IsNaN(lX)) lX = rX;
            if (double.IsNaN(lY)) lY = rY;
            if (double.IsNaN(lZ)) lZ = rZ;
            if (double.IsNaN(lW)) lW = rW;

            if (double.IsNaN(rX)) rX = lX;
            if (double.IsNaN(rY)) rY = lY;
            if (double.IsNaN(rZ)) rZ = lZ;
            if (double.IsNaN(rW)) rW = lW;

            return Vector256.Create(
                Math.Min(lX, rX),
                Math.Min(lY, rY),
                Math.Min(lZ, rZ),
                Math.Min(lW, rW)
            );
        }

        #endregion
    }
}
