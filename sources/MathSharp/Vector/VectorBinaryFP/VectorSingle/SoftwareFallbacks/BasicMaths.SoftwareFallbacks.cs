using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class SoftwareFallbacks
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector4F Abs_Software(Vector4FParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Abs(X(vector)),
                MathF.Abs(Y(vector)),
                MathF.Abs(Z(vector)),
                MathF.Abs(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F HorizontalAdd_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Add_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Add_Software(Vector4FParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                X(vector) + scalar,
                Y(vector) + scalar,
                Z(vector) + scalar,
                W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Subtract_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Subtract_Software(Vector4FParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                X(vector) - scalar,
                Y(vector) - scalar,
                Z(vector) - scalar,
                W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Multiply_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Multiply_Software(Vector4FParam1_3 left, float scalar)
        {
            return Vector128.Create(
                X(left) * scalar,
                Y(left) * scalar,
                Z(left) * scalar,
                W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Divide_Software(Vector4FParam1_3 dividend, Vector4FParam1_3 divisor)
        {
            return Vector128.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Divide_Software(Vector4FParam1_3 dividend, float scalarDivisor)
        {
            return Vector128.Create(
                X(dividend) / scalarDivisor,
                Y(dividend) / scalarDivisor,
                Z(dividend) / scalarDivisor,
                W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Clamp_Software(Vector4FParam1_3 vector, Vector4FParam1_3 low, Vector4FParam1_3 high)
        {
            return Vector128.Create(
                Math.Clamp(X(vector), X(low), X(high)),
                Math.Clamp(Y(vector), Y(low), Y(high)),
                Math.Clamp(Z(vector), Z(low), Z(high)),
                Math.Clamp(W(vector), W(low), W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Sqrt_Software(Vector4FParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Sqrt(X(vector)),
                MathF.Sqrt(Y(vector)),
                MathF.Sqrt(Z(vector)),
                MathF.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Max_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                MathF.Max(X(left), X(right)),
                MathF.Max(Y(left), Y(right)),
                MathF.Max(Z(left), Z(right)),
                MathF.Max(W(left), W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Min_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                MathF.Min(X(left), X(right)),
                MathF.Min(Y(left), Y(right)),
                MathF.Min(Z(left), Z(right)),
                MathF.Min(W(left), W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Negate2D_Software(Vector4FParam1_3 vector)
        {
            return Vector128.Create(
                -X(vector),
                -Y(vector),
                +Z(vector),
                +W(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Negate3D_Software(Vector4FParam1_3 vector)
        {
            return Vector128.Create(
                -X(vector),
                -Y(vector),
                -Z(vector),
                +W(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Negate4D_Software(Vector4FParam1_3 vector)
        {
            return Vector128.Create(
                -X(vector),
                -Y(vector),
                -Z(vector),
                -W(vector)
            );
        }

        #endregion
    }
}
