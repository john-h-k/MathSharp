using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        #region Vector Maths

        private static readonly Vector128<float> SignFlip2D = Vector128.Create(int.MinValue, int.MinValue, 0, 0).AsSingle();
        private static readonly Vector128<float> SignFlip3D = Vector128.Create(int.MinValue, int.MinValue, int.MinValue, 0).AsSingle();
        private static readonly Vector128<float> SignFlip4D = Vector128.Create(int.MinValue, int.MinValue, int.MinValue, int.MinValue).AsSingle();

        #region Normalize

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
                Vector4F result = Sse.And(mul, MaskZW);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                Vector4F dp = Sse3.MoveLowAndDuplicate(result);
                return Sse.Divide(vector, Sse.Sqrt(dp));
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));

                return Sse.Divide(vector, Sse.Sqrt(mul));
            }
            #endregion

            return Normalize2D_Software(vector);
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
                Vector4F result = Sse.And(mul, MaskW);

                // Doubly horizontally adding fills the final vector with the sum
                result = Vector.HorizontalAdd(result, result);
                return Sse.Divide(vector, Sse.Sqrt(Vector.HorizontalAdd(result, result)));
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(vector, vector);


                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Divide(vector, Sse.Sqrt(Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0))));
            }

            return Normalize3D_Software(vector);
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
                copy = Sse.Shuffle(copy, mul, Helpers.Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Helpers.Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Divide(vector, Sse.Sqrt(Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 2, 2, 2))));
            }

            return Normalize4D_Software(vector);
        }

        #endregion

        #region Length

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
                Vector4F result = Sse.And(mul, MaskZW);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                Vector4F dp = Sse3.MoveLowAndDuplicate(result);
                return Sse.Sqrt(dp);
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(vector, vector);

                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));

                return Sse.Sqrt(mul);
            }
            #endregion

            return Length2D_Software(vector);
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
                Vector4F result = Sse.And(mul, MaskW);

                // Doubly horizontally adding fills the final vector with the sum
                result = Vector.HorizontalAdd(result, result);
                return Sse.Sqrt(Vector.HorizontalAdd(result, result));
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(vector, vector);


                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Sqrt(Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0)));
            }

            return Length3D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F Length4D(Vector4FParam1_3 vector)
        {
            if (Sse41.IsSupported)
            {
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse.Sqrt(Sse41.DotProduct(vector, vector, control));
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
                copy = Sse.Shuffle(copy, mul, Helpers.Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Helpers.Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Sqrt(Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 2, 2, 2)));
            }

            return Length4D_Software(vector);
        }

        #endregion

        #region LengthSquared

        // TODO investigate codegen for LengthSquared_D as there have been inlining codegen issues before
        [MethodImpl(MaxOpt)]
        public static Vector4F LengthSquared2D(Vector4FParam1_3 vector)
        {
            return DotProduct2D(vector, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F LengthSquared3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return DotProduct3D(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F LengthSquared4D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            return DotProduct4D(left, right);
        }

        #endregion

        #region DotProduct

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
                Vector4F result = Sse.And(mul, MaskZW);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                return Sse3.MoveLowAndDuplicate(result);
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(left, right);

                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));

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
                Vector4F result = Sse.And(mul, MaskW);

                // Doubly horizontally adding fills the final vector with the sum
                result = Vector.HorizontalAdd(result, result);
                return Vector.HorizontalAdd(result, result);
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(left, right);

                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));
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
                copy = Sse.Shuffle(copy, mul, Helpers.Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Helpers.Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 2, 2, 2));
            }

            return DotProduct4D_Software(left, right);
        }

        #endregion

        #region CrossProduct

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
                Vector4F permute = Sse.Shuffle(right, right, Helpers.Shuffle(0, 1, 0, 1));

                // Multiply A(x, y, ?, ?) by B(y, x, y, x)
                // Resulting in (Ax * By, Ay * Bx, ?, ?)
                permute = Sse.Multiply(left, permute);

                // Create a vector of (Ay * Bx, ?, ?, ?, ?)
                Vector4F temp = Sse.Shuffle(permute, permute, Helpers.Shuffle(1, 0, 0, 0));

                // Subtract it to get ((Ax * By) - (Ay * Bx), ?, ?, ?) the desired result
                permute = Sse.Subtract(permute, temp);

                // Fill the vector with it (like DotProduct)
                return Sse.Shuffle(permute, permute, Helpers.Shuffle(0, 0, 0, 0));
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

                Vector4F leftHandSide1 = Sse.Shuffle(left, left, Helpers.Shuffle(3, 0, 2, 1));
                Vector4F rightHandSide1 = Sse.Shuffle(right, right, Helpers.Shuffle(3, 1, 0, 2));

                /*
                 * lhs2 goes from x, y, z, _ to z, x, y, _
                 * rhs2 goes from x, y, z, _ to y, z, x, _
                 */


                Vector4F leftHandSide2 = Sse.Shuffle(left, left, Helpers.Shuffle(3, 1, 0, 2));
                Vector4F rightHandSide2 = Sse.Shuffle(right, right, Helpers.Shuffle(3, 0, 2, 1));

                Vector4F mul1 = Sse.Multiply(leftHandSide1, rightHandSide1);

                Vector4F mul2 = Sse.Multiply(leftHandSide2, rightHandSide2);

                Vector4F resultNonMaskedW = Sse.Subtract(mul1, mul2);

                return Sse.And(resultNonMaskedW, MaskW);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return CrossProduct3D_Software(left, right);
        }

        // TODO [UsesInstructionSet(InstructionSets.Sse41)]
        [MethodImpl(MaxOpt)]
        public static Vector4F CrossProduct4D(Vector4FParam1_3 one, Vector4FParam1_3 two, Vector4FParam1_3 three)
        {
#warning Needs to be hardware accelerated ASAP
            // hardware

            return CrossProduct4D_Software(one, two, three);
        }

        #endregion

        #region Distance


        [MethodImpl(MaxOpt)]
        public static Vector4F Distance2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);

                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                return Sse.Sqrt(Sse41.DotProduct(diff, diff, control));
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);

                Vector4F mul = Sse.Multiply(diff, diff);

                // Set W and Z to zero
                Vector4F result = Sse.And(mul, MaskZW);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                return Sse.Sqrt(Sse3.MoveLowAndDuplicate(result));
            }
            else if (Sse.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);

                Vector4F mul = Sse.Multiply(diff, diff);

                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));

                return Sse.Sqrt(mul);
            }

            return Distance2D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Distance3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 3 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0111_1111;
                return Sse.Sqrt(Sse41.DotProduct(left, right, control));
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(left, right);

                // Set W to zero
                Vector4F result = Sse.And(mul, MaskW);

                // Doubly horizontally adding fills the final vector with the sum
                result = Vector.HorizontalAdd(result, result);
                return Sse.Sqrt(Vector.HorizontalAdd(result, result));
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(left, right);

                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Sqrt(Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0)));
            }

            return Distance3D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F Distance4D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse41.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse.Sqrt(Sse41.DotProduct(diff, diff, control));
            }
            else if (Sse3.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);
                Vector4F mul = Sse.Multiply(diff, diff);
                mul = Sse3.HorizontalAdd(mul, mul);
                return Sse.Sqrt(Sse3.HorizontalAdd(mul, mul));
            }
            else if (Sse.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);
                Vector4F copy = diff;
                Vector4F mul = Sse.Multiply(diff, copy);
                copy = Sse.Shuffle(copy, mul, Helpers.Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Helpers.Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Sqrt(Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 2, 2, 2)));
            }

            return Distance4D_Software(left, right);
        }

        #endregion

        #region DistanceSquared

        [MethodImpl(MaxOpt)]
        public static Vector4F DistanceSquared2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);

                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                return Sse41.DotProduct(diff, diff, control);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);

                Vector4F mul = Sse.Multiply(diff, diff);

                // Set W and Z to zero
                Vector4F result = Sse.And(mul, MaskZW);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                return Sse3.MoveLowAndDuplicate(result);
            }
            else if (Sse.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);

                Vector4F mul = Sse.Multiply(diff, diff);

                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));

                return mul;
            }

            return DistanceSquared2D_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4F DistanceSquared3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
                Vector4F result = Sse.And(mul, MaskW);

                // Doubly horizontally adding fills the final vector with the sum
                result = Vector.HorizontalAdd(result, result);
                return Vector.HorizontalAdd(result, result);
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(left, right);

                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                return Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));
            }

            return DistanceSquared3D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4F DistanceSquared4D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            if (Sse41.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse41.DotProduct(diff, diff, control);
            }
            else if (Sse3.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);
                Vector4F mul = Sse.Multiply(diff, diff);
                mul = Sse3.HorizontalAdd(mul, mul);
                return Sse3.HorizontalAdd(mul, mul);
            }
            else if (Sse.IsSupported)
            {
                Vector4F diff = Sse.Subtract(left, right);
                Vector4F copy = diff;
                Vector4F mul = Sse.Multiply(diff, copy);
                copy = Sse.Shuffle(copy, mul, Helpers.Shuffle(1, 0, 0, 0));
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, Helpers.Shuffle(0, 3, 0, 0));
                mul = Sse.AddScalar(mul, copy);

                return Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 2, 2, 2));
            }

            return DistanceSquared4D_Software(left, right);
        }

        #endregion

        #region Lerp

        [MethodImpl(MaxOpt)]
        public static Vector4F Lerp(Vector4FParam1_3 from, Vector4FParam1_3 to, float weight)
        {
            Debug.Assert(weight <= 1 && weight >= 0);


            if (Sse.IsSupported)
            {
                // Lerp (Linear interpolate) interpolates between two values (here, vectors)
                // The general formula for it is 'from + (to - from) * weight'
                Vector4F offset = Sse.Subtract(to, from);
                offset = Sse.Multiply(offset, weight.LoadScalarBroadcast());
                return Sse.Add(from, offset);
            }

            return Lerp_Software(from, to, weight);
        }

        #endregion

        #region Reflect

        public static Vector4F Reflect2D(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector4F tmp = Sse41.DotProduct(incident, normal, control);
                tmp = Sse.Add(tmp, tmp);
                tmp = Sse.Multiply(tmp, normal);
                return Sse.Subtract(incident,  tmp);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(incident, normal);

                // Set W and Z to zero
                Vector4F result = Sse.And(mul, MaskZW);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                Vector4F tmp = Sse3.MoveLowAndDuplicate(result);
                tmp = Sse.Add(tmp, tmp);
                tmp = Sse.Multiply(tmp, normal);
                return Sse.Subtract(incident, tmp);
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(incident, normal);

                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));

                mul = Sse.Add(mul, mul);
                mul = Sse.Multiply(mul, normal);
                return Sse.Subtract(incident, mul);
            }

            return Reflect2D_Software(incident, normal);
        }

        public static Vector4F Reflect3D(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            // SSE4.1 has a native dot product instruction, dpps
            if (Sse41.IsSupported)
            {
                // This multiplies the first 3 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0111_1111;
                Vector4F tmp = Sse41.DotProduct(incident, normal, control);
                tmp = Sse.Add(tmp, tmp);
                tmp = Sse.Multiply(tmp, normal);
                return Sse.Subtract(incident, tmp);
            }
            // We can use SSE to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            // on SSE3 vs SSE1
            else if (Sse3.IsSupported)
            {
                Vector4F mul = Sse.Multiply(incident, normal);

                // Set W to zero
                Vector4F result = Sse.And(mul, MaskW);

                // Doubly horizontally adding fills the final vector with the sum
                result = Vector.HorizontalAdd(result, result);
                Vector4F tmp = Vector.HorizontalAdd(result, result);
                tmp = Sse.Add(tmp, tmp);
                tmp = Sse.Multiply(tmp, normal);
                return Sse.Subtract(incident, tmp);

            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(incident, normal);

                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, Helpers.Shuffle(2, 1, 2, 1));

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, Helpers.Shuffle(1, 1, 1, 1));

                mul = Sse.AddScalar(mul, temp);

                Vector4F tmp = Sse.Shuffle(mul, mul, Helpers.Shuffle(0, 0, 0, 0));
                tmp = Sse.Add(tmp, tmp);
                tmp = Sse.Multiply(tmp, normal);
                return Sse.Subtract(incident, tmp);
            }

            return Reflect3D_Software(incident, normal);
        }

        public static Vector4F Reflect4D(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4F tmp = DotProduct4D_Software(incident, normal);
            tmp = Multiply_Software(tmp, tmp);
            tmp = Multiply_Software(tmp, normal);
            return Subtract_Software(incident, tmp);
        }

        #endregion

        #endregion
    }
}
