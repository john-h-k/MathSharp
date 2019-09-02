using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4Int32 = Vector128<int>;
    using Vector4Int32Param1_3 = Vector128<int>;

    public static partial class SoftwareFallbacks
    {
        #region Vector

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Abs_Software(in Vector4Int32Param1_3 vector)
        {
            return Vector128.Create(
                Math.Abs(X(vector)),
                Math.Abs(Y(vector)),
                Math.Abs(Z(vector)),
                Math.Abs(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 HorizontalAdd_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Add_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Add_Software(in Vector4Int32Param1_3 vector, int scalar)
        {
            return Vector128.Create(
                X(vector) + scalar,
                Y(vector) + scalar,
                Z(vector) + scalar,
                W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Subtract_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Subtract_Software(in Vector4Int32Param1_3 vector, int scalar)
        {
            return Vector128.Create(
                X(vector) - scalar,
                Y(vector) - scalar,
                Z(vector) - scalar,
                W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Multiply_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Multiply_Software(in Vector4Int32Param1_3 left, int scalar)
        {
            return Vector128.Create(
                X(left) * scalar,
                Y(left) * scalar,
                Z(left) * scalar,
                W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Divide_Software(in Vector4Int32Param1_3 dividend, in Vector4Int32Param1_3 divisor)
        {
            return Vector128.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Divide_Software(in Vector4Int32Param1_3 dividend, int scalarDivisor)
        {
            return Vector128.Create(
                X(dividend) / scalarDivisor,
                Y(dividend) / scalarDivisor,
                Z(dividend) / scalarDivisor,
                W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Clamp_Software(in Vector4Int32Param1_3 vector, in Vector4Int32Param1_3 low, in Vector4Int32Param1_3 high)
        {
            return Vector128.Create(
                Math.Clamp(X(vector), X(low), X(high)),
                Math.Clamp(Y(vector), Y(low), Y(high)),
                Math.Clamp(Z(vector), Z(low), Z(high)),
                Math.Clamp(W(vector), W(low), W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Sqrt_Software(in Vector4Int32Param1_3 vector)
        {
            return Vector128.Create(
                (int)MathF.Sqrt(X(vector)),
                (int)MathF.Sqrt(Y(vector)),
                (int)MathF.Sqrt(Z(vector)),
                (int)MathF.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Max_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Math.Max(X(left), X(right)),
                Math.Max(Y(left), Y(right)),
                Math.Max(Z(left), Z(right)),
                Math.Max(W(left), W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Min_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Math.Min(X(left), X(right)),
                Math.Min(Y(left), Y(right)),
                Math.Min(Z(left), Z(right)),
                Math.Min(W(left), W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Negate2D_Software(in Vector4Int32Param1_3 vector)
        {
            return Vector128.Create(
                -X(vector),
                -Y(vector),
                +Z(vector),
                +W(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Negate3D_Software(in Vector4Int32Param1_3 vector)
        {
            return Vector128.Create(
                -X(vector),
                -Y(vector),
                -Z(vector),
                +W(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Negate4D_Software(in Vector4Int32Param1_3 vector)
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
