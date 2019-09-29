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
        public static HwVectorAnyS HorizontalAdd_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) + Y(left),
                Z(left) + W(left),
                X(right) + Y(right),
                Z(right) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Add_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) + X(right),
                Y(left) + Y(right),
                Z(left) + Z(right),
                W(left) + W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Subtract_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) - X(right),
                Y(left) - Y(right),
                Z(left) - Z(right),
                W(left) - W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Multiply_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                X(left) * X(right),
                Y(left) * Y(right),
                Z(left) * Z(right),
                W(left) * W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Divide_Software(Vector4FParam1_3 dividend, Vector4FParam1_3 divisor)
        {
            return Vector128.Create(
                X(dividend) / X(divisor),
                Y(dividend) / Y(divisor),
                Z(dividend) / Z(divisor),
                W(dividend) / W(divisor)
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Sqrt_Software(Vector4FParam1_3 vector)
        {
            return Vector128.Create(
                MathF.Sqrt(X(vector)),
                MathF.Sqrt(Y(vector)),
                MathF.Sqrt(Z(vector)),
                MathF.Sqrt(W(vector))
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Max_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                MathF.Max(X(left), X(right)),
                MathF.Max(Y(left), Y(right)),
                MathF.Max(Z(left), Z(right)),
                MathF.Max(W(left), W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Min_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                MathF.Min(X(left), X(right)),
                MathF.Min(Y(left), Y(right)),
                MathF.Min(Z(left), Z(right)),
                MathF.Min(W(left), W(right))
            );
        }

        #endregion
    }
}
