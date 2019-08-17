using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4UInt64 = Vector256<ulong>;
    using Vector4UInt64Param1_3 = Vector256<ulong>;

    public static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Equality_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
               Helpers.BoolToSimdBoolUInt64(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolUInt64(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolUInt64(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolUInt64(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Inequality_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolUInt64(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 GreaterThan_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolUInt64(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 LessThan_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolUInt64(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 GreaterThanOrEqual_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolUInt64(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 LessThanOrEqual_Software(Vector4UInt64Param1_3 left, Vector4UInt64Param1_3 right)
        {
            return Vector256.Create(
                Helpers.BoolToSimdBoolUInt64(Helpers.X(left) <= Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Y(left) <= Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.Z(left) <= Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt64(Helpers.W(left) <= Helpers.W(right))
            );
        }
    }
}
