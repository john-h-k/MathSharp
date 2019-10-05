using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4UInt32 = Vector128<uint>;
    using Vector4UInt32Param1_3 = Vector128<uint>;

    internal static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 CompareEqual_Software(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
               Helpers.BoolToSimdBoolUInt32(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolUInt32(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 CompareNotEqual_Software(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 CompareGreaterThan_Software(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 CompareLessThan_Software(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 CompareGreaterThanOrEqual_Software(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolUInt32(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolUInt32(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 CompareLessThanOrEqual_Software(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
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
