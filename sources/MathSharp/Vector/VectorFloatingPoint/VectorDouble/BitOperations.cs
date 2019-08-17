using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class Vector
    {
        public static readonly Vector256<double> MaskXDouble = Vector256.Create(+0, -1, -1, -1).AsDouble();
        public static readonly Vector256<double> MaskYDouble = Vector256.Create(-1, +0, -1, -1).AsDouble();
        public static readonly Vector256<double> MaskZDouble = Vector256.Create(-1, -1, +0, -1).AsDouble();
        public static readonly Vector256<double> MaskWDouble = Vector256.Create(-1, -1, -1, +0).AsDouble();

        public static readonly Vector256<double> MaskZWDouble = Vector256.Create(-1, -1, +0, +0).AsDouble();
        public static readonly Vector256<double> MaskYZWDouble = Vector256.Create(-1, +0, +0, +0).AsDouble();



        #region Bitwise Operations

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Or(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Or(left, right);
            }

            return SoftwareFallbacks.Or_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D And(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.And(left, right);
            }

            return SoftwareFallbacks.And_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Xor(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.Xor(left, right);
            }

            return SoftwareFallbacks.Xor_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Not(in Vector4DParam1_3 vector)
        {
            if (Avx.IsSupported)
            {
                Vector4D mask = Vector256.Create(-1, -1, -1, -1).AsDouble();
                return Avx.AndNot(vector, mask);
            }

            return SoftwareFallbacks.Not_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Avx)]
        [MethodImpl(MaxOpt)]
        public static Vector4D AndNot(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                return Avx.AndNot(left, right);
            }

            return SoftwareFallbacks.AndNot_Software(left, right);
        }

        #endregion
    }
}
