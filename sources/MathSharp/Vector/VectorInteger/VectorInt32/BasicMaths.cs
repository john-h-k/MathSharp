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
            => Max(vector, Subtract(Vector4Int32Param1_3.Zero, vector));

        
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
            => Add(vector, Vector128.Create(scalar));

        
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
        public static Vector4Int32 Clamp(in Vector4Int32Param1_3 vector, in Vector4Int32Param1_3 low, in Vector4Int32Param1_3 high) 
            => Max(Min(vector, high), low);

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

        #endregion
    }
}
