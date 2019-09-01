using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4D = Vector256<double>;

    public static unsafe partial class Vector
    {
        #region Loads

        // TODO all the code here already exists as Create'yyy' methods in Vector256 - should be cleaned up to just use that

        
        
        [MethodImpl(MaxOpt)]
        public static Vector4D LoadScalar(this double scalar) 
            => Vector256.CreateScalar(scalar);

        [MethodImpl(MaxOpt)]
        public static Vector4D LoadScalarBroadcast(this double scalar) 
            => Vector256.Create(scalar);

        #endregion

        #region Stores

        //public static void Store(this Vector4D vector, out Vector4 destination)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        fixed (void* pDest = &destination)
        //        {
        //            Sse.Store((double*)pDest, vector);
        //        }

        //        return;
        //    }

        //    SoftwareFallback(vector, out destination);

        //    static void SoftwareFallback(Vector4D vector, out Vector4 destination)
        //    {
        //        destination = Unsafe.As<Vector4D, Vector4>(ref vector);
        //    }
        //}

        //public static void Store(this Vector4D vector, out Vector3 destination)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        Vector4D hiBroadcast = Sse.Shuffle(vector, vector, ShuffleValues._2_2_2_2);
        //        fixed (void* pDest = &destination)
        //        {
        //            Sse.StoreLow((double*)pDest, vector);
        //            Sse.StoreScalar((double*)pDest + 3, hiBroadcast);
        //        }

        //        return;
        //    }

        //    SoftwareFallback(vector, out destination);

        //    static void SoftwareFallback(Vector4D vector, out Vector3 destination)
        //    {
        //        destination = Unsafe.As<Vector4D, Vector3>(ref vector);
        //    }
        //}

        //public static void Store(this Vector4D vector, out Vector2 destination)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        fixed (void* pDest = &destination)
        //        {
        //            Sse.StoreLow((double*)pDest, vector);
        //        }

        //        return;
        //    }

        //    SoftwareFallback(vector, out destination);

        //    static void SoftwareFallback(Vector4D vector, out Vector2 destination)
        //    {
        //        destination = Unsafe.As<Vector4D, Vector2>(ref vector);
        //    }
        //}

        //public static void Store(this Vector4D vector, out double destination)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        fixed (double* pDest = &destination)
        //        {
        //            Sse.StoreScalar(pDest, vector);
        //        }

        //        return;
        //    }

        //    SoftwareFallback(vector, out destination);

        //    static void SoftwareFallback(Vector4D vector, out double destination)
        //    {
        //        destination = Unsafe.As<Vector4D, double>(ref vector);
        //    }
        //}

        #endregion

        #region Movement

        //
        //[MethodImpl(MaxOpt)]
        //public static Vector4D ScalarToVector(Vector4D scalar)
        //{
        //    if (Avx2.IsSupported)
        //    {
        //        // TODO is this path better than Avx path or the same?
        //        return Avx2.BroadcastScalarToVector256(scalar);
        //    }
        //    else if (Avx.IsSupported)
        //    {
        //        return Avx.Permute(scalar, 0b_0000_0000);
        //    }
        //    else if (Sse.IsSupported)
        //    {
        //        return Sse.Shuffle(scalar, scalar, 0b_0000_0000);
        //    }

        //    return SoftwareFallback(scalar);

        //    static Vector4D SoftwareFallback(Vector4D scalar)
        //    {
        //        return Vector256.Create(Helpers.X(scalar));
        //    }

        //}
        #endregion
    }
}
