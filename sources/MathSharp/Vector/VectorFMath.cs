
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.Helpers;

namespace MathSharp
{
    using VectorF = Vector128<float>;
    using VectorFParam1_3 = Vector128<float>;
    using VectorFParam4 = Vector128<float>;
    using VectorFParam5_6 = Vector128<float>;
    using VectorFParam7_ = Vector128<float>;

    using VectorFThin = Vector64<float>;
    using VectorFWide = Vector256<float>;

    /// <summary>
    /// A class containing loading, storing, arithmetic, bitwise, and vector math operations
    /// for <see cref="VectorF"/> and <see cref="VectorFWide"/> types
    /// </summary>
    public static unsafe class VectorFMath
    {
        /// <summary>
        /// Gets a value that indicates whether vector math is accelerated using hardware-specific intrinsic instructions
        /// </summary>
        /// <returns><code>true</code> if hardware acceleration occurs, else, <code>false</code></returns>
        public static readonly bool IsHardwareAccelerated = IntrinsicSupport.Sse;

        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Loads

        // TODO all the code here already exists as Create'yyy' methods in Vector128 - should be cleaned up to just use that

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector4 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.LoadVector128((float*)&vector);
            }

            return LoadSoftware(vector);

            static VectorF LoadSoftware(Vector4 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, vector.W);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector3 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being 0
                VectorF lo = Sse.LoadScalarVector128(&vector.X);
                VectorF mid = Sse.LoadScalarVector128(&vector.Y);
                VectorF hi = Sse.LoadScalarVector128(&vector.Z);

                // Construct a vector of (lo, mid, 0, 0)
                VectorF loMid = Sse.UnpackLow(lo, mid);

