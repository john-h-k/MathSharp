using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        #region Vector

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Abs(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector4F zero = Vector4F.Zero;
                zero = Sse.Subtract(zero, vector); // This gets the inverted results of all elements
                return Sse.Max(zero, vector); // This selects the positive values of the 2 vectors
            }

            return Abs_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Sse3)]
        [MethodImpl(MaxOpt)]
        public static Vector4F HorizontalAdd(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
        public static Vector4F Add(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Add(left, right);
            }

            return Add_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Add(Vector4FParam1_3 vector, float scalar)
        {
            if (Sse.IsSupported)
            {
                Vector4F expand = Vector128.Create(scalar);
                return Sse.Add(vector, expand);
            }

            return Add_Software(vector, scalar);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Subtract(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Subtract(Vector4FParam1_3 vector, float scalar)
        {
            if (Sse.IsSupported)
            {
                Vector4F expand = Vector128.Create(scalar);
                return Sse.Add(vector, expand);
            }

            return Subtract_Software(vector, scalar);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Multiply(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, right);
            }

            return Multiply_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Multiply(Vector4FParam1_3 left, float scalar)
        {
            if (Sse.IsSupported)
            {
                return Sse.Multiply(left, Vector128.Create(scalar));
            }

            return Multiply_Software(left, scalar);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Divide(Vector4FParam1_3 dividend, Vector4FParam1_3 divisor)
        {
            if (Sse.IsSupported)
            {
                return Sse.Divide(dividend, divisor);
            }

            return Divide_Software(dividend, divisor);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Divide(Vector4FParam1_3 dividend, float scalarDivisor)
        {
            if (Sse.IsSupported)
            {
                Vector4F expand = Vector128.Create(scalarDivisor);
                return Sse.Divide(dividend, expand);
            }

            return Divide_Software(dividend, scalarDivisor);
        }

        [UsesInstructionSet(InstructionSets.Sse)] [MethodImpl(MaxOpt)]
        public static Vector4F Clamp(Vector4FParam1_3 vector, Vector4FParam1_3 low, Vector4FParam1_3 high)
        {
            if (Sse.IsSupported)
            {
                Vector4F temp = Sse.Min(vector, high);
                return Sse.Max(temp, low);
            }

            return Clamp_Software(vector, low, high);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Sqrt(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Sqrt(vector);
            }

            return Sqrt_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Max(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Max(left, right);
            }

            return Max_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Min(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Min(left, right);
            }

            return Min_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Negate2D(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Xor(vector, SignFlip2D);
            }

            return Negate4D_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Negate3D(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Xor(vector, SignFlip3D);
            }

            return Negate4D_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Negate4D(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.Xor(vector, SignFlip4D);
            }

            return Negate4D_Software(vector);
        }

        #endregion
    }
}
