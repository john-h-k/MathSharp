using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector2D = Vector128<double>;
    using Vector4DParam1_3 = Vector256<double>;

    public static partial class Vector
    {
        #region Vector Maths

        private static readonly Vector256<double> SignFlip2DDouble = Vector256.Create(long.MinValue, long.MinValue, 0, 0).AsDouble();
        private static readonly Vector256<double> SignFlip3DDouble = Vector256.Create(long.MinValue, long.MinValue, long.MinValue, 0).AsDouble();
        private static readonly Vector256<double> SignFlip4DDouble = Vector256.Create(long.MinValue, long.MinValue, long.MinValue, long.MinValue).AsDouble();

        #region Normalize

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize2D(in Vector4DParam1_3 vector)
        {
            // SSE4.1 has a native dot product instruction, dppd
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector2D dp = Sse41.DotProduct(vector.GetLower(), vector.GetLower(), control);

                return Sse2.Divide(vector.GetLower(), Sse2.Sqrt(dp)).ToVector256();
            }
            else if (Sse3.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(vector.GetLower(), vector.GetLower());
                return Sse2.Divide(vector.GetLower(), Sse2.Sqrt(Sse3.HorizontalAdd(tmp, tmp))).ToVector256();
            }
            else if (Sse2.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(vector.GetLower(), vector.GetLower());
                Vector2D shuf = Sse2.Shuffle(tmp, tmp, ShuffleValues._0_1_0_1);
                return Sse2.Divide(vector.GetLower(), Sse2.Sqrt(Sse2.Add(tmp, shuf))).ToVector256();
            }

            return Normalize2D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize3D(in Vector4DParam1_3 vector)
        {
            // We can use AVX to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            if (Avx.IsSupported)
            {
                Vector4D mul = Avx.Multiply(vector, vector);

                // Set W to zero
                Vector4D result = Avx.And(mul, MaskWDouble);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                // Dot done

                return Avx.Divide(vector, Avx.Sqrt(result));
            }

            return Normalize3D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize4D(in Vector4DParam1_3 vector)
        {
            if (Avx.IsSupported)
            {
                Vector4D result = Avx.Multiply(vector, vector);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return Avx.Divide(vector, Avx.Sqrt(result));
            }

            return Normalize4D_Software(vector);
        }

        #endregion

        #region Length

        [MethodImpl(MaxOpt)]
        public static Vector4D Length2D(in Vector4DParam1_3 vector)
        {
            // SSE4.1 has a native dot product instruction, dppd
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector2D dp = Sse41.DotProduct(vector.GetLower(), vector.GetLower(), control);

                return Helpers.DuplicateToVector256(Sse2.Sqrt(dp));
            }
            else if (Sse3.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(vector.GetLower(), vector.GetLower());
                return Helpers.DuplicateToVector256(Sse2.Sqrt(Sse3.HorizontalAdd(tmp, tmp)));
            }
            else if (Sse2.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(vector.GetLower(), vector.GetLower());
                Vector2D shuf = Sse2.Shuffle(tmp, tmp, ShuffleValues._0_1_0_1);
                return Helpers.DuplicateToVector256(Sse2.Sqrt(Sse2.Add(tmp, shuf)));
            }

            return Length2D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Length3D(in Vector4DParam1_3 vector)
        {
            // We can use AVX to vectorize the multiplication
            // There are different fastest methods to sum the resultant vector
            if (Avx.IsSupported)
            {
                Vector4D mul = Avx.Multiply(vector, vector);

                // Set W to zero
                Vector4D result = Avx.And(mul, MaskWDouble);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return Avx.Sqrt(result);
            }

            return Length3D_Software(vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D Length4D(in Vector4DParam1_3 vector)
        {
            if (Avx.IsSupported)
            {
                Vector4D result = Avx.Multiply(vector, vector);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return Avx.Sqrt(result);
            }

            return Length4D_Software(vector);
        }

        #endregion

        #region LengthSquared

        // TODO investigate codegen for LengthSquared_D as there have been inlining codegen issues before
        [MethodImpl(MaxOpt)]
        public static Vector4D LengthSquared2D(in Vector4DParam1_3 vector)
        {
            return DotProduct2D(vector, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D LengthSquared3D(in Vector4DParam1_3 vector)
        {
            return DotProduct3D(vector, vector);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D LengthSquared4D(in Vector4DParam1_3 vector)
        {
            return DotProduct4D(vector, vector);
        }

        #endregion

        #region DotProduct

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Sse3 | InstructionSets.Sse2)]
        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct2D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dppd
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector2D dp = Sse41.DotProduct(left.GetLower(), right.GetLower(), control);

                return Helpers.DuplicateToVector256(dp);
            }
            else if (Sse3.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(left.GetLower(), right.GetLower());
                return Helpers.DuplicateToVector256(Sse3.HorizontalAdd(tmp, tmp));
            }
            else if (Sse2.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(left.GetLower(), right.GetLower());
                Vector2D shuf = Sse2.Shuffle(tmp, tmp, ShuffleValues._0_1_0_1);
                return Helpers.DuplicateToVector256(Sse2.Add(tmp, shuf));
            }

            return DotProduct2D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct3D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            // We can use AVX to vectorize the multiplication
            if (Avx.IsSupported)
            {
                Vector4D mul = Avx.Multiply(left, right);

                // Set W to zero
                Vector4D result = Avx.And(mul, MaskWDouble);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return result;
            }

            return DotProduct3D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4D DotProduct4D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                Vector256<double> result = Avx.Multiply(left, right);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return result;
            }

            return DotProduct4D_Software(left, right);
        }

        #endregion

        #region CrossProduct

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct2D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            /* Cross product of A(x, y, _, _) and B(x, y, _, _) is
             * 'E = (Ax * By) - (Ay * Bx)'
             * 'E'. We expand this (like with DotProduct) to the whole vector
             */

            if (Avx.IsSupported)
            {
                // Transform B(x, y, ?, ?) to (y, x, y, x)
                Vector4D permute = Avx.Shuffle(right, right, ShuffleValues._0_1_0_1);

                // Multiply A(x, y, ?, ?) by B(y, x, y, x)
                // Resulting in (Ax * By, Ay * Bx, ?, ?)
                permute = Avx.Multiply(left, permute);

                // Create a vector of (Ay * Bx, ?, ?, ?, ?)
                Vector4D temp = Avx.Shuffle(permute, permute, ShuffleValues._0_0_0_1);

                // Subtract it to get ((Ax * By) - (Ay * Bx), ?, ?, ?) the desired result
                permute = Avx.Subtract(permute, temp);

                // Fill the vector with it (like DotProduct)
                return Avx.Shuffle(permute, permute, ShuffleValues._0_0_0_0);
            }

            return CrossProduct2D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct3D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx2.IsSupported)
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

                Vector4D leftHandSide1 = Avx2.Permute4x64(left, ShuffleValues._3_0_2_1);
                Vector4D rightHandSide1 = Avx2.Permute4x64(right, ShuffleValues._3_1_0_2);

                /*
                 * lhs2 goes from x, y, z, _ to z, x, y, _
                 * rhs2 goes from x, y, z, _ to y, z, x, _
                 */

                Vector4D leftHandSide2 = Avx2.Permute4x64(left, ShuffleValues._3_1_0_2);
                Vector4D rightHandSide2 = Avx2.Permute4x64(right, ShuffleValues._3_0_2_1);

                Vector4D mul1 = Avx.Multiply(leftHandSide1, rightHandSide1);

                Vector4D mul2 = Avx.Multiply(leftHandSide2, rightHandSide2);

                Vector4D resultNonMaskedW = Avx.Subtract(mul1, mul2);

                return Avx.And(resultNonMaskedW, MaskWDouble);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return CrossProduct3D_Software(left, right);
        }

        // TODO [UsesInstructionSet(InstructionSets.Sse41)]
        [MethodImpl(MaxOpt)]
        public static Vector4D CrossProduct4D(in Vector4DParam1_3 one, in Vector4DParam1_3 two, in Vector4DParam1_3 three)
        {
#warning Needs to be hardware accelerated ASAP
            // hardware

            return CrossProduct4D_Software(one, two, three);
        }

        #endregion

        #region Distance


        [MethodImpl(MaxOpt)]
        public static Vector4D Distance2D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dppd
            if (Sse41.IsSupported)
            {
                Vector2D diff = Sse2.Subtract(left.GetLower(), right.GetLower());
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector2D dp = Sse41.DotProduct(diff, diff, control);

                return Helpers.DuplicateToVector256(Sse2.Sqrt(dp));
            }
            else if (Sse3.IsSupported)
            {
                Vector2D diff = Sse2.Subtract(left.GetLower(), right.GetLower());
                Vector2D tmp = Sse2.Multiply(diff, diff);
                return Helpers.DuplicateToVector256(Sse2.Sqrt(Sse3.HorizontalAdd(tmp, tmp)));
            }
            else if (Sse2.IsSupported)
            {
                Vector2D tmp = Sse2.Subtract(left.GetLower(), right.GetLower());
                tmp = Sse2.Multiply(tmp, tmp);
                Vector2D shuf = Sse2.Shuffle(tmp, tmp, ShuffleValues._0_1_0_1);
                return Helpers.DuplicateToVector256(Sse2.Sqrt(Sse2.Add(tmp, shuf)));
            }

            return Distance2D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Distance3D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                Vector4D diff = Avx.Subtract(left, right);
                Vector4D mul = Avx.Multiply(diff, diff);

                // Set W to zero
                Vector4D result = Avx.And(mul, MaskWDouble);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return Avx.Sqrt(result);
            }

            return Distance3D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Distance4D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                Vector4D diff = Avx.Subtract(left, right);
                Vector256<double> result = Avx.Multiply(diff, diff);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return Avx.Sqrt(result);
            }

            return Distance4D_Software(left, right);
        }

        #endregion

        #region DistanceSquared

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared2D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            // SSE4.1 has a native dot product instruction, dppd
            if (Sse41.IsSupported)
            {
                Vector2D diff = Sse2.Subtract(left.GetLower(), right.GetLower());
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector2D dp = Sse41.DotProduct(diff, diff, control);

                return Helpers.DuplicateToVector256(dp);
            }
            else if (Sse3.IsSupported)
            {
                Vector2D diff = Sse2.Subtract(left.GetLower(), right.GetLower());
                Vector2D tmp = Sse2.Multiply(diff, diff);
                return Helpers.DuplicateToVector256(Sse3.HorizontalAdd(tmp, tmp));
            }
            else if (Sse2.IsSupported)
            {
                Vector2D tmp = Sse2.Subtract(left.GetLower(), right.GetLower());
                tmp = Sse2.Multiply(tmp, tmp);
                Vector2D shuf = Sse2.Shuffle(tmp, tmp, ShuffleValues._0_1_0_1);
                return Helpers.DuplicateToVector256(Sse2.Add(tmp, shuf));
            }

            return DistanceSquared2D_Software(left, right);
        }

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared3D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                Vector4D diff = Avx.Subtract(left, right);
                Vector256<double> mul = Avx.Multiply(diff, diff);

                // Set W to zero
                Vector256<double> result = Avx.And(mul, MaskWDouble);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return result;
            }

            return DistanceSquared3D_Software(left, right);
        }

        [UsesInstructionSet(InstructionSets.Sse41 | InstructionSets.Avx | InstructionSets.Sse)]
        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared4D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
        {
            if (Avx.IsSupported)
            {
                Vector4D diff = Avx.Subtract(left, right);
                Vector256<double> result  = Avx.Multiply(diff, diff);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return result;
            }

            return DistanceSquared4D_Software(left, right);
        }

        #endregion

        #region Lerp

        [MethodImpl(MaxOpt)]
        public static Vector4D Lerp(in Vector4DParam1_3 from, in Vector4DParam1_3 to, double weight)
        {
            Debug.Assert(weight <= 1 && weight >= 0);


            if (Avx.IsSupported)
            {
                // Lerp (Linear interpolate) interpolates between two values (here, vectors)
                // The general formula for it is 'from + (to - from) * weight'
                Vector4D offset = Avx.Subtract(to, from);
                offset = Avx.Multiply(offset, Vector256.Create(weight));
                return Avx.Add(from, offset);
            }

            return Lerp_Software(from, to, weight);
        }

        #endregion

        #region Reflect

        public static Vector4D Reflect2D(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            // SSE4.1 has a native dot product instruction, dppd
            if (Sse41.IsSupported)
            {
                // This multiplies the first 2 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_0011_1111;
                Vector2D dp = Sse41.DotProduct(incident.GetLower(), normal.GetLower(), control);
                dp = Sse2.Add(dp, dp);
                dp = Sse2.Multiply(dp, normal.GetLower());
                return Sse2.Subtract(incident.GetLower(), dp).ToVector256();
            }
            else if (Sse3.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(incident.GetLower(), normal.GetLower());
                tmp = Sse3.HorizontalAdd(tmp, tmp);
                tmp = Sse2.Add(tmp, tmp);
                tmp = Sse2.Multiply(tmp, normal.GetLower());
                return Sse2.Subtract(incident.GetLower(), tmp).ToVector256();
            }
            else if (Sse2.IsSupported)
            {
                Vector2D tmp = Sse2.Multiply(incident.GetLower(), normal.GetLower());
                Vector2D shuf = Sse2.Shuffle(tmp, tmp, ShuffleValues._0_1_0_1);
                tmp = Sse2.Add(tmp, shuf);
                tmp = Sse2.Add(tmp, tmp);
                tmp = Sse2.Multiply(tmp, normal.GetLower());
                return Sse2.Subtract(incident.GetLower(), tmp).ToVector256();
            }

            return Reflect2D_Software(incident, normal);
        }

        public static Vector4D Reflect3D(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            if (Avx.IsSupported)
            {
                Vector256<double> mul = Avx.Multiply(incident, normal);

                // Set W to zero
                Vector256<double> result = Avx.And(mul, MaskWDouble);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                // Dot done

                result = Avx.Add(result, result);
                result = Avx.Multiply(result, normal);
                return Avx.Subtract(incident, result);
            }

            return Reflect3D_Software(incident, normal);
        }

        public static Vector4D Reflect4D(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector256<double> result = Avx.Multiply(incident, normal);

            // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
            result = Avx.HorizontalAdd(result, result);

            // Now we have (X + Y, X + Y, Z + 0, Z + 0)
            result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
            // We switch the 2 halves, and add that to the original, getting the result in all elems

            result = Avx.Add(result, result);
            result = Avx.Multiply(result, normal);
            return Avx.Subtract(incident, result);
        }

        #endregion

        #endregion
    }
}