                // Given the hi vector is zeroed, the first two elements are (hi, 0)
                // Move these 2 values to the last 2 elements of the loMid vector
                // This results in (lo, mid, hi, 0), the desired vector
                return Sse.MoveLowToHigh(loMid, hi);
            }

            return LoadSoftware(vector);

            static VectorF LoadSoftware(Vector3 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, 0);
            }
        }


        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector3 vector, float scalarW)
        {
            if (IntrinsicSupport.Sse)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being 0
                VectorF lo = Vector128.CreateScalarUnsafe(vector.X);
                VectorF midLo = Vector128.CreateScalarUnsafe(vector.Y);
                VectorF midHi = Vector128.CreateScalarUnsafe(vector.Z);
                VectorF hi = Vector128.CreateScalarUnsafe(scalarW);

                // Construct a vector of (lo, midLo, ?, ?) and (midHi, hi, ?, ?)
                VectorF loMid = Sse.UnpackLow(lo, midLo);
                VectorF hiMid = Sse.UnpackLow(midHi, hi);

                // Move the low elements of hiMid to high elements of lowMid
                // resulting in (lo, midLo, midHi, hi)
                return Sse.MoveLowToHigh(loMid, hiMid);

                // TODO minimise reg usage
            }

            return LoadSoftware(vector);

            static VectorF LoadSoftware(Vector3 vector, float scalarW)
            {
                return Vector128.Create(vector.X, vector.Y, vector.Z, scalarW);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Load(this Vector2 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being 0
                VectorF lo = Sse.LoadScalarVector128(&vector.X);
                VectorF hi = Sse.LoadScalarVector128(&vector.Y);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                return Sse.UnpackLow(lo, hi);
            }

            return LoadSoftware(vector);

            static VectorF LoadSoftware(Vector2 vector)
            {
                return Vector128.Create(vector.X, vector.Y, 0, 0);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF LoadBroadcast(this Vector2 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being undefined (because we fill them later)
                VectorF lo = Vector128.CreateScalarUnsafe(vector.X);
                VectorF hi = Vector128.CreateScalarUnsafe(vector.Y);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                VectorF loHiHalf = Sse.UnpackLow(lo, hi);

                return Sse.UnpackHigh(loHiHalf, loHiHalf);
            }

            return LoadSoftware(vector);

            static VectorF LoadSoftware(Vector2 vector)
            {
                return Vector128.Create(vector.X, vector.Y, vector.X, vector.Y);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF LoadScalar(this float scalar)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.LoadScalarVector128(&scalar);
            }

            return Vector128.CreateScalar(scalar);
        }

        [UsesInstructionSet(InstructionSets.Avx2 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF LoadScalarBroadcast(this float scalar)
        {
            if (IntrinsicSupport.Avx2)
            {
                // Intrinsic, which is a nop if 'scalar' is already in a XMM reg
                VectorF load = Vector128.CreateScalarUnsafe(scalar);
                return Avx2.BroadcastScalarToVector128(load);
            }
            else if (IntrinsicSupport.Avx)
            {
                // Intrinsic, which is a nop if 'scalar' is already in a XMM reg
                VectorF load = Vector128.CreateScalarUnsafe(scalar);
                return Avx.Permute(load, 0b_0000_0000);
            }
            else if (IntrinsicSupport.Sse)
            {
                // TODO investigate eliding redundant init of upper 3 elements from LoadScalarVector128
                VectorF lo = Sse.LoadScalarVector128(&scalar);

                Sse.Shuffle(lo, lo, 0b_0000_0000);
            }

            return LoadScalarBroadcastSoftware(scalar);

            static VectorF LoadScalarBroadcastSoftware(float scalar)
            {
                return Vector128.Create(scalar);
            }
        }

        #endregion

        #region Stores

        public static void Store(this VectorF vector, out Vector4 destination)
        {
            if (IntrinsicSupport.Sse)
            {
                fixed (float* pDest = &destination.X)
                {
                    Sse.Store(pDest, vector);
                }
            }

            StoreSoftware(vector, out destination);

            static void StoreSoftware(VectorF vector, out Vector4 destination)
            {
                destination = Unsafe.As<VectorF, Vector4>(ref vector);
            }
        }

        public static void Store(this VectorF vector, out Vector3 destination)
        {
            if (IntrinsicSupport.Sse)
            {
                VectorF hiBroadcast = Sse.Shuffle(vector, vector, Shuffle(2, 2, 2, 2));
                fixed (float* pDest = &destination.X)
                {
                    Sse.StoreLow(pDest, vector);
                    Sse.StoreScalar(pDest + 3, hiBroadcast);
                }
            }

            StoreSoftware(vector, out destination);

            static void StoreSoftware(VectorF vector, out Vector3 destination)
            {
                destination = Unsafe.As<VectorF, Vector3>(ref vector);
            }
        }

        public static void Store(this VectorF vector, out Vector2 destination)
        {
            if (IntrinsicSupport.Sse)
            {
                fixed (float* pDest = &destination.X)
                {
                    Sse.StoreLow(pDest, vector);
                }
            }

            StoreSoftware(vector, out destination);

            static void StoreSoftware(VectorF vector, out Vector2 destination)
            {
                destination = Unsafe.As<VectorF, Vector2>(ref vector);
            }
        }

        public static void Store(this VectorF vector, out float destination)
        {
            if (IntrinsicSupport.Sse)
            {
                fixed (float* pDest = &destination)
                {
                    Sse.StoreScalar(pDest, vector);
                }
            }

            StoreSoftware(vector, out destination);

            static void StoreSoftware(VectorF vector, out float destination)
            {
                destination = Unsafe.As<VectorF, float>(ref vector);
            }
        }

        #endregion

        #region Arithmetic

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Abs(VectorFParam1_3 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                VectorF zero = VectorF.Zero;
                zero = Sse.Subtract(zero, vector); // This gets the inverted results of all elements
                return Sse.Max(zero, vector); // This selects the positive values of the 2 vectors
            }

            return AbsSoftware(vector);

            static VectorF AbsSoftware(VectorF vector)
            {
                return Vector128.Create(
                    MathF.Abs(X(vector)),
                    MathF.Abs(Y(vector)),
                    MathF.Abs(Z(vector)),
                    MathF.Abs(W(vector))
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse3)]
        [MethodImpl(MaxOpt)]
        public static VectorF HorizontalAdd(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse3)
            {
                return Sse3.HorizontalAdd(left, right);
            }

            return HorizontalAddSoftware(left, right);

            static VectorF HorizontalAddSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    X(left) + Y(left),
                    Z(left) + W(left),
                    X(right) + Y(right),
                    Z(right) + W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Add(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Add(left, right);
            }

            return AddSoftware(left, right);

            static VectorF AddSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    X(left) + X(right),
                    Y(left) + Y(right),
                    Z(left) + Z(right),
                    W(left) + W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Subtract(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Subtract(left, right);
            }

            return SubtractSoftware(left, right);

            static VectorF SubtractSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    X(left) - X(right),
                    Y(left) - Y(right),
                    Z(left) - Z(right),
                    W(left) - W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Multiply(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Multiply(left, right);
            }

            return MultiplySoftware(left, right);

            static VectorF MultiplySoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    X(left) * X(right),
                    Y(left) * Y(right),
                    Z(left) * Z(right),
                    W(left) * W(right)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Divide(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Divide(dividend, divisor);
            }

            return DivideSoftware(dividend, divisor);

            static VectorF DivideSoftware(VectorFParam1_3 dividend, VectorFParam1_3 divisor)
            {
                return Vector128.Create(
                    X(dividend) / X(divisor),
                    Y(dividend) / Y(divisor),
                    Z(dividend) / Z(divisor),
                    W(dividend) / W(divisor)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Sqrt(VectorFParam1_3 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Sqrt(vector);
            }

            return SqrtSoftware(vector);

            static VectorF SqrtSoftware(VectorFParam1_3 vector)
            {
                return Vector128.Create(
                    MathF.Sqrt(X(vector)),
                    MathF.Sqrt(Y(vector)),
                    MathF.Sqrt(Z(vector)),
                    MathF.Sqrt(W(vector))
                );
            }
        }

        #endregion

        #region Bitwise Operations

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Or(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Or(left, right);
            }

            return OrSoftware(left, right);

            static VectorF OrSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                float x1 = X(left);
                float y1 = Y(left);
                float z1 = Z(left);
                float w1 = W(left);

                float x2 = X(right);
                float y2 = Y(right);
                float z2 = Z(right);
                float w2 = W(right);

                uint orX = Unsafe.As<float, uint>(ref x1) | Unsafe.As<float, uint>(ref x2);
                uint orY = Unsafe.As<float, uint>(ref y1) | Unsafe.As<float, uint>(ref y2);
                uint orZ = Unsafe.As<float, uint>(ref z1) | Unsafe.As<float, uint>(ref z2);
                uint orW = Unsafe.As<float, uint>(ref w1) | Unsafe.As<float, uint>(ref w2);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref orX),
                    Unsafe.As<uint, float>(ref orY),
                    Unsafe.As<uint, float>(ref orZ),
                    Unsafe.As<uint, float>(ref orW)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF And(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.And(left, right);
            }

            return AndSoftware(left, right);

            static VectorF AndSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                float x1 = X(left);
                float y1 = Y(left);
                float z1 = Z(left);
                float w1 = W(left);

                float x2 = X(right);
                float y2 = Y(right);
                float z2 = Z(right);
                float w2 = W(right);

                uint andX = Unsafe.As<float, uint>(ref x1) & Unsafe.As<float, uint>(ref x2);
                uint andY = Unsafe.As<float, uint>(ref y1) & Unsafe.As<float, uint>(ref y2);
                uint andZ = Unsafe.As<float, uint>(ref z1) & Unsafe.As<float, uint>(ref z2);
                uint andW = Unsafe.As<float, uint>(ref w1) & Unsafe.As<float, uint>(ref w2);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref andX),
                    Unsafe.As<uint, float>(ref andY),
                    Unsafe.As<uint, float>(ref andZ),
                    Unsafe.As<uint, float>(ref andW)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Xor(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.Xor(left, right);
            }

            return XorSoftware(left, right);

            static VectorF XorSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                float x1 = X(left);
                float y1 = Y(left);
                float z1 = Z(left);
                float w1 = W(left);

                float x2 = X(right);
                float y2 = Y(right);
                float z2 = Z(right);
                float w2 = W(right);

                uint xorX = Unsafe.As<float, uint>(ref x1) ^ Unsafe.As<float, uint>(ref x2);
                uint xorY = Unsafe.As<float, uint>(ref y1) ^ Unsafe.As<float, uint>(ref y2);
                uint xorZ = Unsafe.As<float, uint>(ref z1) ^ Unsafe.As<float, uint>(ref z2);
                uint xorW = Unsafe.As<float, uint>(ref w1) ^ Unsafe.As<float, uint>(ref w2);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref xorX),
                    Unsafe.As<uint, float>(ref xorY),
                    Unsafe.As<uint, float>(ref xorZ),
                    Unsafe.As<uint, float>(ref xorW)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Not(VectorFParam1_3 vector)
        {
            if (IntrinsicSupport.Sse)
            {
                VectorF mask = Vector128.Create(-1, -1, -1, -1).AsSingle();
                return Sse.AndNot(vector, mask);
            }

            return NotSoftware(vector);

            static VectorF NotSoftware(VectorFParam1_3 vector)
            {
                float x = X(vector);
                float y = Y(vector);
                float z = Z(vector);
                float w = W(vector);

                uint notX = ~Unsafe.As<float, uint>(ref x);
                uint notY = ~Unsafe.As<float, uint>(ref y);
                uint notZ = ~Unsafe.As<float, uint>(ref z);
                uint notW = ~Unsafe.As<float, uint>(ref w);

                return Vector128.Create(
                    Unsafe.As<uint, float>(ref notX),
                    Unsafe.As<uint, float>(ref notY),
                    Unsafe.As<uint, float>(ref notZ),
                    Unsafe.As<uint, float>(ref notW)
                );
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF AndNot(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                return Sse.AndNot(left, right);
            }

            return AndNotSoftware(left, right);

            static VectorF AndNotSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return And(Not(left), right);
            }
        }

        #endregion

        #region Vector Maths

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Vector3DotProduct(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (IntrinsicSupport.Sse41)
            {
                // This multiplies the first 3 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0111_1111;
                return Sse41.DotProduct(left, right, control);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (IntrinsicSupport.Sse3)
            {
                VectorF mul = Multiply(left, right);

                // Set W to zero
                VectorF result = And(mul, MaskWToZero);

                // Doubly horizontally adding fills the final vector with the sum
                result = HorizontalAdd(result, result);
                return HorizontalAdd(result, result);
            }
            else if (IntrinsicSupport.Sse)
            {
                //VectorF mul = Multiply(left, right);

                throw new NotImplementedException();
            }

            return Vector3DotProductSoftware(left, right);

            static VectorF Vector3DotProductSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                VectorF mul = Multiply(left, right);
                float result = X(mul) + Y(mul) + Z(mul);
                return Vector128.Create(result);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Vector4DotProduct(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse41)
            {
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse41.DotProduct(left, right, control);
            }
            else if (IntrinsicSupport.Sse3)
            {
                throw new NotImplementedException();
            }

            return Vector4DotProductSoftware(left, right);

            static VectorF Vector4DotProductSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Vector128.Create(
                    X(left) * X(right)
                    + Y(left) * Y(right)
                    + Z(left) * Z(right)
                    + W(left) * W(right)
                );
            }
        }

        private static readonly VectorF MaskWToZero = Vector128.Create(-1, -1, -1, 0).AsSingle();

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF Vector3CrossProduct(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            if (IntrinsicSupport.Sse)
            {
                #region Comments

                /* Cross product of A(x, y, z, _) and B(x, y, z, _) is
                 *                    0  1  2  3        0  1  2  3
                 *
                 * '(X = (Ay * Bz) - (Az * By), Y = (Az * Bx) - (Ax * Bz), Z = (Ax * By) - (Ay * Bx)'
                 *           1           2              1           2              1            2
                 * So we can do (Ay, Az, Ax, _) * (Bz, Bx, By, _) (last elem is irrelevant, as this is for Vector3)
                 * which leaves us with a of the first subtraction element for each (marked 1 above)
                 * Then we repeat with the right hand of subtractions (Az, Ax, Ay, _) * (By, Bz, Bx, _)
                 * which leaves us with the right hand sides (marked 2 above)
                 * Then we subtract them to get the correct vector
                 * We then mask out W to zero, because that is required for the Vector3 representation
                 *
                 * We perform the first 2 multiplications by shuffling the vectors and then multiplying them
                 * Helpers.Shuffle is the same as the C++ macro _MM_SHUFFLE, and you provide the order you wish the elements
                 * to be in *reversed* (no clue why), so here (3, 0, 2, 1) means you have the 2nd elem (1, 0 indexed) in the first slot,
                 * the 3rd elem (2) in the next one, the 1st elem (0) in the next one, and the 4th (3, W/_, unused here) in the last reg
                 */

                #endregion

                /*
                 * lhs1 goes from x, y, z, _ to y, z, x, _
                 * rhs1 goes from x, y, z, _ to z, x, y, _
                 */

                VectorF leftHandSide1 = Sse.Shuffle(left, left, Shuffle(3, 0, 2, 1));
                VectorF rightHandSide1 = Sse.Shuffle(right, right, Shuffle(3, 1, 0, 2));

                /*
                 * lhs2 goes from x, y, z, _ to z, x, y, _
                 * rhs2 goes from x, y, z, _ to y, z, x, _
                 */


                VectorF leftHandSide2 = Sse.Shuffle(left, left, Shuffle(3, 1, 0, 2));
                VectorF rightHandSide2 = Sse.Shuffle(right, right, Shuffle(3, 0, 2, 1));

                VectorF mul1 = Multiply(leftHandSide1, rightHandSide1);

                VectorF mul2 = Multiply(leftHandSide2, rightHandSide2);

                VectorF resultNonMaskedW = Subtract(mul1, mul2);

                return And(resultNonMaskedW, MaskWToZero);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return Vector3CrossProductSoftware(left, right);

            static VectorF Vector3CrossProductSoftware(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                /* Cross product of A(x, y, z, _) and B(x, y, z, _) is
                 *
                 * '(X = (Ay * Bz) - (Az * By), Y = (Az * Bx) - (Ax * Bz), Z = (Ax * By) - (Ay * Bx)'
                 */

                return Vector128.Create(
                    Y(left) * Z(right) - Z(left) * Y(right),
                    Z(left) * X(right) - X(left) * Z(right),
                    X(left) * Y(right) - Y(left) * X(right),
                    0
                );
            }
        }

        // TODO [UsesInstructionSet(InstructionSets.Sse41)]
        [MethodImpl(MaxOpt)]
        public static VectorF Vector4CrossProduct(VectorFParam1_3 one, VectorFParam1_3 two, VectorFParam1_3 three)
        {
            throw new NotImplementedException();
            // hardware

            return Vector4CrossProductSoftware(one, two, three);

            static VectorF Vector4CrossProductSoftware(VectorFParam1_3 one, VectorFParam1_3 two, VectorFParam1_3 three)
            {
                return Vector128.Create(
                    (two.GetElement(2) * three.GetElement(3) - two.GetElement(3) * three.GetElement(2)) *
                    one.GetElement(1) -
                    (two.GetElement(1) * three.GetElement(3) - two.GetElement(3) * three.GetElement(1)) *
                    one.GetElement(2) +
                    (two.GetElement(1) * three.GetElement(2) - two.GetElement(2) * three.GetElement(1)) *
                    one.GetElement(3),
                    (two.GetElement(3) * three.GetElement(2) - two.GetElement(2) * three.GetElement(3)) *
                    one.GetElement(0) -
                    (two.GetElement(3) * three.GetElement(0) - two.GetElement(0) * three.GetElement(3)) *
                    one.GetElement(2) +
                    (two.GetElement(2) * three.GetElement(0) - two.GetElement(0) * three.GetElement(2)) *
                    one.GetElement(3),
                    (two.GetElement(1) * three.GetElement(3) - two.GetElement(3) * three.GetElement(1)) *
                    one.GetElement(0) -
                    (two.GetElement(0) * three.GetElement(3) - two.GetElement(3) * three.GetElement(0)) *
                    one.GetElement(1) +
                    (two.GetElement(0) * three.GetElement(1) - two.GetElement(1) * three.GetElement(0)) *
                    one.GetElement(3),
                    (two.GetElement(2) * three.GetElement(1) - two.GetElement(1) * three.GetElement(2)) *
                    one.GetElement(0) -
                    (two.GetElement(2) * three.GetElement(0) - two.GetElement(0) * three.GetElement(2)) *
                    one.GetElement(1) +
                    (two.GetElement(1) * three.GetElement(0) - two.GetElement(0) * three.GetElement(1)) *
                    one.GetElement(2)
                    );
            }
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Length(VectorFParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            return Sqrt(Vector3DotProduct(vector, vector));
        }

        [MethodImpl(MaxOpt)]
        public static VectorF Normalize(VectorFParam1_3 vector)
        {
            // No software fallback needed, these methods cover it
            VectorF magnitude = Length(vector);
            return Divide(vector, magnitude);
        }

        #endregion
    }
}
