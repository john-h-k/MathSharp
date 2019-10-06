using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        // Whether 'Fastxxx' operations use 'Fusedxxx' operations or not
        private static bool CanFuseOperations 
            => Fma.IsSupported && Options.AllowImpreciseMath;

        private static void ThrowPlatformNotSupported() =>
            ThrowHelper.ThrowPlatformNotSupportedException(FmaRequiredPlatformNotSupportedMessage());

        private static string FmaRequiredPlatformNotSupportedMessage()
            => "Platform not supported for operation as it does not support FMA instructions";

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplyAdd(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
            {
                return Vector128.Create(
                    MathF.FusedMultiplyAdd(X(x), X(y), X(z)),
                    MathF.FusedMultiplyAdd(Y(x), Y(y), Y(z)),
                    MathF.FusedMultiplyAdd(Z(x), Z(y), Z(z)),
                    MathF.FusedMultiplyAdd(W(x), W(y), W(z))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplyAdd(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedMultiplyAdd(x, y, z);
            }

            return Add(Multiply(x, y), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedNegateMultiplyAdd(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
            {
                ThrowPlatformNotSupported();
                return default;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastNegateMultiplyAdd(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedNegateMultiplyAdd(x, y, z);
            }

            return Add(Negate(Multiply(x, y)), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplySubtract(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
            {
                return FusedMultiplyAdd(x, y, Negate(z));
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplySubtract(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtract(x, y, z);
            }

            return Subtract(Multiply(x, y), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedNegateMultiplySubtract(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
            {
                ThrowPlatformNotSupported();
                return default;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastNegateMultiplySubtract(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedNegateMultiplySubtract(x, y, z);
            }

            return Subtract(Negate(Multiply(x, y)), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplyAddSubtractAlternating(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddSubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, SingleConstants.MaskNotSignYW));
            }
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplyAddSubtractAlternating(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplyAddSubtractAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, SingleConstants.MaskNotSignYW);
            return Add(mul, negate);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FusedMultiplySubtractAddAlternating(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector128<float> SoftwareFallback(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, SingleConstants.MaskNotSignXZ));
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> FastMultiplySubtractAddAlternating(Vector4FParam1_3 x, Vector4FParam1_3 y, Vector4FParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtractAddAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, SingleConstants.MaskNotSignXZ);
            return Add(mul, negate);
        }
    }
}
