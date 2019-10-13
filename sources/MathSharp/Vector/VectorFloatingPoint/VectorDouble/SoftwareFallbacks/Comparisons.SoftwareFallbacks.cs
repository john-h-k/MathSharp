using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    internal static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
               BoolToSimdBoolDouble(lX == rX),
               BoolToSimdBoolDouble(lY == rY),
               BoolToSimdBoolDouble(lZ == rZ),
               BoolToSimdBoolDouble(lW == rW)
            );
        }

        private static bool IsNan(double a, double b) => double.IsNaN(a) || double.IsNaN(b);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareNotEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                BoolToSimdBoolDouble(lX != rX || IsNan(lX, rX)),
                BoolToSimdBoolDouble(lY != rY || IsNan(lY, rY)),
                BoolToSimdBoolDouble(lZ != rZ || IsNan(lZ, rZ)),
                BoolToSimdBoolDouble(lW != rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareGreaterThan_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                BoolToSimdBoolDouble(lX > rX || IsNan(lX, rX)),
                BoolToSimdBoolDouble(lY > rY || IsNan(lY, rY)),
                BoolToSimdBoolDouble(lZ > rZ || IsNan(lZ, rZ)),
                BoolToSimdBoolDouble(lW > rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareLessThan_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                BoolToSimdBoolDouble(lX < rX/* || IsNan(lX, rX)*/),
                BoolToSimdBoolDouble(lY < rY/* || IsNan(lY, rY)*/),
                BoolToSimdBoolDouble(lZ < rZ/* || IsNan(lZ, rZ)*/),
                BoolToSimdBoolDouble(lW < rW/* || IsNan(lW, rW)*/)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareGreaterThanOrEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                BoolToSimdBoolDouble(lX >= rX || IsNan(lX, rX)),
                BoolToSimdBoolDouble(lY >= rY || IsNan(lY, rY)),
                BoolToSimdBoolDouble(lZ >= rZ || IsNan(lZ, rZ)),
                BoolToSimdBoolDouble(lW >= rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareLessThanOrEqual_Software(Vector4DParam1_3 left, Vector4DParam1_3 right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                BoolToSimdBoolDouble(lX <= rX/* || IsNan(lX, rX)*/),
                BoolToSimdBoolDouble(lY <= rY/* || IsNan(lY, rY)*/),
                BoolToSimdBoolDouble(lZ <= rZ/* || IsNan(lZ, rZ)*/),
                BoolToSimdBoolDouble(lW <= rW/* || IsNan(lW, rW)*/)
            );
        }
    }
}
