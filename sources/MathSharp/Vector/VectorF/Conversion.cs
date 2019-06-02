using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.Helpers;
using BitOperations = MathSharp.VectorFloat.BitOperations;

namespace MathSharp.VectorF
{
    using VectorF = Vector128<float>;
    using VectorFParam1_3 = Vector128<float>;
    using VectorFWide = Vector256<float>;

    public static unsafe class Conversion
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Loads

        // TODO all the code here already exists as Create'yyy' methods in Vector128 - should be cleaned up to just use that

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector4 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.LoadVector128((float*)&vector);
            }

            return SoftwareFallback(vector);

            static VectorF SoftwareFallback(Vector4 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, vector.W);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector3 vector)
        {
            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                VectorF lo = Vector128.CreateScalarUnsafe(vector.X);
                VectorF mid = Vector128.CreateScalarUnsafe(vector.Y);
                VectorF hi = Vector128.CreateScalarUnsafe(vector.Z);
                hi = BitOperations.And(hi, BitOperations.MaskW);

                // Construct a vector of (lo, mid, ?, ?)
                VectorF loMid = Sse.UnpackLow(lo, mid);

                // Given the hi vector is zeroed (because of the And), the first two elements are (hi, 0)
                // Move these 2 values to the last 2 elements of the loMid vector
                // This results in (lo, mid, hi, 0), the desired vector
                return Sse.MoveLowToHigh(loMid, hi);
            }

            return SoftwareFallback(vector);

            static VectorF SoftwareFallback(Vector3 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, 0);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector3 vector, float scalarW)
        {
            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being 0
                VectorF lo = Vector128.CreateScalarUnsafe(vector.X);
                VectorF midLo = Vector128.CreateScalarUnsafe(vector.Y);
                VectorF midHi = Vector128.CreateScalarUnsafe(vector.Z);
                VectorF hi = Vector128.CreateScalarUnsafe(scalarW);

                // Construct a vector of(lo, midLo, ?, ?) and(midHi, hi, ?, ?)
                VectorF loMid = Sse.UnpackLow(lo, midLo);
                VectorF hiMid = Sse.UnpackLow(midHi, hi);

                // Move the low elements of hiMid to high elements of lowMid
                // resulting in (lo, midLo, midHi, hi)
                return Sse.MoveLowToHigh(loMid, hiMid);

                // TODO minimise reg usage
            }

            return SoftwareFallback(vector, scalarW);

            static VectorF SoftwareFallback(Vector3 vector, float scalarW)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, scalarW);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector2 vector)
        {
            if (Sse.IsSupported)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being 0
                VectorF lo = Sse.LoadScalarVector128(&vector.X);
                VectorF hi = Sse.LoadScalarVector128(&vector.Y);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                return Sse.UnpackLow(lo, hi);
            }

            return SoftwareFallback(vector);

            static VectorF SoftwareFallback(Vector2 vector)
            {
                return Vector128.Create(vector.X, vector.Y, 0, 0);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF LoadBroadcast(this Vector2 vector)
        {
            if (Sse.IsSupported)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being undefined (because we fill them later)
                VectorF lo = Vector128.CreateScalarUnsafe(vector.X);
                VectorF hi = Vector128.CreateScalarUnsafe(vector.Y);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                VectorF loHiHalf = Sse.UnpackLow(lo, hi);

                return Sse.MoveLowToHigh(loHiHalf, loHiHalf);
            }

            return SoftwareFallback(vector);

            static VectorF SoftwareFallback(Vector2 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.X, vector.Y);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF LoadScalar(this float scalar)
        {
            if (Sse.IsSupported)
            {
                return Sse.LoadScalarVector128(&scalar);
            }

            return Vector128.CreateScalar(scalar);
        }

        [MethodImpl(MaxOpt)]
        public static VectorF LoadScalarBroadcast(this float scalar)
        {
            return Vector128.Create(scalar);
        }

        #endregion

        #region Stores

        public static void Store(this VectorF vector, out Vector4 destination)
        {
            if (Sse.IsSupported)
            {
                fixed (void* pDest = &destination)
                {
                    Sse.Store((float*)pDest, vector);
                }

                return;
            }

            SoftwareFallback(vector, out destination);

            static void SoftwareFallback(VectorF vector, out Vector4 destination)
            {
                destination = Unsafe.As<VectorF, Vector4>(ref vector);
            }
        }

        public static void Store(this VectorF vector, out Vector3 destination)
        {
            if (Sse.IsSupported)
            {
                VectorF hiBroadcast = Sse.Shuffle(vector, vector, Shuffle(2, 2, 2, 2));
                fixed (void* pDest = &destination)
                {
                    Sse.StoreLow((float*)pDest, vector);
                    Sse.StoreScalar((float*)pDest + 3, hiBroadcast);
                }

                return;
            }

            SoftwareFallback(vector, out destination);

            static void SoftwareFallback(VectorF vector, out Vector3 destination)
            {
                destination = Unsafe.As<VectorF, Vector3>(ref vector);
            }
        }

        public static void Store(this VectorF vector, out Vector2 destination)
        {
            if (Sse.IsSupported)
            {
                fixed (void* pDest = &destination)
                {
                    Sse.StoreLow((float*)pDest, vector);
                }

                return;
            }

            SoftwareFallback(vector, out destination);

            static void SoftwareFallback(VectorF vector, out Vector2 destination)
            {
                destination = Unsafe.As<VectorF, Vector2>(ref vector);
            }
        }

        public static void Store(this VectorF vector, out float destination)
        {
            if (Sse.IsSupported)
            {
                fixed (float* pDest = &destination)
                {
                    Sse.StoreScalar(pDest, vector);
                }

                return;
            }

            SoftwareFallback(vector, out destination);

            static void SoftwareFallback(VectorF vector, out float destination)
            {
                destination = Unsafe.As<VectorF, float>(ref vector);
            }
        }

        #endregion

        #region Movement

        [UsesInstructionSet(InstructionSets.Avx2 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF ScalarToVector(VectorF scalar)
        {
            if (Avx2.IsSupported)
            {
                // TODO is this path better than Avx path or the same?
                return Avx2.BroadcastScalarToVector128(scalar);
            }
            else if (Avx.IsSupported)
            {
                return Avx.Permute(scalar, 0b_0000_0000);
            }
            else if (Sse.IsSupported)
            {
                return Sse.Shuffle(scalar, scalar, 0b_0000_0000);
            }

            return SoftwareFallback(scalar);

            static VectorF SoftwareFallback(VectorF scalar)
            {
                return Vector128.Create(X(scalar));
            }

        }
        #endregion
    }
}
