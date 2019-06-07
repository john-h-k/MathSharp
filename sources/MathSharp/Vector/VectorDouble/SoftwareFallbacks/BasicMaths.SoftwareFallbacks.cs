using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using VectorDParam1_3 = Vector256<double>;

    public static unsafe partial class SoftwareFallbacks
    {
        #region VectorD

        [MethodImpl(MaxOpt)]
        public static Vector4D Abs_Software(VectorDParam1_3 vector)
        {
            return Vector256.Create(
                Math.Abs(Helpers.X(vector)),
                Math.Abs(Helpers.Y(vector)),
                Math.Abs(Helpers.Z(vector)),
                Math.Abs(Helpers.W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D HorizontalAdd_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                Helpers.X(left) + Helpers.Y(left),
                Helpers.Z(left) + Helpers.W(left),
                Helpers.X(right) + Helpers.Y(right),
                Helpers.Z(right) + Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Add_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                Helpers.X(left) + Helpers.X(right),
                Helpers.Y(left) + Helpers.Y(right),
                Helpers.Z(left) + Helpers.Z(right),
                Helpers.W(left) + Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Add_Software(VectorDParam1_3 vector, float scalar)
        {
            return Vector256.Create(
                Helpers.X(vector) + scalar,
                Helpers.Y(vector) + scalar,
                Helpers.Z(vector) + scalar,
                Helpers.W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                Helpers.X(left) - Helpers.X(right),
                Helpers.Y(left) - Helpers.Y(right),
                Helpers.Z(left) - Helpers.Z(right),
                Helpers.W(left) - Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract_Software(VectorDParam1_3 vector, float scalar)
        {
            return Vector256.Create(
                Helpers.X(vector) - scalar,
                Helpers.Y(vector) - scalar,
                Helpers.Z(vector) - scalar,
                Helpers.W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
            return Vector256.Create(
                Helpers.X(left) * Helpers.X(right),
                Helpers.Y(left) * Helpers.Y(right),
                Helpers.Z(left) * Helpers.Z(right),
                Helpers.W(left) * Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply_Software(VectorDParam1_3 left, float scalar)
        {
            return Vector256.Create(
                Helpers.X(left) * scalar,
                Helpers.Y(left) * scalar,
                Helpers.Z(left) * scalar,
                Helpers.W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide_Software(VectorDParam1_3 dividend, VectorDParam1_3 divisor)
        {
            return Vector256.Create(
                Helpers.X(dividend) / Helpers.X(divisor),
                Helpers.Y(dividend) / Helpers.Y(divisor),
                Helpers.Z(dividend) / Helpers.Z(divisor),
                Helpers.W(dividend) / Helpers.W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide_Software(VectorDParam1_3 dividend, float scalarDivisor)
        {
            return Vector256.Create(
                Helpers.X(dividend) / scalarDivisor,
                Helpers.Y(dividend) / scalarDivisor,
                Helpers.Z(dividend) / scalarDivisor,
                Helpers.W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Clamp_Software(VectorDParam1_3 vector, VectorDParam1_3 low, VectorDParam1_3 high)
        {
            return Vector256.Create(
                Math.Clamp(Helpers.X(vector), Helpers.X(low), Helpers.X(high)),
                Math.Clamp(Helpers.Y(vector), Helpers.Y(low), Helpers.Y(high)),
                Math.Clamp(Helpers.Z(vector), Helpers.Z(low), Helpers.Z(high)),
                Math.Clamp(Helpers.W(vector), Helpers.W(low), Helpers.W(high))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Sqrt_Software(VectorDParam1_3 vector)
        {
            return Vector256.Create(
                Math.Sqrt(Helpers.X(vector)),
                Math.Sqrt(Helpers.Y(vector)),
                Math.Sqrt(Helpers.Z(vector)),
                Math.Sqrt(Helpers.W(vector))
            );
        }

        #endregion
    }
}
