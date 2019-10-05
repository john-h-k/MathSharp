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
        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareEqual(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left, right);
            }

            return CompareEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareNotEqual(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<int> mask = Vector128.Create(-1).AsInt32();
                return Sse2.Xor(Sse2.CompareEqual(left, right), mask);
            }

            return CompareNotEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareGreaterThan(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareGreaterThan(left, right);
            }

            return CompareGreaterThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareLessThan(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareLessThan(left, right);
            }

            return CompareLessThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareGreaterThanOrEqual(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<int> mask = Vector128.Create(-1).AsInt32();
                return Sse2.Xor(Sse2.CompareLessThan(left, right), mask);
            }

            return CompareGreaterThanOrEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 CompareLessThanOrEqual(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<int> mask = Vector128.Create(-1).AsInt32();
                return Sse2.Xor(Sse2.CompareGreaterThan(left, right), mask);
            }

            return CompareLessThanOrEqual_Software(left, right);
        }
    }
}
