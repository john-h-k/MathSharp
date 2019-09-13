using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.Utils.Helpers;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class Vector
    {
        #region Vector


        [MethodImpl(MaxOpt)]
        public static Vector4D Abs(in Vector4DParam1_3 vector)
            => Max(Subtract(Vector4D.Zero, vector), vector);


        [MethodImpl(MaxOpt)]
        public static Vector4D HorizontalAdd(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.HorizontalAdd(left, right);
            }

            return HorizontalAdd_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Add(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Add(left, right);
            }

            return Add_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Add(in Vector4DParam1_3 vector, double scalar)
            => Add(vector, Vector256.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract(in Vector4DParam1_3 vector, double scalar)
            => Subtract(vector, Vector256.Create(scalar));


        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Multiply(in Vector4DParam1_3 vector, double scalar)
            => Multiply(vector, Vector256.Create(scalar));

        [MethodImpl(MaxOpt)]
        public static Vector4D Divide(in Vector4DParam1_3 dividend, in Vector4DParam1_3 divisor)
        {
            if (Avx.IsSupported)
            {
                return Avx.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Divide(in Vector4DParam1_3 dividend, double scalarDivisor)
            => Subtract(dividend, Vector256.Create(scalarDivisor));


        public static Vector4D Clamp(in Vector4DParam1_3 vector, in Vector4DParam1_3 low, in Vector4DParam1_3 high)
            => Max(Min(vector, high), low);


        [MethodImpl(MaxOpt)]
        public static Vector4D Sqrt(in Vector4DParam1_3 vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.Sqrt(vector);
            }

            return Sqrt_Software(vector);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Max(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Max(left, right);
            }

            return Max_Software(left, right);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4D Min(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Min(left, right);
            }

            return Min_Software(left, right);
        }

        public static HwVector2D Negate(HwVector2D vector)
            => Negate2D(vector);

        public static HwVector3D Negate(HwVector3D vector)
            => Negate3D(vector);

        public static HwVector4D Negate(HwVector4D vector)
            => Negate4D(vector);


        [MethodImpl(MaxOpt)]
        public static Vector4D Negate2D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip2DDouble);


        [MethodImpl(MaxOpt)]
        public static Vector4D Negate3D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip3DDouble);


        [MethodImpl(MaxOpt)]
        public static Vector4D Negate4D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip4DDouble);

        [MethodImpl(MaxOpt)]
        public static Vector4D Remainder(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            var n = Divide(left, right);
            n = Truncate(n);

            var y = Multiply(n, right);

            return Subtract(left, y);
        }

        public static Vector4D Remainder(in Vector4DParam1_3 left, double right)
            => Remainder(left, Vector256.Create(right));

        [MethodImpl(MaxOpt)]
        public static Vector4D Truncate(in Vector4DParam1_3 vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.RoundToZero(vector);
            }

            return SoftwareFallback(vector);

            static Vector4D SoftwareFallback(in Vector4DParam1_3 vector)
            {
                return Vector256.Create(
                   Math.Truncate(X(vector)),
                   Math.Truncate(Y(vector)),
                   Math.Truncate(Z(vector)),
                   Math.Truncate(W(vector))
               );
             }
        }
        #endregion
    }
}
