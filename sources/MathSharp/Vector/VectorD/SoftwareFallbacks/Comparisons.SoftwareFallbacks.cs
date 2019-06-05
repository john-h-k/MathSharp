using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Helpers;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp.SoftwareFallbacks
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    internal static partial class SoftwareFallbacksVector4F
    {
        [MethodImpl(MaxOpt)]
        public static Vector4D Equality_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
               BoolToSimdBoolDouble(X(left) == X(right)),
               BoolToSimdBoolDouble(Y(left) == Y(right)),
               BoolToSimdBoolDouble(Z(left) == Z(right)),
               BoolToSimdBoolDouble(W(left) == W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Inequality_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                BoolToSimdBoolDouble(X(left) != X(right)),
                BoolToSimdBoolDouble(Y(left) != Y(right)),
                BoolToSimdBoolDouble(Z(left) != Z(right)),
                BoolToSimdBoolDouble(W(left) != W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D GreaterThan_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                BoolToSimdBoolDouble(X(left) > X(right)),
                BoolToSimdBoolDouble(Y(left) > Y(right)),
                BoolToSimdBoolDouble(Z(left) > Z(right)),
                BoolToSimdBoolDouble(W(left) > W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D LessThan_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                BoolToSimdBoolDouble(X(left) < X(right)),
                BoolToSimdBoolDouble(Y(left) < Y(right)),
                BoolToSimdBoolDouble(Z(left) < Z(right)),
                BoolToSimdBoolDouble(W(left) < W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D GreaterThanOrEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                BoolToSimdBoolDouble(X(left) >= X(right)),
                BoolToSimdBoolDouble(Y(left) >= Y(right)),
                BoolToSimdBoolDouble(Z(left) >= Z(right)),
                BoolToSimdBoolDouble(W(left) >= W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D LessThanOrEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                BoolToSimdBoolDouble(X(left) <= X(right)),
                BoolToSimdBoolDouble(Y(left) <= Y(right)),
                BoolToSimdBoolDouble(Z(left) <= Z(right)),
                BoolToSimdBoolDouble(W(left) <= W(right))
            );
        }
    }
}
