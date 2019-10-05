using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    internal static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
               Helpers.BoolToSimdBoolSingle(Helpers.X(left) == Helpers.X(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Y(left) == Helpers.Y(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.Z(left) == Helpers.Z(right)),
               Helpers.BoolToSimdBoolSingle(Helpers.W(left) == Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareNotEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) != Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) != Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) != Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) != Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) > Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) > Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) > Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) > Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) < Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) < Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) < Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) < Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return Vector128.Create(
                Helpers.BoolToSimdBoolSingle(Helpers.X(left) >= Helpers.X(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Y(left) >= Helpers.Y(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.Z(left) >= Helpers.Z(right)),
                Helpers.BoolToSimdBoolSingle(Helpers.W(left) >= Helpers.W(right))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
