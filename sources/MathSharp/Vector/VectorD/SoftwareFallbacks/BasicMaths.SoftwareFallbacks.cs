using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Helpers;

namespace MathSharp.SoftwareFallbacks
{
    using Vector4D = Vector256<double>;
    using VectorDParam1_3 = Vector256<double>;

    internal static unsafe partial class SoftwareFallbacksVector4D
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region VectorD

        [MethodImpl(MaxOpt)]
        public static Vector4D Abs_Software(VectorDParam1_3 vector)
        {
            return Vector256.Create(
                Math.Abs(X(vector)),
                Math.Abs(Y(vector)),
                Math.Abs(Z(vector)),
                Math.Abs(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D HorizontalAdd_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Add_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Add_Software(VectorDParam1_3 vector, float scalar)
        {
            return Vector256.Create(
                X(vector) + scalar,
                Y(vector) + scalar,
                Z(vector) + scalar,
                W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract_Software(VectorDParam1_3 vector, float scalar)
        {
            return Vector256.Create(
                X(vector) - scalar,
                Y(vector) - scalar,
                Z(vector) - scalar,
                W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply_Software(VectorDParam1_3 left, float scalar)
        {
            return Vector256.Create(
                X(left) * scalar,
                Y(left) * scalar,
                Z(left) * scalar,
                W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide_Software(VectorDParam1_3 dividend, VectorDParam1_3 divisor)
        {
            return Vector256.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide_Software(VectorDParam1_3 dividend, float scalarDivisor)
        {
            return Vector256.Create(
                X(dividend) / scalarDivisor,
                Y(dividend) / scalarDivisor,
                Z(dividend) / scalarDivisor,
                W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Clamp_Software(VectorDParam1_3 vector, VectorDParam1_3 low, VectorDParam1_3 high)
        {
            return Vector256.Create(
                Math.Clamp(X(vector), X(low), X(high)),
                Math.Clamp(Y(vector), Y(low), Y(high)),
                Math.Clamp(Z(vector), Z(low), Z(high)),
                Math.Clamp(W(vector), W(low), W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Sqrt_Software(VectorDParam1_3 vector)
        {
            return Vector256.Create(
                Math.Sqrt(X(vector)),
                Math.Sqrt(Y(vector)),
                Math.Sqrt(Z(vector)),
                Math.Sqrt(W(vector))
            );
        }

        #endregion
    }
}
