using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;

namespace MathSharp
{
    using Vector4UInt32 = Vector128<byte>;
    using Vector4UInt32Param1_3 = Vector128<byte>;

    using Vector4UInt64 = Vector256<byte>;
    using Vector4UInt64Param1_3 = Vector256<byte>;

    public static partial class Vector
    {
        public static readonly Vector128<uint> MaskXInt32 = Vector128.Create(+0, -1, -1, -1).AsUInt32();
        public static readonly Vector128<uint> MaskYInt32 = Vector128.Create(-1, +0, -1, -1).AsUInt32();
        public static readonly Vector128<uint> MaskZInt32 = Vector128.Create(-1, -1, +0, -1).AsUInt32();
        public static readonly Vector128<uint> MaskWInt32 = Vector128.Create(-1, -1, -1, +0).AsUInt32();
        public static readonly Vector128<uint> MaskZWInt32 = Vector128.Create(-1, -1, +0, +0).AsUInt32();

        #region Vector128

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Or(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Or(left, right);
            }

            return SoftwareFallbacks.Or_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 And(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.And(left, right);
            }

            return SoftwareFallbacks.And_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Xor(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Xor(left, right);
            }

            return SoftwareFallbacks.Xor_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Not(in Vector4UInt32Param1_3 vector)
        {
            if (Sse2.IsSupported)
            {
                Vector4UInt32 mask = Vector128.Create(-1, -1, -1, -1).AsByte();
                return Sse2.AndNot(vector, mask);
            }

            return SoftwareFallbacks.Not_Software(vector);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 AndNot(in Vector4UInt32Param1_3 left, in Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.AndNot(left, right);
            }

            return SoftwareFallbacks.AndNot_Software(left, right);
        }

        #endregion

        #region Vector256

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Or(in Vector4UInt64Param1_3 left, in Vector4UInt64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Or(left, right);
            }

            return SoftwareFallbacks.Or_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 And(in Vector4UInt64Param1_3 left, in Vector4UInt64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.And(left, right);
            }

            return SoftwareFallbacks.And_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Xor(in Vector4UInt64Param1_3 left, in Vector4UInt64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.Xor(left, right);
            }

            return SoftwareFallbacks.Xor_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 Not(in Vector4UInt64Param1_3 vector)
        {
            if (Avx2.IsSupported)
            {
                Vector4UInt64 mask = Vector256.Create(-1, -1, -1, -1).AsByte();
                return Avx2.AndNot(vector, mask);
            }

            return SoftwareFallbacks.Not_Software(vector);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt64 AndNot(in Vector4UInt64Param1_3 left, in Vector4UInt64Param1_3 right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.AndNot(left, right);
            }

            return SoftwareFallbacks.AndNot_Software(left, right);
        }

        #endregion
    }
}
