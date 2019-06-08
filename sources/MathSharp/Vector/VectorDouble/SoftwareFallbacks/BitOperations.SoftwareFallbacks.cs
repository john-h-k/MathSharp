using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using VectorDParam1_3 = Vector256<double>;

    public static unsafe partial class SoftwareFallbacks
    {
        #region Bitwise Operations

        [MethodImpl(MaxOpt)]
        public static Vector4D Or_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
                double x1 = Helpers.X(left);
                double y1 = Helpers.Y(left);
                double z1 = Helpers.Z(left);
                double w1 = Helpers.W(left);

                double x2 = Helpers.X(right);
                double y2 = Helpers.Y(right);
                double z2 = Helpers.Z(right);
                double w2 = Helpers.W(right);

                uint orX = Unsafe.As<double, uint>(ref x1) | Unsafe.As<double, uint>(ref x2);
                uint orY = Unsafe.As<double, uint>(ref y1) | Unsafe.As<double, uint>(ref y2);
                uint orZ = Unsafe.As<double, uint>(ref z1) | Unsafe.As<double, uint>(ref z2);
                uint orW = Unsafe.As<double, uint>(ref w1) | Unsafe.As<double, uint>(ref w2);

                return Vector256.Create(
                    Unsafe.As<uint, double>(ref orX),
                    Unsafe.As<uint, double>(ref orY),
                    Unsafe.As<uint, double>(ref orZ),
                    Unsafe.As<uint, double>(ref orW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D And_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
                double x1 = Helpers.X(left);
                double y1 = Helpers.Y(left);
                double z1 = Helpers.Z(left);
                double w1 = Helpers.W(left);

                double x2 = Helpers.X(right);
                double y2 = Helpers.Y(right);
                double z2 = Helpers.Z(right);
                double w2 = Helpers.W(right);

                uint andX = Unsafe.As<double, uint>(ref x1) & Unsafe.As<double, uint>(ref x2);
                uint andY = Unsafe.As<double, uint>(ref y1) & Unsafe.As<double, uint>(ref y2);
                uint andZ = Unsafe.As<double, uint>(ref z1) & Unsafe.As<double, uint>(ref z2);
                uint andW = Unsafe.As<double, uint>(ref w1) & Unsafe.As<double, uint>(ref w2);

                return Vector256.Create(
                    Unsafe.As<uint, double>(ref andX),
                    Unsafe.As<uint, double>(ref andY),
                    Unsafe.As<uint, double>(ref andZ),
                    Unsafe.As<uint, double>(ref andW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Xor_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
                double x1 = Helpers.X(left);
                double y1 = Helpers.Y(left);
                double z1 = Helpers.Z(left);
                double w1 = Helpers.W(left);

                double x2 = Helpers.X(right);
                double y2 = Helpers.Y(right);
                double z2 = Helpers.Z(right);
                double w2 = Helpers.W(right);

                uint xorX = Unsafe.As<double, uint>(ref x1) ^ Unsafe.As<double, uint>(ref x2);
                uint xorY = Unsafe.As<double, uint>(ref y1) ^ Unsafe.As<double, uint>(ref y2);
                uint xorZ = Unsafe.As<double, uint>(ref z1) ^ Unsafe.As<double, uint>(ref z2);
                uint xorW = Unsafe.As<double, uint>(ref w1) ^ Unsafe.As<double, uint>(ref w2);

                return Vector256.Create(
                    Unsafe.As<uint, double>(ref xorX),
                    Unsafe.As<uint, double>(ref xorY),
                    Unsafe.As<uint, double>(ref xorZ),
                    Unsafe.As<uint, double>(ref xorW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Not_Software(VectorDParam1_3 vector)
        {
                double x = Helpers.X(vector);
                double y = Helpers.Y(vector);
                double z = Helpers.Z(vector);
                double w = Helpers.W(vector);

                uint notX = ~Unsafe.As<double, uint>(ref x);
                uint notY = ~Unsafe.As<double, uint>(ref y);
                uint notZ = ~Unsafe.As<double, uint>(ref z);
                uint notW = ~Unsafe.As<double, uint>(ref w);

                return Vector256.Create(
                    Unsafe.As<uint, double>(ref notX),
                    Unsafe.As<uint, double>(ref notY),
                    Unsafe.As<uint, double>(ref notZ),
                    Unsafe.As<uint, double>(ref notW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D AndNot_Software(VectorDParam1_3 left, VectorDParam1_3 right)
        {
                return And_Software(Not_Software(left), right);
        }

        #endregion
    }
}
