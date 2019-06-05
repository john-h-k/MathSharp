using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Helpers;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp.SoftwareFallbacks
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    internal static partial class SoftwareFallbacksVector4F
    {
        [MethodImpl(MaxOpt)]
        public static Vector4F Equality_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
               BoolToSimdBoolSingle(X(left) == X(right)),
               BoolToSimdBoolSingle(Y(left) == Y(right)),
               BoolToSimdBoolSingle(Z(left) == Z(right)),
               BoolToSimdBoolSingle(W(left) == W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Inequality_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                BoolToSimdBoolSingle(X(left) != X(right)),
                BoolToSimdBoolSingle(Y(left) != Y(right)),
                BoolToSimdBoolSingle(Z(left) != Z(right)),
                BoolToSimdBoolSingle(W(left) != W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F GreaterThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                BoolToSimdBoolSingle(X(left) > X(right)),
                BoolToSimdBoolSingle(Y(left) > Y(right)),
                BoolToSimdBoolSingle(Z(left) > Z(right)),
                BoolToSimdBoolSingle(W(left) > W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F LessThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                BoolToSimdBoolSingle(X(left) < X(right)),
                BoolToSimdBoolSingle(Y(left) < Y(right)),
                BoolToSimdBoolSingle(Z(left) < Z(right)),
                BoolToSimdBoolSingle(W(left) < W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F GreaterThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                BoolToSimdBoolSingle(X(left) >= X(right)),
                BoolToSimdBoolSingle(Y(left) >= Y(right)),
                BoolToSimdBoolSingle(Z(left) >= Z(right)),
                BoolToSimdBoolSingle(W(left) >= W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F LessThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                BoolToSimdBoolSingle(X(left) <= X(right)),
                BoolToSimdBoolSingle(Y(left) <= Y(right)),
                BoolToSimdBoolSingle(Z(left) <= Z(right)),
                BoolToSimdBoolSingle(W(left) <= W(right))
            );
        }
    }
}
