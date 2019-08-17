using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4UInt64 = Vector256<ulong>;
    using Vector4UInt64Param1_3 = Vector256<ulong>;

    public static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 HorizontalAdd_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Add_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Add_Software(Vector4UInt64Param1_3 vector, ulong scalar)
        {
            return Vector256.Create(
                X(vector) + scalar,
                Y(vector) + scalar,
                Z(vector) + scalar,
                W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Subtract_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Subtract_Software(Vector4UInt64Param1_3 vector, ulong scalar)
        {
            return Vector256.Create(
                X(vector) - scalar,
                Y(vector) - scalar,
                Z(vector) - scalar,
                W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Multiply_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Multiply_Software(Vector4UInt64Param1_3 left, ulong scalar)
        {
            return Vector256.Create(
                X(left) * scalar,
                Y(left) * scalar,
                Z(left) * scalar,
                W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Divide_Software(Vector4UInt64Param1_3 dividend, Vector4UInt64Param1_3 divisor)
        {
            return Vector256.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Divide_Software(Vector4UInt64Param1_3 dividend, ulong scalarDivisor)
        {
            return Vector256.Create(
                X(dividend) / scalarDivisor,
                Y(dividend) / scalarDivisor,
                Z(dividend) / scalarDivisor,
                W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Clamp_Software(Vector4UInt64Param1_3 vector, Vector4UInt64Param1_3 low, Vector4UInt64Param1_3 high)
        {
            return Vector256.Create(
                Math.Clamp(X(vector), X(low), X(high)),
                Math.Clamp(Y(vector), Y(low), Y(high)),
                Math.Clamp(Z(vector), Z(low), Z(high)),
                Math.Clamp(W(vector), W(low), W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Sqrt_Software(Vector4UInt64Param1_3 vector)
        {
            return Vector256.Create(
                (ulong)MathF.Sqrt(X(vector)),
                (ulong)MathF.Sqrt(Y(vector)),
                (ulong)MathF.Sqrt(Z(vector)),
                (ulong)MathF.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Max_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                Math.Max(X(left), X(right)),
                Math.Max(Y(left), Y(right)),
                Math.Max(Z(left), Z(right)),
                Math.Max(W(left), W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Min_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                Math.Min(X(left), X(right)),
                Math.Min(Y(left), Y(right)),
                Math.Min(Z(left), Z(right)),
                Math.Min(W(left), W(right))
            );
        }
    }
}
