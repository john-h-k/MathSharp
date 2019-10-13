using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    internal static partial class SoftwareFallbacks
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector128<float> HorizontalAdd_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide_Software(Vector4FParam1_3 dividend, Vector4FParam1_3 divisor)
        {
            return Vector128.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sqrt_Software(Vector4FParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Sqrt(X(vector)),
                MathF.Sqrt(Y(vector)),
                MathF.Sqrt(Z(vector)),
                MathF.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Max_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
        public static Vector128<float> Min_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
