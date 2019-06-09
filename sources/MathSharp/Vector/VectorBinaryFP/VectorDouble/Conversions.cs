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

        //[UsesInstructionSet(InstructionSets.Sse)]
        //[MethodImpl(MaxOpt)]
        //public static Vector4D Load(this Vector4 vector)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        return Sse.LoadVector256((double*)&vector);
        //    }

        //    return SoftwareFallback(vector);

        //    static Vector4D SoftwareFallback(Vector4 vector)
        //    {
        //        return Vector256.Create(vector.X, vector.Y, vector.Z, vector.W);
        //    }
        //}

        //[UsesInstructionSet(InstructionSets.Sse)]
        //[MethodImpl(MaxOpt)]
        //public static Vector4D Load(this Vector3 vector)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        // Construct 3 separate vectors, each with the first element being the value
        //        // and the rest being undefined (shown as ?)
        //        Vector4D lo = Vector256.CreateScalarUnsafe(vector.X);
        //        Vector4D mid = Vector256.CreateScalarUnsafe(vector.Y);
        //        Vector4D hi = Vector256.CreateScalarUnsafe(vector.Z);
        //        hi = MathSharp.Vector.And(hi, MathSharp.Vector.MaskWSingle);

        //        // Construct a vector of (lo, mid, ?, ?)
        //        Vector4D loMid = Sse.UnpackLow(lo, mid);

        //        // Given the hi vector is zeroed (because of the And), the first two elements are (hi, 0)
        //        // Move these 2 values to the last 2 elements of the loMid vector
        //        // This results in (lo, mid, hi, 0), the desired vector
        //        return Sse.MoveLowToHigh(loMid, hi);
        //    }

        //    return SoftwareFallback(vector);

        //    static Vector4D SoftwareFallback(Vector3 vector)
        //    {
        //        return Vector256.Create(vector.X, vector.Y, vector.Z, 0);
        //    }
        //}

        //[UsesInstructionSet(InstructionSets.Sse)]
        //[MethodImpl(MaxOpt)]
        //public static Vector4D Load(this Vector3 vector, double scalarW)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        // Construct 3 separate vectors, each with the first element being the value
        //        // and the rest being 0
        //        Vector4D lo = Vector256.CreateScalarUnsafe(vector.X);
        //        Vector4D midLo = Vector256.CreateScalarUnsafe(vector.Y);
        //        Vector4D midHi = Vector256.CreateScalarUnsafe(vector.Z);
        //        Vector4D hi = Vector256.CreateScalarUnsafe(scalarW);

        //        // Construct a vector of(lo, midLo, ?, ?) and(midHi, hi, ?, ?)
        //        Vector4D loMid = Sse.UnpackLow(lo, midLo);
        //        Vector4D hiMid = Sse.UnpackLow(midHi, hi);

        //        // Move the low elements of hiMid to high elements of lowMid
        //        // resulting in (lo, midLo, midHi, hi)
        //        return Sse.MoveLowToHigh(loMid, hiMid);

        //        // TODO minimise reg usage
        //    }

        //    return SoftwareFallback(vector, scalarW);

        //    static Vector4D SoftwareFallback(Vector3 vector, double scalarW)
        //    {
        //        return Vector256.Create(vector.X, vector.Y, vector.Z, scalarW);
        //    }
        //}

        //[UsesInstructionSet(InstructionSets.Sse)]
        //[MethodImpl(MaxOpt)]
        //public static Vector4D Load(this Vector2 vector)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        // Construct 2 separate vectors, each having the first element being the value
        //        // and the rest being 0
        //        Vector4D lo = Sse.LoadScalarVector256(&vector.X);
        //        Vector4D hi = Sse.LoadScalarVector256(&vector.Y);

        //        // Unpack these to (lo, mid, 0, 0), the desired vector
        //        return Sse.UnpackLow(lo, hi);
        //    }

        //    return SoftwareFallback(vector);

        //    static Vector4D SoftwareFallback(Vector2 vector)
        //    {
        //        return Vector256.Create(vector.X, vector.Y, 0, 0);
        //    }
        //}

        //[UsesInstructionSet(InstructionSets.Sse)]
        //[MethodImpl(MaxOpt)]
        //public static Vector4D LoadBroadcast(this Vector2 vector)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        // Construct 2 separate vectors, each having the first element being the value
        //        // and the rest being undefined (because we fill them later)
        //        Vector4D lo = Vector256.CreateScalarUnsafe(vector.X);
        //        Vector4D hi = Vector256.CreateScalarUnsafe(vector.Y);

        //        // Unpack these to (lo, mid, 0, 0), the desired vector
        //        Vector4D loHiHalf = Sse.UnpackLow(lo, hi);

        //        return Sse.MoveLowToHigh(loHiHalf, loHiHalf);
        //    }

        //    return SoftwareFallback(vector);

        //    static Vector4D SoftwareFallback(Vector2 vector)
        //    {
        //        return Vector256.Create(vector.X, vector.Y, vector.X, vector.Y);
        //    }
        //}

        //[UsesInstructionSet(InstructionSets.Sse)]
        //[MethodImpl(MaxOpt)]
        //public static Vector4D LoadScalar(this double scalar)
        //{
        //    if (Sse.IsSupported)
        //    {
        //        return Sse.LoadScalarVector256(&scalar);
        //    }

        //    return Vector256.CreateScalar(scalar);
        //}

        //[MethodImpl(MaxOpt)]
        //public static Vector4D LoadScalarBroadcast(this double scalar)
        //{
        //    return Vector256.Create(scalar);
        //}

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
        //        Vector4D hiBroadcast = Sse.Shuffle(vector, vector, Helpers.Shuffle(2, 2, 2, 2));
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

        //[UsesInstructionSet(InstructionSets.Avx2 | InstructionSets.Avx | InstructionSets.Sse)]
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
