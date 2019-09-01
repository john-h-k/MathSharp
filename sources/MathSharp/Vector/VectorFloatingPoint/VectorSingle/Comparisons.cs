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
        
        [MethodImpl(MaxOpt)]
        public static Vector4F Equality(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareEqual(left, right);
            }

            return Equality_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4F Inequality(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareNotEqual(left, right);
            }

            return Inequality_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4F GreaterThan(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareGreaterThan(left, right);
            }

            return GreaterThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4F LessThan(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareLessThan(left, right);
            }

            return LessThan_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4F GreaterThanOrEqual(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareGreaterThanOrEqual(left, right);
            }

            return GreaterThanOrEqual_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4F LessThanOrEqual(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
            {
                return Sse.CompareLessThanOrEqual(left, right);
            }

            return LessThanOrEqual_Software(left, right);
        }
    }
}
