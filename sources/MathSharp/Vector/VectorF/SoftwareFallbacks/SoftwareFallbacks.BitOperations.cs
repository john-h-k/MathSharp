using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.Helpers;

namespace MathSharp.VectorFloat
{
    using VectorF = Vector128<float>;
    using VectorFParam1_3 = Vector128<float>;
    using VectorFWide = Vector256<float>;

    internal static unsafe partial class SoftwareFallbacks
    {
        public static readonly Vector128<float> MaskX = Vector128.Create(+0, -1, -1, -1).AsSingle();
        public static readonly Vector128<float> MaskY = Vector128.Create(-1, +0, -1, -1).AsSingle();
        public static readonly Vector128<float> MaskZ = Vector128.Create(-1, -1, +0, -1).AsSingle();
        public static readonly Vector128<float> MaskW = Vector128.Create(-1, -1, -1, +0).AsSingle();

        #region Bitwise Operations

        [MethodImpl(MaxOpt)]
        public static VectorF Or_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
                float x1 = X(left);
                float y1 = Y(left);
                float z1 = Z(left);
                float w1 = W(left);

                float x2 = X(right);
                float y2 = Y(right);
                float z2 = Z(right);
                float w2 = W(right);

                uint orX = Unsafe.As<float, uint>(ref x1) | Unsafe.As<float, uint>(ref x2);
                uint orY = Unsafe.As<float, uint>(ref y1) | Unsafe.As<float, uint>(ref y2);
                uint orZ = Unsafe.As<float, uint>(ref z1) | Unsafe.As<float, uint>(ref z2);
                uint orW = Unsafe.As<float, uint>(ref w1) | Unsafe.As<float, uint>(ref w2);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref orX),
                    Unsafe.As<uint, float>(ref orY),
                    Unsafe.As<uint, float>(ref orZ),
                    Unsafe.As<uint, float>(ref orW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF And_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
                float x1 = X(left);
                float y1 = Y(left);
                float z1 = Z(left);
                float w1 = W(left);

                float x2 = X(right);
                float y2 = Y(right);
                float z2 = Z(right);
                float w2 = W(right);

                uint andX = Unsafe.As<float, uint>(ref x1) & Unsafe.As<float, uint>(ref x2);
                uint andY = Unsafe.As<float, uint>(ref y1) & Unsafe.As<float, uint>(ref y2);
                uint andZ = Unsafe.As<float, uint>(ref z1) & Unsafe.As<float, uint>(ref z2);
                uint andW = Unsafe.As<float, uint>(ref w1) & Unsafe.As<float, uint>(ref w2);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref andX),
                    Unsafe.As<uint, float>(ref andY),
                    Unsafe.As<uint, float>(ref andZ),
                    Unsafe.As<uint, float>(ref andW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Xor_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
                float x1 = X(left);
                float y1 = Y(left);
                float z1 = Z(left);
                float w1 = W(left);

                float x2 = X(right);
                float y2 = Y(right);
                float z2 = Z(right);
                float w2 = W(right);

                uint xorX = Unsafe.As<float, uint>(ref x1) ^ Unsafe.As<float, uint>(ref x2);
                uint xorY = Unsafe.As<float, uint>(ref y1) ^ Unsafe.As<float, uint>(ref y2);
                uint xorZ = Unsafe.As<float, uint>(ref z1) ^ Unsafe.As<float, uint>(ref z2);
                uint xorW = Unsafe.As<float, uint>(ref w1) ^ Unsafe.As<float, uint>(ref w2);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref xorX),
                    Unsafe.As<uint, float>(ref xorY),
                    Unsafe.As<uint, float>(ref xorZ),
                    Unsafe.As<uint, float>(ref xorW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Not_Software(VectorFParam1_3 vector)
        {
                float x = X(vector);
                float y = Y(vector);
                float z = Z(vector);
                float w = W(vector);

                uint notX = ~Unsafe.As<float, uint>(ref x);
                uint notY = ~Unsafe.As<float, uint>(ref y);
                uint notZ = ~Unsafe.As<float, uint>(ref z);
                uint notW = ~Unsafe.As<float, uint>(ref w);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref notX),
                    Unsafe.As<uint, float>(ref notY),
                    Unsafe.As<uint, float>(ref notZ),
                    Unsafe.As<uint, float>(ref notW)
                );
        }

        [MethodImpl(MaxOpt)]
        public static VectorF AndNot_Software(VectorFParam1_3 left, VectorFParam1_3 right)
        {
                return And_Software(Not_Software(left), right);
        }

        #endregion
    }
}
