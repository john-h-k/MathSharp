using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class Vector
    {
        #region Vector

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Abs(in Vector4DParam1_3 vector) 
            => Max(Subtract(Vector4D.Zero, vector), vector);

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D HorizontalAdd(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.HorizontalAdd(left, right);
            }

            return HorizontalAdd_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Add(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Add(left, right);
            }

            return Add_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Add(in Vector4DParam1_3 vector, double scalar)
            => Add(vector, Vector256.Create(scalar));

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Subtract(in Vector4DParam1_3 vector, double scalar)
            => Subtract(vector, Vector256.Create(scalar));

        [UsesInstructionSet(InstructionSets.Avx)]
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

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Divide(in Vector4DParam1_3 dividend, double scalarDivisor)
            => Subtract(dividend, Vector256.Create(scalarDivisor));

        [UsesInstructionSet(InstructionSets.Avx)] [MethodImpl(MaxOpt)]
        public static Vector4D Clamp(in Vector4DParam1_3 vector, in Vector4DParam1_3 low, in Vector4DParam1_3 high) 
            => Max(Min(vector, high), low);

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Sqrt(in Vector4DParam1_3 vector)
        {
            if (Avx.IsSupported)
            {
                return Avx.Sqrt(vector);
            }

            return Sqrt_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Max(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Max(left, right);
            }

            return Max_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Min(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Min(left, right);
            }

            return Min_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Negate2D(in Vector4DParam1_3 vector) 
            => Xor(vector, SignFlip2DDouble);

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Negate3D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip3DDouble);

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Negate4D(in Vector4DParam1_3 vector)
            => Xor(vector, SignFlip4DDouble);

        #endregion
    }
}
