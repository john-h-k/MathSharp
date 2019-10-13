using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    internal static unsafe partial class SoftwareFallbacks
    {
        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector4D Abs_Software(in Vector4DParam1_3 vector)
        {
            return Vector256.Create(
                Math.Abs(X(vector)),
                Math.Abs(Y(vector)),
                Math.Abs(Z(vector)),
                Math.Abs(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D HorizontalAdd_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                X(left) + Y(left),
                X(right) + Y(right),
                Z(left) + W(left),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Add_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Add_Software(in Vector4DParam1_3 vector, double scalar)
        {
            return Vector256.Create(
                X(vector) + scalar,
                Y(vector) + scalar,
                Z(vector) + scalar,
                W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract_Software(in Vector4DParam1_3 vector, double scalar)
        {
            return Vector256.Create(
                X(vector) - scalar,
                Y(vector) - scalar,
                Z(vector) - scalar,
                W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply_Software(in Vector4DParam1_3 left, double scalar)
        {
            return Vector256.Create(
                X(left) * scalar,
                Y(left) * scalar,
                Z(left) * scalar,
                W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide_Software(in Vector4DParam1_3 dividend, in Vector4DParam1_3 divisor)
        {
            return Vector256.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide_Software(in Vector4DParam1_3 dividend, double scalarDivisor)
        {
            return Vector256.Create(
                X(dividend) / scalarDivisor,
                Y(dividend) / scalarDivisor,
                Z(dividend) / scalarDivisor,
                W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Clamp_Software(in Vector4DParam1_3 vector, in Vector4DParam1_3 low, in Vector4DParam1_3 high)
        {
            return Vector256.Create(
                Math.Clamp(X(vector), X(low), X(high)),
                Math.Clamp(Y(vector), Y(low), Y(high)),
                Math.Clamp(Z(vector), Z(low), Z(high)),
                Math.Clamp(W(vector), W(low), W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Sqrt_Software(in Vector4DParam1_3 vector)
        {
            return Vector256.Create(
                Math.Sqrt(X(vector)),
                Math.Sqrt(Y(vector)),
                Math.Sqrt(Z(vector)),
                Math.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Max_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
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
        public static Vector4D Min_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
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

        [MethodImpl(MaxOpt)]
        public static Vector4D Negate2D_Software(in Vector4DParam1_3 vector)
        {
            return Vector256.Create(
                -X(vector),
                -Y(vector),
                +Z(vector),
                +W(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Negate3D_Software(in Vector4DParam1_3 vector)
        {
            return Vector256.Create(
                -X(vector),
                -Y(vector),
                -Z(vector),
                +W(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Negate4D_Software(in Vector4DParam1_3 vector)
        {
            return Vector256.Create(
                -X(vector),
                -Y(vector),
                -Z(vector),
                -W(vector)
            );
        }

        #endregion
    }
}
