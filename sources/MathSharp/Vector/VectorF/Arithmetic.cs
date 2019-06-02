using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.VectorF;
using static MathSharp.VectorFloat.SoftwareFallbacks;

namespace MathSharp.VectorFloat
{
    using VectorF = Vector128<float>;
    using VectorFParam1_3 = Vector128<float>;

    public static class Arithmetic
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Arithmetic

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Abs(VectorFParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                VectorF zero = VectorF.Zero;
                zero = Sse.Subtract(zero, vector); // This gets the inverted results of all elements
                return Sse.Max(zero, vector); // This selects the positive values of the 2 vectors
            }

            return Abs_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Sse3)]
        [MethodImpl(MaxOpt)]
        public static VectorF HorizontalAdd(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse3.IsSupported)
            {
                return Sse3.HorizontalAdd(left, right);
            }

            // TODO can Sse be used over the software fallback?

            return HorizontalAdd_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Add(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Add(left, right);
            }

            return Add_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Add(VectorFParam1_3 vector, float scalar)
        {
            if (Sse.IsSupported)
            {
                VectorF expand = Vector128.Create(scalar);
                return Sse.Add(vector, expand);
            }

            return Add_Software(vector, scalar);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Subtract(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Subtract(VectorFParam1_3 vector, float scalar)
        {
            if (Sse.IsSupported)
            {
                VectorF expand = Vector128.Create(scalar);
                return Sse.Add(vector, expand);
            }

            return Subtract_Software(vector, scalar);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Multiply(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Multiply(VectorFParam1_3 left, float scalar)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, Vector128.Create(scalar));
            }

            return Multiply_Software(left, scalar);
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Divide(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
        {
            if (Sse.IsSupported)
            {
                return Sse.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Divide(VectorFParam1_3 dividend, float scalarDivisor)
        {
            if (Sse.IsSupported)
            {
                VectorF expand = Vector128.Create(scalarDivisor);
                return Sse.Divide(dividend, expand);
            }

            return Divide_Software(dividend, scalarDivisor);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Sqrt(VectorFParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Sqrt(vector);
            }

            return Sqrt_Software(vector);
        }

        #endregion
    }
}
