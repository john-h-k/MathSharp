using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4Int32 = Vector128<int>;
    using Vector4Int32Param1_3 = Vector128<int>;

    public static partial class Vector
    {
        [UsesInstructionSet(InstructionSets.Sse2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Equality(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left, right);
            }

            return Equality_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Inequality(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<int> mask = Vector128.Create(-1).AsInt32();
                return Sse2.Xor(Sse2.CompareEqual(left, right), mask);
            }

            return Inequality_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 GreaterThan(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareGreaterThan(left, right);
            }

            return GreaterThan_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 LessThan(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareLessThan(left, right);
            }

            return LessThan_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 GreaterThanOrEqual(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<int> mask = Vector128.Create(-1).AsInt32();
                return Sse2.Xor(Sse2.CompareLessThan(left, right), mask);
            }

            return GreaterThanOrEqual_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse2)]
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 LessThanOrEqual(Vector4Int32Param1_3 left, Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<int> mask = Vector128.Create(-1).AsInt32();
                return Sse2.Xor(Sse2.CompareGreaterThan(left, right), mask);
            }

            return LessThanOrEqual_Software(left, right);
        }
    }
}
