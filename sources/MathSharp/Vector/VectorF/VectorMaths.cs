using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using static MathSharp.Helpers;
using static MathSharp.VectorFloat.Arithmetic;
using static MathSharp.VectorFloat.Conversion;
using static MathSharp.VectorFloat.SoftwareFallbacks;

namespace MathSharp.VectorFloat
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;
    public static class VectorMaths
    {
        private const MethodImplOptions MaxOpt =
            MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        #region Vector Maths

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F DotProduct2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                return Sse41.DotProduct(left, right, control);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(left, right);

                // Set W and Z to zero
                Vector4F result = Sse.And(mul, MaskWAndZToZero);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                return Sse3.MoveLowAndDuplicate(result);
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(left, right);

                Vector4F temp = Sse.Shuffle(mul, mul, Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Shuffle(0, 0, 0, 0));

                return mul;
            }

            return DotProduct2D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F DotProduct3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 3 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0111_1111;
                return Sse41.DotProduct(left, right, control);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(left, right);

                // Set W to zero
                Vector4F result = Sse.And(mul, MaskWToZero);

                // Doubly horizontally adding fills the final vector with the sum
                result = HorizontalAdd(result, result);
                return HorizontalAdd(result, result);
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(left, right);


                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Shuffle(mul, mul, Shuffle(0, 0, 0, 0));
            }

            return DotProduct3D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F DotProduct4D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse41.IsSupported)
            {
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse41.DotProduct(left, right, control);
            }
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(left, right);
                mul = Sse3.HorizontalAdd(mul, mul);
                return Sse3.HorizontalAdd(mul, mul);
            }
            else if (Sse.IsSupported)
            {
                Vector4F copy = right;
                Vector4F mul = Sse.Multiply(left, copy);
                copy = Sse.Shuffle(copy, mul, Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Shuffle(mul, mul, Shuffle(2, 2, 2, 2));
            }

            return DotProduct4D_Software(left, right);
        }

        private static readonly Vector4F MaskWToZero = Vector128.Create(-1, -1, -1, 0).AsSingle();
        private static readonly Vector4F MaskWAndZToZero = Vector128.Create(-1, -1, 0, 0).AsSingle();

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F CrossProduct2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            /* Cross product of A(x, y, _, _) and B(x, y, _, _) is
             * 'E = (Ax * By) - (Ay * Bx)'
             * 'E'. We expand this (like with DotProduct) to the whole vector
             */

            if (Sse.IsSupported)
            {
                // Transform B(x, y, ?, ?) to (y, x, y, x)
                Vector4F permute = Sse.Shuffle(right, right, Shuffle(0, 1, 0, 1));

                // Multiply A(x, y, ?, ?) by B(y, x, y, x)
                // Resulting in (Ax * By, Ay * Bx, ?, ?)
                permute = Sse.Multiply(left, right);

                // Create a vector of (Ay * Bx, ?, ?, ?, ?)
                Vector4F temp = Sse.Shuffle(permute, permute, Shuffle(1, 0, 0, 0));

                // Subtract it to get ((Ax * By) - (Ay * Bx), ?, ?, ?) the desired result
                permute = Sse.Subtract(permute, temp);

                // Fill the vector with it (like DotProduct)
                return Sse.Shuffle(permute, permute, Shuffle(0, 0, 0, 0));
            }

            return CrossProduct2D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F CrossProduct3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse.IsSupported)
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

                Vector4F leftHandSide1 = Sse.Shuffle(left, left, Shuffle(3, 0, 2, 1));
                Vector4F rightHandSide1 = Sse.Shuffle(right, right, Shuffle(3, 1, 0, 2));

                /*
                 * lhs2 goes from x, y, z, _ to z, x, y, _
                 * rhs2 goes from x, y, z, _ to y, z, x, _
                 */


                Vector4F leftHandSide2 = Sse.Shuffle(left, left, Shuffle(3, 1, 0, 2));
                Vector4F rightHandSide2 = Sse.Shuffle(right, right, Shuffle(3, 0, 2, 1));

                Vector4F mul1 = Sse.Multiply(leftHandSide1, rightHandSide1);

                Vector4F mul2 = Sse.Multiply(leftHandSide2, rightHandSide2);

                Vector4F resultNonMaskedW = Sse.Subtract(mul1, mul2);

                return Sse.And(resultNonMaskedW, MaskWToZero);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return CrossProduct3D_Software(left, right);
        }

        // TODO [UsesInstructionSet(InstructionSets.Sse41)]
        [MethodImpl(MaxOpt)]
        public static Vector4F CrossProduct4D(Vector4FParam1_3 one, Vector4FParam1_3 two, Vector4FParam1_3 three)
        {
            throw new NotImplementedException();
            // hardware

            return CrossProduct4D_Software(one, two, three);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Length2D(Vector4FParam1_3 vector)
        {
            #region Manual Inline
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector4F dp = Sse41.DotProduct(vector, vector, control);

                return Sse.Sqrt(dp);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                // Set W and Z to zero
                Vector4F result = Sse.And(mul, MaskWAndZToZero);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                Vector4F dp = Sse3.MoveLowAndDuplicate(result);
                return Sse.Sqrt(dp);
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                Vector4F temp = Sse.Shuffle(mul, mul, Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Shuffle(0, 0, 0, 0));

                return Sse.Sqrt(mul);
            }
            #endregion

            return Length2D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Normalize2D(Vector4FParam1_3 vector)
        {
            #region Manual Inline
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector4F dp = Sse41.DotProduct(vector, vector, control);

                return Sse.Divide(vector, Sse.Sqrt(dp));
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                // Set W and Z to zero
                Vector4F result = Sse.And(mul, MaskWAndZToZero);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                Vector4F dp = Sse3.MoveLowAndDuplicate(result);
                return Sse.Divide(vector, Sse.Sqrt(dp));
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                Vector4F temp = Sse.Shuffle(mul, mul, Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Shuffle(0, 0, 0, 0));

                return Sse.Divide(vector, Sse.Sqrt(mul));
            }
            #endregion

            return Normalize2D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Length3D(Vector4FParam1_3 vector)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 3 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0111_1111;
                return Sse.Sqrt(Sse41.DotProduct(vector, vector, control));
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                // Set W to zero
                Vector4F result = Sse.And(mul, MaskWToZero);

                // Doubly horizontally adding fills the final vector with the sum
                result = HorizontalAdd(result, result);
                return Sse.Sqrt(HorizontalAdd(result, result));
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(vector, vector);


                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Sqrt(Sse.Shuffle(mul, mul, Shuffle(0, 0, 0, 0)));
            }

            return Length3D_Software(vector);
        }


        [MethodImpl(MaxOpt)]
        public static Vector4F Normalize3D(Vector4FParam1_3 vector)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 3 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0111_1111;
                return Sse.Divide(vector, Sse.Sqrt(Sse41.DotProduct(vector, vector, control)));
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                // Set W to zero
                Vector4F result = Sse.And(mul, MaskWToZero);

                // Doubly horizontally adding fills the final vector with the sum
                result = HorizontalAdd(result, result);
                return Sse.Divide(vector, Sse.Sqrt(HorizontalAdd(result, result)));
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(vector, vector);


                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Divide(vector, Sse.Sqrt(Sse.Shuffle(mul, mul, Shuffle(0, 0, 0, 0))));
            }

            return Normalize3D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Length4D(Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse41.DotProduct(vector, vector, control);
            }
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);
                mul = Sse3.HorizontalAdd(mul, mul);
                return Sse.Sqrt(Sse3.HorizontalAdd(mul, mul));
            }
            else if (Sse.IsSupported)
            {
                Vector4F copy = vector;
                Vector4F mul = Sse.Multiply(vector, copy);
                copy = Sse.Shuffle(copy, mul, Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Sqrt(Sse.Shuffle(mul, mul, Shuffle(2, 2, 2, 2)));
            }

            return Length4D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Normalize4D(Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse.Divide(vector, Sse41.DotProduct(vector, vector, control));
            }
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);
                mul = Sse3.HorizontalAdd(mul, mul);
                return Sse.Divide(vector, Sse.Sqrt(Sse3.HorizontalAdd(mul, mul)));
            }
            else if (Sse.IsSupported)
            {
                Vector4F copy = vector;
                Vector4F mul = Sse.Multiply(vector, copy);
                copy = Sse.Shuffle(copy, mul, Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Divide(vector, Sse.Sqrt(Sse.Shuffle(mul, mul, Shuffle(2, 2, 2, 2))));
            }

            return Normalize4D_Software(vector);
        }

        #endregion
    }
}
