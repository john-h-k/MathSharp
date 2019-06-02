using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.VectorF;

namespace MathSharp.VectorFloat
{
    using VectorF = Vector128<float>;
    using VectorFParam1_3 = Vector128<float>;
    using VectorFWide = Vector256<float>;

    internal static unsafe partial class SoftwareFallbacks
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Arithmetic

        [MethodImpl(MaxOpt)]
        public static VectorF Abs_Software(VectorFParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Abs(Helpers.X(vector)),
                MathF.Abs(Helpers.Y(vector)),
                MathF.Abs(Helpers.Z(vector)),
                MathF.Abs(Helpers.W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF HorizontalAdd_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) + Helpers.Y(left),
                Helpers.Z(left) + Helpers.W(left),
                Helpers.X(right) + Helpers.Y(right),
                Helpers.Z(right) + Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Add_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) + Helpers.X(right),
                Helpers.Y(left) + Helpers.Y(right),
                Helpers.Z(left) + Helpers.Z(right),
                Helpers.W(left) + Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Add_Software(VectorFParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                Helpers.X(vector) + scalar,
                Helpers.Y(vector) + scalar,
                Helpers.Z(vector) + scalar,
                Helpers.W(vector) + scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Subtract_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) - Helpers.X(right),
                Helpers.Y(left) - Helpers.Y(right),
                Helpers.Z(left) - Helpers.Z(right),
                Helpers.W(left) - Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Subtract_Software(VectorFParam1_3 vector, float scalar)
        {
            return Vector128.Create(
                Helpers.X(vector) - scalar,
                Helpers.Y(vector) - scalar,
                Helpers.Z(vector) - scalar,
                Helpers.W(vector) - scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Multiply_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            return Vector128.Create(
                Helpers.X(left) * Helpers.X(right),
                Helpers.Y(left) * Helpers.Y(right),
                Helpers.Z(left) * Helpers.Z(right),
                Helpers.W(left) * Helpers.W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Multiply_Software(VectorFParam1_3 left, float scalar)
        {
            return Vector128.Create(
                Helpers.X(left) * scalar,
                Helpers.Y(left) * scalar,
                Helpers.Z(left) * scalar,
                Helpers.W(left) * scalar
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Divide_Software(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
        {
            return Vector128.Create(
                Helpers.X(dividend) / Helpers.X(divisor),
                Helpers.Y(dividend) / Helpers.Y(divisor),
                Helpers.Z(dividend) / Helpers.Z(divisor),
                Helpers.W(dividend) / Helpers.W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Divide_Software(VectorFParam1_3 dividend, float scalarDivisor)
        {
            return Vector128.Create(
                Helpers.X(dividend) / scalarDivisor,
                Helpers.Y(dividend) / scalarDivisor,
                Helpers.Z(dividend) / scalarDivisor,
                Helpers.W(dividend) / scalarDivisor
            );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Sqrt_Software(VectorFParam1_3 vector)
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
