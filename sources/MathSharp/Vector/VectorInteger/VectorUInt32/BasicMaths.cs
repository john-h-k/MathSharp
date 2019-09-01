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
        #region Vector
        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 HorizontalAdd(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Ssse3.IsSupported)
            {
                return Ssse3.HorizontalAdd(left.AsInt32(), right.AsInt32()).AsUInt32();
            }

            // TODO can Sse be used over the software fallback?

            return HorizontalAdd_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Add(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Add(left, right);
            }

            return Add_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Add(Vector4UInt32Param1_3 vector, uint scalar)
        {
            if (Sse2.IsSupported)
            {
                Vector4UInt32 expand = Vector128.Create(scalar);
                return Sse2.Add(vector, expand);
            }

            return Add_Software(vector, scalar);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Subtract(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Subtract(Vector4UInt32Param1_3 vector, uint scalar)
        {
            if (Sse2.IsSupported)
            {
                Vector4UInt32 expand = Vector128.Create(scalar);
                return Sse2.Add(vector, expand);
            }

            return Subtract_Software(vector, scalar);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Multiply(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.MultiplyLow(left, right);
            }
            // TODO try accelerate with less than < Sse4.1
            //else if (Sse2.IsSupported)
            //{
            //    Vector128<ulong> elem2And0 = Sse2.Multiply(left.AsUInt32(), right.AsUInt32());
            //}

            return Multiply_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Multiply(Vector4UInt32Param1_3 left, uint scalar)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.MultiplyLow(left, Vector128.Create(scalar));
            }
            // TODO try accelerate with less than < Sse4.1
            //else if (Sse2.IsSupported)
            //{
            //    Vector128<ulong> elem2And0 = Sse2.Multiply(left.AsUInt32(), right.AsUInt32());
            //}

            return Multiply_Software(left, scalar);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Divide(Vector4UInt32Param1_3 dividend, Vector4UInt32Param1_3 divisor)
        {
#warning No direct hardware acceleration for integer divison; research acceleration techniques
            //if (Sse2.IsSupported)
            //{
            //    return Ssse3.Divide(dividend, divisor);
            //}

            return Divide_Software(dividend, divisor);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Divide(Vector4UInt32Param1_3 dividend, uint scalarDivisor)
        {
#warning No direct hardware acceleration for integer divison; research acceleration techniques
            //if (Sse.IsSupported)
            //{
            //    Vector4Int expand = Vector128.Create(scalarDivisor);
            //    return Sse.Divide(dividend, expand);
            //}

            return Divide_Software(dividend, scalarDivisor);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Clamp(Vector4UInt32Param1_3 vector, Vector4UInt32Param1_3 low, Vector4UInt32Param1_3 high)
        {
            if (Sse41.IsSupported)
            {
                Vector4UInt32 temp = Sse41.Min(vector, high);
                return Sse41.Max(temp, low);
            }

            return Clamp_Software(vector, low, high);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Sqrt(Vector4UInt32Param1_3 vector)
        {
#warning No direct hardware acceleration for integer sqrt; research acceleration techniques
            //if (Sse.IsSupported)
            //{
            //    return Sse42.Sqrt(vector);
            //}

            return Sqrt_Software(vector);
        }

        // Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded, and also with +0/-0, where with MathF/Math, +0 is returned over -0,
        // - here, the second op is returned irrelevant of value if both are +0/-0
        // TODO We should provide a symmetric alternative to this
        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Max(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.Max(left, right);
            }

            return Max_Software(left, right);
        }

        // TODO Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded. We should provide a symmetric alternative to this
        
        [MethodImpl(MaxOpt)]
        public static Vector4UInt32 Min(Vector4UInt32Param1_3 left, Vector4UInt32Param1_3 right)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.Min(left, right);
            }

            return Min_Software(left, right);
        }
        
        #endregion
    }
}
