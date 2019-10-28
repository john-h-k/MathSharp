using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    internal static partial class SoftwareFallbacks
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector128<float> HorizontalAdd_Software(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add_Software(Vector128<float> left, Vector128<float> right)
        {
            //return Vector128.Create(
            //    X(left) + X(right),
            //    Y(left) + Y(right),
            //    Z(left) + Z(right),
            //    W(left) + W(right)
            //);

            Vector128<float> result = default;

            for (var i = 0; i < Vector128<float>.Count; i++)
            {
                result = result.WithElement(i, left.GetElement(i) + right.GetElement(i));
            }

            return result;
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract_Software(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply_Software(Vector128<float> left, Vector128<float> right)
        {
            return Vector128.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide_Software(Vector128<float> dividend, Vector128<float> divisor)
        {
            return Vector128.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sqrt_Software(Vector128<float> vector)
        {
            return Vector128.Create(
                MathF.Sqrt(X(vector)),
                MathF.Sqrt(Y(vector)),
                MathF.Sqrt(Z(vector)),
                MathF.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Max_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            if (float.IsNaN(lX)) lX = rX;
            if (float.IsNaN(lY)) lY = rY;
            if (float.IsNaN(lZ)) lZ = rZ;
            if (float.IsNaN(lW)) lW = rW;

            if (float.IsNaN(rX)) rX = lX;
            if (float.IsNaN(rY)) rY = lY;
            if (float.IsNaN(rZ)) rZ = lZ;
            if (float.IsNaN(rW)) rW = lW;

            return Vector128.Create(
                MathF.Max(lX, rX),
                MathF.Max(lY, rY),
                MathF.Max(lZ, rZ),
                MathF.Max(lW, rW)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Min_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            if (float.IsNaN(lX)) lX = rX;
            if (float.IsNaN(lY)) lY = rY;
            if (float.IsNaN(lZ)) lZ = rZ;
            if (float.IsNaN(lW)) lW = rW;

            if (float.IsNaN(rX)) rX = lX;
            if (float.IsNaN(rY)) rY = lY;
            if (float.IsNaN(rZ)) rZ = lZ;
            if (float.IsNaN(rW)) rW = lW;

            return Vector128.Create(
                MathF.Min(lX, rX),
                MathF.Min(lY, rY),
                MathF.Min(lZ, rZ),
                MathF.Min(lW, rW)
            );
        }

        #endregion
    }
}
