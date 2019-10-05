using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4Int64 = Vector256<long>;
    using Vector4Int64Param1_3 = Vector256<long>;

    public static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Equality_Software(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            return Vector256.Create(
               Helpers.BoolToSimdBoolInt64(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolInt64(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolInt64(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolInt64(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Inequality_Software(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolInt64(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int64 GreaterThan_Software(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolInt64(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int64 LessThan_Software(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolInt64(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int64 GreaterThanOrEqual_Software(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolInt64(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int64 LessThanOrEqual_Software(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolInt64(Helpers.X(left) <= Helpers.X(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Y(left) <= Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.Z(left) <= Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt64(Helpers.W(left) <= Helpers.W(right))
            );
        }
    }
}
