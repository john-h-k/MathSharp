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
        public static Vector4F Equality_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
               Helpers.BoolToSimdBoolSingle(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Inequality_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F GreaterThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F LessThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F GreaterThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F LessThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
