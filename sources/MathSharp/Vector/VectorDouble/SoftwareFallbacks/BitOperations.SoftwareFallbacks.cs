using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static unsafe partial class SoftwareFallbacks
    {
        public static readonly Vector256<double> MaskXDouble = Vector256.Create(+0, -1, -1, -1).AsDouble();
        public static readonly Vector256<double> MaskYDouble = Vector256.Create(-1, +0, -1, -1).AsDouble();
        public static readonly Vector256<double> MaskZDouble = Vector256.Create(-1, -1, +0, -1).AsDouble();
        public static readonly Vector256<double> MaskWDouble = Vector256.Create(-1, -1, -1, +0).AsDouble();

        #region Bitwise Operations

        [MethodImpl(MaxOpt)]
        public static Vector4D Or_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
                double x1 = Helpers.X(left);
                double y1 = Helpers.Y(left);
                double z1 = Helpers.Z(left);
                double w1 = Helpers.W(left);

                double x2 = Helpers.X(right);
                double y2 = Helpers.Y(right);
                double z2 = Helpers.Z(right);
                double w2 = Helpers.W(right);

                ulong orX = Unsafe.As<double, ulong>(ref x1) | Unsafe.As<double, ulong>(ref x2);
                ulong orY = Unsafe.As<double, ulong>(ref y1) | Unsafe.As<double, ulong>(ref y2);
                ulong orZ = Unsafe.As<double, ulong>(ref z1) | Unsafe.As<double, ulong>(ref z2);
                ulong orW = Unsafe.As<double, ulong>(ref w1) | Unsafe.As<double, ulong>(ref w2);

                return Vector256.Create(
                    Unsafe.As<ulong, double>(ref orX),
                    Unsafe.As<ulong, double>(ref orY),
                    Unsafe.As<ulong, double>(ref orZ),
                    Unsafe.As<ulong, double>(ref orW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D And_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
                double x1 = Helpers.X(left);
                double y1 = Helpers.Y(left);
                double z1 = Helpers.Z(left);
                double w1 = Helpers.W(left);

                double x2 = Helpers.X(right);
                double y2 = Helpers.Y(right);
                double z2 = Helpers.Z(right);
                double w2 = Helpers.W(right);

                ulong andX = Unsafe.As<double, ulong>(ref x1) & Unsafe.As<double, ulong>(ref x2);
                ulong andY = Unsafe.As<double, ulong>(ref y1) & Unsafe.As<double, ulong>(ref y2);
                ulong andZ = Unsafe.As<double, ulong>(ref z1) & Unsafe.As<double, ulong>(ref z2);
                ulong andW = Unsafe.As<double, ulong>(ref w1) & Unsafe.As<double, ulong>(ref w2);

                return Vector256.Create(
                    Unsafe.As<ulong, double>(ref andX),
                    Unsafe.As<ulong, double>(ref andY),
                    Unsafe.As<ulong, double>(ref andZ),
                    Unsafe.As<ulong, double>(ref andW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Xor_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
                double x1 = Helpers.X(left);
                double y1 = Helpers.Y(left);
                double z1 = Helpers.Z(left);
                double w1 = Helpers.W(left);

                double x2 = Helpers.X(right);
                double y2 = Helpers.Y(right);
                double z2 = Helpers.Z(right);
                double w2 = Helpers.W(right);

                ulong xorX = Unsafe.As<double, ulong>(ref x1) ^ Unsafe.As<double, ulong>(ref x2);
                ulong xorY = Unsafe.As<double, ulong>(ref y1) ^ Unsafe.As<double, ulong>(ref y2);
                ulong xorZ = Unsafe.As<double, ulong>(ref z1) ^ Unsafe.As<double, ulong>(ref z2);
                ulong xorW = Unsafe.As<double, ulong>(ref w1) ^ Unsafe.As<double, ulong>(ref w2);

                return Vector256.Create(
                    Unsafe.As<ulong, double>(ref xorX),
                    Unsafe.As<ulong, double>(ref xorY),
                    Unsafe.As<ulong, double>(ref xorZ),
                    Unsafe.As<ulong, double>(ref xorW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Not_Software(in Vector4DParam1_3 vector)
        {
                double x = Helpers.X(vector);
                double y = Helpers.Y(vector);
                double z = Helpers.Z(vector);
                double w = Helpers.W(vector);

                ulong notX = ~Unsafe.As<double, ulong>(ref x);
                ulong notY = ~Unsafe.As<double, ulong>(ref y);
                ulong notZ = ~Unsafe.As<double, ulong>(ref z);
                ulong notW = ~Unsafe.As<double, ulong>(ref w);

                return Vector256.Create(
                    Unsafe.As<ulong, double>(ref notX),
                    Unsafe.As<ulong, double>(ref notY),
                    Unsafe.As<ulong, double>(ref notZ),
                    Unsafe.As<ulong, double>(ref notW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D AndNot_Software(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
                return And_Software(Not_Software(left), right);
        }

        #endregion
    }
}
