using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        public static readonly Vector128<float> MaskX = Vector128.Create(+0, -1, -1, -1).AsSingle();
        public static readonly Vector128<float> MaskY = Vector128.Create(-1, +0, -1, -1).AsSingle();
        public static readonly Vector128<float> MaskZ = Vector128.Create(-1, -1, +0, -1).AsSingle();
        public static readonly Vector128<float> MaskW = Vector128.Create(-1, -1, -1, +0).AsSingle();

        public static readonly Vector128<float> MaskZW = Vector128.Create(-1, -1, +0, +0).AsSingle();



        #region Bitwise Operations

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Or(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Or(left, right);
            }

            return SoftwareFallbacks.Or_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F And(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.And(left, right);
            }

            return SoftwareFallbacks.And_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Xor(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.Xor(left, right);
            }

            return SoftwareFallbacks.Xor_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Not(Vector4FParam1_3 vector)
        {
            if (Sse.IsSupported)
            {
                Vector4F mask = Vector128.Create(-1, -1, -1, -1).AsSingle();
                return Sse.AndNot(vector, mask);
            }

            return SoftwareFallbacks.Not_Software(vector);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F AndNot(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.AndNot(left, right);
            }

            return SoftwareFallbacks.AndNot_Software(left, right);
        }

        #endregion
    }
}
