using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

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
                MathF.Abs(Helpers.X(vector)),
                MathF.Abs(Helpers.Y(vector)),
                MathF.Abs(Helpers.Z(vector)),
                MathF.Abs(Helpers.W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> HorizontalAdd_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) + Helpers.Y(left),
                Helpers.Z(left) + Helpers.W(left),
                Helpers.X(right) + Helpers.Y(right),
                Helpers.Z(right) + Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) + Helpers.X(right),
                Helpers.Y(left) + Helpers.Y(right),
                Helpers.Z(left) + Helpers.Z(right),
                Helpers.W(left) + Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Add_Software(VectorFParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                Helpers.X(vector) + scalar,
                Helpers.Y(vector) + scalar,
                Helpers.Z(vector) + scalar,
                Helpers.W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) - Helpers.X(right),
                Helpers.Y(left) - Helpers.Y(right),
                Helpers.Z(left) - Helpers.Z(right),
                Helpers.W(left) - Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Subtract_Software(VectorFParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                Helpers.X(vector) - scalar,
                Helpers.Y(vector) - scalar,
                Helpers.Z(vector) - scalar,
                Helpers.W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) * Helpers.X(right),
                Helpers.Y(left) * Helpers.Y(right),
                Helpers.Z(left) * Helpers.Z(right),
                Helpers.W(left) * Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Multiply_Software(VectorFParam1_3 left, float scalar)
        {
            return Vector128.Create(
                Helpers.X(left) * scalar,
                Helpers.Y(left) * scalar,
                Helpers.Z(left) * scalar,
                Helpers.W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide_Software(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
        {
            return Vector128.Create(
                Helpers.X(dividend) / Helpers.X(divisor),
                Helpers.Y(dividend) / Helpers.Y(divisor),
                Helpers.Z(dividend) / Helpers.Z(divisor),
                Helpers.W(dividend) / Helpers.W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Divide_Software(VectorFParam1_3 dividend, float scalarDivisor)
        {
            return Vector128.Create(
                Helpers.X(dividend) / scalarDivisor,
                Helpers.Y(dividend) / scalarDivisor,
                Helpers.Z(dividend) / scalarDivisor,
                Helpers.W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Sqrt_Software(VectorFParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Sqrt(Helpers.X(vector)),
                MathF.Sqrt(Helpers.Y(vector)),
                MathF.Sqrt(Helpers.Z(vector)),
                MathF.Sqrt(Helpers.W(vector))
            );
        }

        #endregion
    }
}
