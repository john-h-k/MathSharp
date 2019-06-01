using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;

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
            if (IntrinsicSupport.Sse)
            {
                VectorF zero = VectorF.Zero;
                zero = Sse.Subtract(zero, vector); // This gets the inverted results of all elements
                return Sse.Max(zero, vector); // This selects the positive values of the 2 vectors
            }

            return SoftwareFallback(vector);

            static VectorF SoftwareFallback(VectorF vector)
            {
                return Vector128.Create(
                    MathF.Abs(Helpers.X(vector)),
                    MathF.Abs(Helpers.Y(vector)),
                    MathF.Abs(Helpers.Z(vector)),
                    MathF.Abs(Helpers.W(vector))
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse3)]
        [MethodImpl(MaxOpt)]
        public static VectorF HorizontalAdd(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse3)
            {
                return Sse3.HorizontalAdd(left, right);
            }

            // TODO can Sse be used over the software fallback?

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    Helpers.X(left) + Helpers.Y(left),
                    Helpers.Z(left) + Helpers.W(left),
                    Helpers.X(right) + Helpers.Y(right),
                    Helpers.Z(right) + Helpers.W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Add(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Add(left, right);
            }

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    Helpers.X(left) + Helpers.X(right),
                    Helpers.Y(left) + Helpers.Y(right),
                    Helpers.Z(left) + Helpers.Z(right),
                    Helpers.W(left) + Helpers.W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Add(VectorFParam1_3 vector, float scalar)
        {
            if (IntrinsicSupport.Sse)
            {
                VectorF expand = Vector128.Create(scalar);
                return Sse.Add(vector, expand);
            }

            return SoftwareFallback(vector, scalar);

            static VectorF SoftwareFallback(VectorFParam1_3 vector, float scalar)
            {
                return Vector128.Create(
                    Helpers.X(vector) + scalar,
                    Helpers.Y(vector) + scalar,
                    Helpers.Z(vector) + scalar,
                    Helpers.W(vector) + scalar
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Subtract(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Subtract(left, right);
            }

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    Helpers.X(left) - Helpers.X(right),
                    Helpers.Y(left) - Helpers.Y(right),
                    Helpers.Z(left) - Helpers.Z(right),
                    Helpers.W(left) - Helpers.W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Subtract(VectorFParam1_3 vector, float scalar)
        {
            if (IntrinsicSupport.Sse)
            {
                VectorF expand = Vector128.Create(scalar);
                return Sse.Add(vector, expand);
            }

            return SoftwareFallback(vector, scalar);

            static VectorF SoftwareFallback(VectorFParam1_3 vector, float scalar)
            {
                return Vector128.Create(
                    Helpers.X(vector) - scalar,
                    Helpers.Y(vector) - scalar,
                    Helpers.Z(vector) - scalar,
                    Helpers.W(vector) - scalar
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF PerElementMultiply(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Multiply(left, right);
            }

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    Helpers.X(left) * Helpers.X(right),
                    Helpers.Y(left) * Helpers.Y(right),
                    Helpers.Z(left) * Helpers.Z(right),
                    Helpers.W(left) * Helpers.W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Divide(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Divide(dividend, divisor);
            }

            return SoftwareFallback(dividend, divisor);

            static VectorF SoftwareFallback(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
            {
                return Vector128.Create(
                    Helpers.X(dividend) / Helpers.X(divisor),
                    Helpers.Y(dividend) / Helpers.Y(divisor),
                    Helpers.Z(dividend) / Helpers.Z(divisor),
                    Helpers.W(dividend) / Helpers.W(divisor)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Divide(VectorFParam1_3 dividend, float scalarDivisor)
        {
            if (IntrinsicSupport.Sse)
            {
                VectorF expand = Vector128.Create(scalarDivisor);
                return Sse.Divide(dividend, expand);
            }

            return SoftwareFallback(dividend, scalarDivisor);

            static VectorF SoftwareFallback(VectorFParam1_3 dividend, float scalarDivisor)
            {
                return Vector128.Create(
                    Helpers.X(dividend) / scalarDivisor,
                    Helpers.Y(dividend) / scalarDivisor,
                    Helpers.Z(dividend) / scalarDivisor,
                    Helpers.W(dividend) / scalarDivisor
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Sqrt(VectorFParam1_3 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Sqrt(vector);
            }

            return SoftwareFallback(vector);

            static VectorF SoftwareFallback(VectorFParam1_3 vector)
            {
                return Vector128.Create(
                    MathF.Sqrt(Helpers.X(vector)),
                    MathF.Sqrt(Helpers.Y(vector)),
                    MathF.Sqrt(Helpers.Z(vector)),
                    MathF.Sqrt(Helpers.W(vector))
                );
            }
        }

        #endregion
    }
}
