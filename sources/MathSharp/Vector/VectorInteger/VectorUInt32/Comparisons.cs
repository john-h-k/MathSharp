using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4UInt32 = Vector128<uint>;
    using Vector4UInt32Param1_3 = Vector128<uint>;

    public static partial class Vector
    {
        private static readonly Vector4UInt32 ComparisonMask = Vector128.Create(int.MinValue).AsUInt32();

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Equality(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left, right);
            }

            return Equality_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Inequality(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<uint> mask = Vector128.Create(-1).AsUInt32();
                return Sse2.Xor(Sse2.CompareEqual(left, right), mask);
            }

            return Inequality_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 GreaterThan(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareGreaterThan(Sse2.Xor(left, ComparisonMask).AsInt32(), Sse2.Xor(right, ComparisonMask).AsInt32()).AsUInt32();
            }

            return GreaterThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 LessThan(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareLessThan(Sse2.Xor(left, ComparisonMask).AsInt32(), Sse2.Xor(right, ComparisonMask).AsInt32()).AsUInt32();
            }

            return LessThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 GreaterThanOrEqual(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<uint> mask = Vector128.Create(-1).AsUInt32();
                Vector4UInt32 temp = Sse2.CompareLessThan(Sse2.Xor(left, ComparisonMask).AsInt32(), Sse2.Xor(right, ComparisonMask).AsInt32()).AsUInt32();
                return Sse2.Xor(mask, mask);
            }

            return GreaterThanOrEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 LessThanOrEqual(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                Vector128<uint> mask = Vector128.Create(-1).AsUInt32();
                Vector4UInt32 temp = Sse2.CompareGreaterThan(Sse2.Xor(left, ComparisonMask).AsInt32(), Sse2.Xor(right, ComparisonMask).AsInt32()).AsUInt32();
                return Sse2.Xor(mask, mask);
            }

            return LessThanOrEqual_Software(left, right);
        }
    }
}
