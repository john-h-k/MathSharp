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
        public static Vector4Int32 Equality_Software(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
               Helpers.BoolToSimdBoolInt32(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolInt32(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolInt32(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolInt32(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Inequality_Software(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 GreaterThan_Software(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 LessThan_Software(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 GreaterThanOrEqual_Software(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolInt32(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolInt32(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int32 LessThanOrEqual_Software(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
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
