using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4Int32 = Vector128<int>;
    using Vector4Int32Param1_3 = Vector128<int>;

    public static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareEqual_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
               Helpers.BoolToSimdBoolInt32(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolInt32(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolInt32(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolInt32(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareNotEqual_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareGreaterThan_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareLessThan_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareGreaterThanOrEqual_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareLessThanOrEqual_Software(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) <= Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) <= Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) <= Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) <= Helpers.W(right))
            );
        }
    }
}
