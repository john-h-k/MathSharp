using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4UInt32 = Vector128<uint>;
    using Vector4UInt32Param1_3 = Vector128<uint>;

    public static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Equality_Software(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
               Helpers.BoolToSimdBoolUInt32(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolUInt32(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Inequality_Software(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 GreaterThan_Software(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 LessThan_Software(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 GreaterThanOrEqual_Software(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 LessThanOrEqual_Software(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) <= Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) <= Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) <= Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) <= Helpers.W(right))
            );
        }
    }
}
