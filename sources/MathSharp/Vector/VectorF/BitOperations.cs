using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.VectorFloat.SoftwareFallbacks;
using MathSharp.Attributes;

namespace MathSharp.VectorFloat
{
    using VectorF = Vector128<float>;
    using VectorFParam1_3 = Vector128<float>;

    public static class BitOperations
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        public static readonly Vector128<float> MaskX = Vector128.Create(+0, -1, -1, -1).AsSingle();
        public static readonly Vector128<float> MaskY = Vector128.Create(-1, +0, -1, -1).AsSingle();
        public static readonly Vector128<float> MaskZ = Vector128.Create(-1, -1, +0, -1).AsSingle();
        public static readonly Vector128<float> MaskW = Vector128.Create(-1, -1, -1, +0).AsSingle();

        #region Bitwise Operations

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Or(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Or(left, right);
            }

            return Or_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF And(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.And(left, right);
            }

            return And_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Xor(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Xor(left, right);
            }

            return Xor_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Not(VectorFParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                VectorF mask = Vector128.Create(-1, -1, -1, -1).AsSingle();
                return Sse.AndNot(vector, mask);
            }

            return Not_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF AndNot(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.AndNot(left, right);
            }

            return AndNot_Software(left, right);
        }

        #endregion
    }
}
