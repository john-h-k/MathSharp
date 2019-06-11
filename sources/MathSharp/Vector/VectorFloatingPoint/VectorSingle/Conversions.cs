using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;

namespace MathSharp
{
    using Vector4F = Vector128<float>;

    public static unsafe partial class Vector
    {
        #region Loads

        // TODO all the code here already exists as Create'yyy' methods in Vector128 - should be cleaned up to just use that

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Load(this Vector4 vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.LoadVector128((float*)&vector);
            }

            return SoftwareFallback(vector);

            static Vector4F SoftwareFallback(Vector4 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, vector.W);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Load(this Vector3 vector)
        {
            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                Vector4F lo = Vector128.CreateScalarUnsafe(vector.X);
                Vector4F mid = Vector128.CreateScalarUnsafe(vector.Y);
                Vector4F hi = Vector128.CreateScalarUnsafe(vector.Z);
                hi = Sse.And(hi, Vector.MaskWSingle);

                // Construct a vector of (lo, mid, ?, ?)
                Vector4F loMid = Sse.UnpackLow(lo, mid);

                // Given the hi vector is zeroed (because of the And), the first two elements are (hi, 0)
                // Move these 2 values to the last 2 elements of the loMid vector
                // This results in (lo, mid, hi, 0), the desired vector
                return Sse.MoveLowToHigh(loMid, hi);
            }

            return SoftwareFallback(vector);

            static Vector4F SoftwareFallback(Vector3 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, 0);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Load(this Vector3 vector, float scalarW)
        {
            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being 0
                Vector4F lo = Vector128.CreateScalarUnsafe(vector.X);
                Vector4F midLo = Vector128.CreateScalarUnsafe(vector.Y);
                Vector4F midHi = Vector128.CreateScalarUnsafe(vector.Z);
                Vector4F hi = Vector128.CreateScalarUnsafe(scalarW);

                // Construct a vector of(lo, midLo, ?, ?) and(midHi, hi, ?, ?)
                Vector4F loMid = Sse.UnpackLow(lo, midLo);
                Vector4F hiMid = Sse.UnpackLow(midHi, hi);

                // Move the low elements of hiMid to high elements of lowMid
                // resulting in (lo, midLo, midHi, hi)
                return Sse.MoveLowToHigh(loMid, hiMid);

                // TODO minimise reg usage
            }

            return SoftwareFallback(vector, scalarW);

            static Vector4F SoftwareFallback(Vector3 vector, float scalarW)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, scalarW);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Load(this Vector2 vector)
        {
            if (Sse.IsSupported)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being 0
                Vector4F lo = Sse.LoadScalarVector128(&vector.X);
                Vector4F hi = Sse.LoadScalarVector128(&vector.Y);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                return Sse.UnpackLow(lo, hi);
            }

            return SoftwareFallback(vector);

            static Vector4F SoftwareFallback(Vector2 vector)
            {
                return Vector128.Create(vector.X, vector.Y, 0, 0);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F LoadBroadcast(this Vector2 vector)
        {
            if (Sse.IsSupported)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being undefined (because we fill them later)
                Vector4F lo = Vector128.CreateScalarUnsafe(vector.X);
                Vector4F hi = Vector128.CreateScalarUnsafe(vector.Y);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                Vector4F loHiHalf = Sse.UnpackLow(lo, hi);

                return Sse.MoveLowToHigh(loHiHalf, loHiHalf);
            }

            return SoftwareFallback(vector);

            static Vector4F SoftwareFallback(Vector2 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.X, vector.Y);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F LoadScalar(this float scalar)
        {
            if (Sse.IsSupported)
            {
                return Sse.LoadScalarVector128(&scalar);
            }

            return Vector128.CreateScalar(scalar);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F LoadScalarBroadcast(this float scalar)
        {
            return Vector128.Create(scalar);
        }

        #endregion

        #region Stores

        public static void Store(this Vector4F vector, out Vector4 destination)
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

            static void SoftwareFallback(Vector4F vector, out Vector4 destination)
            {
                destination = Unsafe.As<Vector4F, Vector4>(ref vector);
            }
        }

        public static void Store(this Vector4F vector, out Vector3 destination)
        {
            if (Sse.IsSupported)
            {
                Vector4F hiBroadcast = Sse.Shuffle(vector, vector, ShuffleValues._2_2_2_2);
                fixed (void* pDest = &destination)
                {
                    Sse.StoreLow((float*)pDest, vector);
                    Sse.StoreScalar((float*)pDest + 3, hiBroadcast);
                }

                return;
            }

            SoftwareFallback(vector, out destination);

            static void SoftwareFallback(Vector4F vector, out Vector3 destination)
            {
                destination = Unsafe.As<Vector4F, Vector3>(ref vector);
            }
        }

        public static void Store(this Vector4F vector, out Vector2 destination)
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

            static void SoftwareFallback(Vector4F vector, out Vector2 destination)
            {
                destination = Unsafe.As<Vector4F, Vector2>(ref vector);
            }
        }

        public static void Store(this Vector4F vector, out float destination)
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

            static void SoftwareFallback(Vector4F vector, out float destination)
            {
                destination = Unsafe.As<Vector4F, float>(ref vector);
            }
        }

        #endregion

        #region Movement

        [UsesInstructionSet(InstructionSets.Avx2 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F ScalarToVector(Vector4F scalar)
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

            static Vector4F SoftwareFallback(Vector4F scalar)
            {
                return Vector128.Create(Helpers.X(scalar));
            }

        }
        #endregion
    }
}
