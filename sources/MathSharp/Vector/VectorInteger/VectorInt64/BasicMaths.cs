using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4Int64 = Vector256<long>;
    using Vector4Int64Param1_3 = Vector256<long>;

    public static partial class Vector
    {
        #region Vector

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Abs(Vector4Int64Param1_3 vector)
        {
            if (Avx2.IsSupported)
            {
                Vector4Int64 zero = Vector4Int64.Zero;
                zero = Avx2.Subtract(zero, vector); // This gets the inverted results of all elements
                return Avx2.Max(zero, vector); // This selects the positive values of the 2 vectors
            }

            return Abs_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Ssse3)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 HorizontalAdd(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            if (Ssse3.IsSupported)
            {
                return Avx2.HorizontalAdd(left, right);
            }

            // TODO can Sse be used over the software fallback?

            return HorizontalAdd_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Add(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Add(left, right);
            }

            return Add_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Add(Vector4Int64Param1_3 vector, long scalar)
        {
            if (Avx2.IsSupported)
            {
                Vector4Int64 expand = Vector256.Create(scalar);
                return Avx2.Add(vector, expand);
            }

            return Add_Software(vector, scalar);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Subtract(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Subtract(Vector4Int64Param1_3 vector, long scalar)
        {
            if (Avx2.IsSupported)
            {
                Vector4Int64 expand = Vector256.Create(scalar);
                return Avx2.Add(vector, expand);
            }

            return Subtract_Software(vector, scalar);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Multiply(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.MultiplyLow(left, right);
            }
            // TODO try accelerate with less than < Sse4.1
            //else if (Avx2.IsSupported)
            //{
            //    Vector128<ulong> elem2And0 = Avx2.Multiply(left.AsUInt32(), right.AsUInt32());
            //}

            return Multiply_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Multiply(Vector4Int64Param1_3 left, long scalar)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.MultiplyLow(left, Vector128.Create(scalar));
            }
            // TODO try accelerate with less than < Sse4.1
            //else if (Avx2.IsSupported)
            //{
            //    Vector128<ulong> elem2And0 = Avx2.Multiply(left.AsUInt32(), right.AsUInt32());
            //}

            return Multiply_Software(left, scalar);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Divide(Vector4Int64Param1_3 dividend, Vector4Int64Param1_3 divisor)
        {
#warning No direct hardware acceleration for longeger divison; research acceleration techniques
            //if (Avx2.IsSupported)
            //{
            //    return Ssse3.Divide(dividend, divisor);
            //}

            return Divide_Software(dividend, divisor);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Divide(Vector4Int64Param1_3 dividend, long scalarDivisor)
        {
#warning No direct hardware acceleration for longeger divison; research acceleration techniques
            //if (Sse.IsSupported)
            //{
            //    Vector4Int expand = Vector128.Create(scalarDivisor);
            //    return Sse.Divide(dividend, expand);
            //}

            return Divide_Software(dividend, scalarDivisor);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Clamp(Vector4Int64Param1_3 vector, Vector4Int64Param1_3 low, Vector4Int64Param1_3 high)
        {
            if (Avx2.IsSupported)
            {
                Vector4Int64 temp = Avx2.Min(vector, high);
                return Avx2.Max(temp, low);
            }

            return Clamp_Software(vector, low, high);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Sqrt(Vector4Int64Param1_3 vector)
        {
#warning No direct hardware acceleration for longeger sqrt; research acceleration techniques
            //if (Sse.IsSupported)
            //{
            //    return Sse42.Sqrt(vector);
            //}

            return Sqrt_Software(vector);
        }

        // Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded, and also with +0/-0, where with MathF/Math, +0 is returned over -0,
        // - here, the second op is returned irrelevant of value if both are +0/-0
        // TODO We should provide a symmetric alternative to this
        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Max(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Max(left, right);
            }

            return Max_Software(left, right);
        }

        // TODO Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded. We should provide a symmetric alternative to this
        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Min(Vector4Int64Param1_3 left, Vector4Int64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Min(left, right);
            }

            return Min_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Negate2D(Vector4Int64Param1_3 vector)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Xor(vector, SignFlip2D.AsInt32());
            }

            return Negate4D_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Negate3D(Vector4Int64Param1_3 vector)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Xor(vector, SignFlip3D.AsInt32());
            }

            return Negate4D_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Avx2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int64 Negate4D(Vector4Int64Param1_3 vector)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Xor(vector, SignFlip4D.AsInt32());
            }

            return Negate4D_Software(vector);
        }

        #endregion
    }
}
