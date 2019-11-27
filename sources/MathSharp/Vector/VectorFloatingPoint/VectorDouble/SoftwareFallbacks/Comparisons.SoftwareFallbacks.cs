using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp
{
    
    

    internal static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareEqual_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
               AsMaskDouble(lX == rX),
               AsMaskDouble(lY == rY),
               AsMaskDouble(lZ == rZ),
               AsMaskDouble(lW == rW)
            );
        }

        private static bool IsNan(double a, double b) => double.IsNaN(a) || double.IsNaN(b);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareNotEqual_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                AsMaskDouble(lX != rX || IsNan(lX, rX)),
                AsMaskDouble(lY != rY || IsNan(lY, rY)),
                AsMaskDouble(lZ != rZ || IsNan(lZ, rZ)),
                AsMaskDouble(lW != rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareGreaterThan_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                AsMaskDouble(lX > rX || IsNan(lX, rX)),
                AsMaskDouble(lY > rY || IsNan(lY, rY)),
                AsMaskDouble(lZ > rZ || IsNan(lZ, rZ)),
                AsMaskDouble(lW > rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareLessThan_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                AsMaskDouble(lX < rX/* || IsNan(lX, rX)*/),
                AsMaskDouble(lY < rY/* || IsNan(lY, rY)*/),
                AsMaskDouble(lZ < rZ/* || IsNan(lZ, rZ)*/),
                AsMaskDouble(lW < rW/* || IsNan(lW, rW)*/)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareGreaterThanOrEqual_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                AsMaskDouble(lX >= rX || IsNan(lX, rX)),
                AsMaskDouble(lY >= rY || IsNan(lY, rY)),
                AsMaskDouble(lZ >= rZ || IsNan(lZ, rZ)),
                AsMaskDouble(lW >= rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> CompareLessThanOrEqual_Software(Vector256<double> left, Vector256<double> right)
        {
            double lX = X(left), rX = X(right);
            double lY = Y(left), rY = Y(right);
            double lZ = Z(left), rZ = Z(right);
            double lW = W(left), rW = W(right);

            return Vector256.Create(
                AsMaskDouble(lX <= rX/* || IsNan(lX, rX)*/),
                AsMaskDouble(lY <= rY/* || IsNan(lY, rY)*/),
                AsMaskDouble(lZ <= rZ/* || IsNan(lZ, rZ)*/),
                AsMaskDouble(lW <= rW/* || IsNan(lW, rW)*/)
            );
        }
    }
}
