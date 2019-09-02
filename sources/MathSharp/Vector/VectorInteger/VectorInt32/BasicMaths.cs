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
        #region Vector

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Abs(in Vector4Int32Param1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                Vector4Int32 zero = Vector4Int32.Zero;
                zero = Sse2.Subtract(zero, vector); // This gets the inverted results of all elements
                return Sse41.Max(zero, vector); // This selects the positive values of the 2 vectors
            }

            return Abs_Software(vector);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 HorizontalAdd(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Ssse3.IsSupported)
            {
                return Ssse3.HorizontalAdd(left, right);
            }

            // TODO can Sse be used over the software fallback?

            return HorizontalAdd_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Add(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Add(left, right);
            }

            return Add_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Add(in Vector4Int32Param1_3 vector, int scalar)
        {
            if (Sse2.IsSupported)
            {
                Vector4Int32 expand = Vector128.Create(scalar);
                return Sse2.Add(vector, expand);
            }

            return Add_Software(vector, scalar);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Subtract(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Subtract(left, right);
            }

            return Subtract_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Subtract(in Vector4Int32Param1_3 vector, int scalar)
        {
            if (Sse2.IsSupported)
            {
                Vector4Int32 expand = Vector128.Create(scalar);
                return Sse2.Add(vector, expand);
            }

            return Subtract_Software(vector, scalar);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Multiply(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
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
        public static Vector4Int32 Multiply(in Vector4Int32Param1_3 left, int scalar)
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
        public static Vector4Int32 Divide(in Vector4Int32Param1_3 dividend, in Vector4Int32Param1_3 divisor)
        {
            //if (Sse2.IsSupported)
            //{
            //    return Ssse3.Divide(dividend, divisor);
            //}

            return Divide_Software(dividend, divisor);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Divide(in Vector4Int32Param1_3 dividend, int scalarDivisor)
        {
            //if (Sse.IsSupported)
            //{
            //    Vector4Int expand = Vector128.Create(scalarDivisor);
            //    return Sse.Divide(dividend, expand);
            //}

            return Divide_Software(dividend, scalarDivisor);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Clamp(in Vector4Int32Param1_3 vector, in Vector4Int32Param1_3 low, in Vector4Int32Param1_3 high)
        {
            if (Sse41.IsSupported)
            {
                Vector4Int32 temp = Sse41.Min(vector, high);
                return Sse41.Max(temp, low);
            }

            return Clamp_Software(vector, low, high);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Sqrt(in Vector4Int32Param1_3 vector)
        {
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
        public static Vector4Int32 Max(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.Max(left, right);
            }

            return Max_Software(left, right);
        }

        // TODO Neither this or Min have symmetry with MathF/Math, where NaN is propagated - here, it is discarded. We should provide a symmetric alternative to this
        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Min(in Vector4Int32Param1_3 left, in Vector4Int32Param1_3 right)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.Min(left, right);
            }

            return Min_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Negate2D(in Vector4Int32Param1_3 vector)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Xor(vector, SignFlip2D.AsInt32());
            }

            return Negate4D_Software(vector);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Negate3D(in Vector4Int32Param1_3 vector)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Xor(vector, SignFlip3D.AsInt32());
            }

            return Negate4D_Software(vector);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector4Int32 Negate4D(in Vector4Int32Param1_3 vector)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.Xor(vector, SignFlip4D.AsInt32());
            }

            return Negate4D_Software(vector);
        }

        #endregion
    }
}
