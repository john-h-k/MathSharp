﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class Vector
    {

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplyAdd(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
            {
                return Vector256.Create(
                    Math.FusedMultiplyAdd(X(x), X(y), X(z)),
                    Math.FusedMultiplyAdd(Y(x), Y(y), Y(z)),
                    Math.FusedMultiplyAdd(Z(x), Z(y), Z(z)),
                    Math.FusedMultiplyAdd(W(x), W(y), W(z))
                );
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplyAdd(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedMultiplyAdd(x, y, z);
            }

            return Add(Multiply(x, y), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedNegateMultiplyAdd(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
            {
                ThrowPlatformNotSupported();
                return default;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastNegateMultiplyAdd(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (CanFuseOperations)
            {
                // FMA is faster than Add-Mul where it compiles to the native instruction, but it is not exactly semantically equivalent
                return FusedNegateMultiplyAdd(x, y, z);
            }

            return Add(Negate(Multiply(x, y)), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplySubtract(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
            {
                ThrowPlatformNotSupported();
                return default;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplySubtract(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtract(x, y, z);
            }

            return Subtract(Multiply(x, y), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedNegateMultiplySubtract(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractNegated(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
            {
                ThrowPlatformNotSupported();
                return default;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastNegateMultiplySubtract(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedNegateMultiplySubtract(x, y, z);
            }

            return Subtract(Negate(Multiply(x, y)), z);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplyAddSubtractAlternating(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplyAddSubtract(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, DoubleConstants.MaskNotSignYW));
            }
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplyAddSubtractAlternating(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplyAddSubtractAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, DoubleConstants.MaskNotSignYW);
            return Add(mul, negate);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FusedMultiplySubtractAddAlternating(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (Fma.IsSupported)
            {
                return Fma.MultiplySubtractAdd(x, y, z);
            }

            return SoftwareFallback(x, y, z);

            static Vector256<double> SoftwareFallback(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
            {
                return FusedMultiplyAdd(x, y, Xor(z, DoubleConstants.MaskNotSignXZ));
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> FastMultiplySubtractAddAlternating(Vector4DParam1_3 x, Vector4DParam1_3 y, Vector4DParam1_3 z)
        {
            if (CanFuseOperations)
            {
                return FusedMultiplySubtractAddAlternating(x, y, z);
            }

            var mul = Multiply(x, y);
            var negate = Xor(z, DoubleConstants.MaskNotSignXZ);
            return Add(mul, negate);
        }
    }
}