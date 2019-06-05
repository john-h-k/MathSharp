using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Helpers;

namespace MathSharp.SoftwareFallbacks
{
    using VectorFParam1_3 = Vector128<float>;

    internal static unsafe partial class SoftwareFallbacks
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region VectorF

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Abs_Software(VectorFParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Abs(X(vector)),
                MathF.Abs(Y(vector)),
                MathF.Abs(Z(vector)),
                MathF.Abs(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> HorizontalAdd_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add_Software(VectorFParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                X(vector) + scalar,
                Y(vector) + scalar,
                Z(vector) + scalar,
                W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract_Software(VectorFParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                X(vector) - scalar,
                Y(vector) - scalar,
                Z(vector) - scalar,
                W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply_Software(VectorFParam1_3 left, float scalar)
        {
            return Vector128.Create(
                X(left) * scalar,
                Y(left) * scalar,
                Z(left) * scalar,
                W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide_Software(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
        {
            return Vector128.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide_Software(VectorFParam1_3 dividend, float scalarDivisor)
        {
            return Vector128.Create(
                X(dividend) / scalarDivisor,
                Y(dividend) / scalarDivisor,
                Z(dividend) / scalarDivisor,
                W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Clamp_Software(VectorFParam1_3 vector, VectorFParam1_3 low, VectorFParam1_3 high)
        {
            return Vector128.Create(
                Math.Clamp(X(vector), X(low), X(high)),
                Math.Clamp(Y(vector), Y(low), Y(high)),
                Math.Clamp(Z(vector), Z(low), Z(high)),
                Math.Clamp(W(vector), W(low), W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sqrt_Software(VectorFParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Sqrt(X(vector)),
                MathF.Sqrt(Y(vector)),
                MathF.Sqrt(Z(vector)),
                MathF.Sqrt(W(vector))
            );
        }

        #endregion
    }
}
