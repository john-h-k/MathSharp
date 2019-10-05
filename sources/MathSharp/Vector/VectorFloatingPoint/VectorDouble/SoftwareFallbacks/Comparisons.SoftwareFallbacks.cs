using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

// ReSharper disable CompareOfdoublesByEqualityOperator

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    internal static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4D CompareEqual_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
               Helpers.BoolToSimdBoolSingle(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CompareNotEqual_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CompareGreaterThan_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CompareLessThan_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CompareGreaterThanOrEqual_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D CompareLessThanOrEqual_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) <= Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) <= Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) <= Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) <= Helpers.W(right))
            );
        }
    }
}
