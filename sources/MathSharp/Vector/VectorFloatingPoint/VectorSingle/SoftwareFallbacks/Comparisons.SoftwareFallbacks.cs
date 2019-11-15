using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MathSharp
{
    
    

    internal static partial class SoftwareFallbacks
    {
        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareEqual_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
               AsMaskSingle(lX == rX),
               AsMaskSingle(lY == rY),
               AsMaskSingle(lZ == rZ),
               AsMaskSingle(lW == rW)
            );
        }

        private static bool IsNan(float a, float b) => float.IsNaN(a) || float.IsNaN(b);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareNotEqual_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                AsMaskSingle(lX != rX || IsNan(lX, rX)),
                AsMaskSingle(lY != rY || IsNan(lY, rY)),
                AsMaskSingle(lZ != rZ || IsNan(lZ, rZ)),
                AsMaskSingle(lW != rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThan_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                AsMaskSingle(lX > rX || IsNan(lX, rX)),
                AsMaskSingle(lY > rY || IsNan(lY, rY)),
                AsMaskSingle(lZ > rZ || IsNan(lZ, rZ)),
                AsMaskSingle(lW > rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThan_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                AsMaskSingle(lX < rX/* || IsNan(lX, rX)*/),
                AsMaskSingle(lY < rY/* || IsNan(lY, rY)*/),
                AsMaskSingle(lZ < rZ/* || IsNan(lZ, rZ)*/),
                AsMaskSingle(lW < rW/* || IsNan(lW, rW)*/)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareGreaterThanOrEqual_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                AsMaskSingle(lX >= rX || IsNan(lX, rX)),
                AsMaskSingle(lY >= rY || IsNan(lY, rY)),
                AsMaskSingle(lZ >= rZ || IsNan(lZ, rZ)),
                AsMaskSingle(lW >= rW || IsNan(lW, rW))
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> CompareLessThanOrEqual_Software(Vector128<float> left, Vector128<float> right)
        {
            float lX = X(left), rX = X(right);
            float lY = Y(left), rY = Y(right);
            float lZ = Z(left), rZ = Z(right);
            float lW = W(left), rW = W(right);

            return Vector128.Create(
                AsMaskSingle(lX <= rX/* || IsNan(lX, rX)*/),
                AsMaskSingle(lY <= rY/* || IsNan(lY, rY)*/),
                AsMaskSingle(lZ <= rZ/* || IsNan(lZ, rZ)*/),
                AsMaskSingle(lW <= rW/* || IsNan(lW, rW)*/)
            );
        }
    }
}
