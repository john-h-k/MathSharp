using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.Helpers;
using static MathSharp.VectorFloat.Arithmetic;
using static MathSharp.VectorF.BitOperations;
using static MathSharp.VectorF.Conversion;

namespace MathSharp.VectorF
{
    using VectorF = Vector128<float>;
    using VectorFParam1_3 = Vector128<float>;
    using VectorFWide = Vector256<float>;
    public static class VectorOperations
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Vector Maths

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF DotProduct2D(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (IntrinsicSupport.Sse41)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                return Sse41.DotProduct(left, right, control);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (IntrinsicSupport.Sse3)
            {
                VectorF mul = PerElementMultiply(left, right);

                // Set W and Z to zero
                VectorF result = And(mul, MaskWAndZToZero);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                return Sse3.MoveLowAndDuplicate(result);
            }
            else if (IntrinsicSupport.Sse)
            {
                VectorF mul = PerElementMultiply(left, right);

                mul = HorizontalAdd(mul, mul);
                return ScalarToVector(mul);
            }

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                VectorF mul = PerElementMultiply(left, right);
                float result = X(mul) + Y(mul);
                return Vector128.Create(result);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF DotProduct3D(VectorFParam1_3 left, VectorFParam1_3 right)
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
                VectorF mul = PerElementMultiply(left, right);

                // Set W to zero
                VectorF result = And(mul, MaskWToZero);

                // Doubly horizontally adding fills the final vector with the sum
                result = HorizontalAdd(result, result);
                return HorizontalAdd(result, result);
            }
            else if (IntrinsicSupport.Sse)
            {
                VectorF mul = PerElementMultiply(left, right);

                throw new NotImplementedException();
            }

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                VectorF mul = PerElementMultiply(left, right);
                float result = X(mul) + Y(mul) + Z(mul);
                return Vector128.Create(result);
            }
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF DotProduct4D(VectorFParam1_3 left, VectorFParam1_3 right)
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

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
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
        private static readonly VectorF MaskWAndZToZero = Vector128.Create(-1, -1, 0, 0).AsSingle();

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF CrossProduct2D(VectorFParam1_3 left, VectorFParam1_3 right)
        {
            throw new NotImplementedException();

            // hardware

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
            {
                return Conversion.LoadScalarBroadcast(X(left) * Y(right) - Y(left) * X(right));
            }
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static VectorF CrossProduct3D(VectorFParam1_3 left, VectorFParam1_3 right)
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

                VectorF mul1 = PerElementMultiply(leftHandSide1, rightHandSide1);

                VectorF mul2 = PerElementMultiply(leftHandSide2, rightHandSide2);

                VectorF resultNonMaskedW = Subtract(mul1, mul2);

                return And(resultNonMaskedW, MaskWToZero);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return SoftwareFallback(left, right);

            static VectorF SoftwareFallback(VectorFParam1_3 left, VectorFParam1_3 right)
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
        public static VectorF CrossProduct4D(VectorFParam1_3 one, VectorFParam1_3 two, VectorFParam1_3 three)
        {
            throw new NotImplementedException();
            // hardware

            return SoftwareFallback(one, two, three);

            static VectorF SoftwareFallback(VectorFParam1_3 one, VectorFParam1_3 two, VectorFParam1_3 three)
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
            return Sqrt(DotProduct3D(vector, vector));
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
