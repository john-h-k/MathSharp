using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Equality_Software(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            return Vector128.Create(
               Helpers.BoolToSimdBoolSingle(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Inequality_Software(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS GreaterThan_Software(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS LessThan_Software(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS GreaterThanOrEqual_Software(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS LessThanOrEqual_Software(in Vector4FParam1_3 left, in Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) <= Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) <= Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) <= Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) <= Helpers.W(right))
            );
        }
    }
}
