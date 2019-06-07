using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4D Equality_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
               Helpers.BoolToSimdBoolDouble(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolDouble(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolDouble(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolDouble(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Inequality_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolDouble(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D GreaterThan_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolDouble(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D LessThan_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolDouble(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D GreaterThanOrEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolDouble(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D LessThanOrEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolDouble(Helpers.X(left) <= Helpers.X(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Y(left) <= Helpers.Y(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.Z(left) <= Helpers.Z(right)),
                Helpers.BoolToSimdBoolDouble(Helpers.W(left) <= Helpers.W(right))
            );
        }
    }
}
