using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

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
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
               BoolToSimdBoolSingle(lX == rX),
               BoolToSimdBoolSingle(lY == rY),
               BoolToSimdBoolSingle(lZ == rZ),
               BoolToSimdBoolSingle(lW == rW)
            );
        }

        private static bool IsNan(float a, float b) => float.IsNaN(a) || float.IsNaN(b);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareNotEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                BoolToSimdBoolSingle(lX != rX || IsNan(lX, rX)),
                BoolToSimdBoolSingle(lY != rY || IsNan(lY, rY)),
                BoolToSimdBoolSingle(lZ != rZ || IsNan(lZ, rZ)),
                BoolToSimdBoolSingle(lW != rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                BoolToSimdBoolSingle(lX > rX || IsNan(lX, rX)),
                BoolToSimdBoolSingle(lY > rY || IsNan(lY, rY)),
                BoolToSimdBoolSingle(lZ > rZ || IsNan(lZ, rZ)),
                BoolToSimdBoolSingle(lW > rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThan_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                BoolToSimdBoolSingle(lX < rX/* || IsNan(lX, rX)*/),
                BoolToSimdBoolSingle(lY < rY/* || IsNan(lY, rY)*/),
                BoolToSimdBoolSingle(lZ < rZ/* || IsNan(lZ, rZ)*/),
                BoolToSimdBoolSingle(lW < rW/* || IsNan(lW, rW)*/)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                BoolToSimdBoolSingle(lX >= rX || IsNan(lX, rX)),
                BoolToSimdBoolSingle(lY >= rY || IsNan(lY, rY)),
                BoolToSimdBoolSingle(lZ >= rZ || IsNan(lZ, rZ)),
                BoolToSimdBoolSingle(lW >= rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThanOrEqual_Software(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                BoolToSimdBoolSingle(lX <= rX/* || IsNan(lX, rX)*/),
                BoolToSimdBoolSingle(lY <= rY/* || IsNan(lY, rY)*/),
                BoolToSimdBoolSingle(lZ <= rZ/* || IsNan(lZ, rZ)*/),
                BoolToSimdBoolSingle(lW <= rW/* || IsNan(lW, rW)*/)
            );
        }
    }
}
