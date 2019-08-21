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
        
        public static readonly Vector256<double> MaskWDouble = Vector256.Create(-1, -1, -1, 0).AsDouble();

        public static readonly Vector256<double> UnitXDouble = Vector256.Create(1d, 0d, 0d, 0d);
        public static readonly Vector256<double> UnitYDouble = Vector256.Create(0d, 1d, 0d, 0d);
        public static readonly Vector256<double> UnitZDouble = Vector256.Create(0d, 0d, 1d, 0d);
        public static readonly Vector256<double> UnitWDouble = Vector256.Create(0d, 0d, 0d, 1d);

        public static readonly Vector256<double> OneDouble = Vector256.Create(1d, 1d, 1d, 1d);
        public static readonly Vector256<double> ZeroDouble = Vector256<double>.Zero;

        #region Normalize

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize2D(in Vector4DParam1_3 vector)
            => Divide(vector, Length2D(vector));

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize3D(in Vector4DParam1_3 vector)
            => Divide(vector, Length3D(vector));

        [MethodImpl(MaxOpt)]
        public static Vector4D Normalize4D(in Vector4DParam1_3 vector)
            => Divide(vector, Length4D(vector));

        #endregion

        #region Length

        [MethodImpl(MaxOpt)]
        public static Vector4D Length2D(in Vector4DParam1_3 vector)
            => Sqrt(DotProduct2D(vector, vector));

        [MethodImpl(MaxOpt)]
        public static Vector4D Length3D(in Vector4DParam1_3 vector)
            => Sqrt(DotProduct3D(vector, vector));

        [MethodImpl(MaxOpt)]
        public static Vector4D Length4D(in Vector4DParam1_3 vector)
            => Sqrt(DotProduct4D(vector, vector));

        #endregion

        #region LengthSquared

        [MethodImpl(MaxOpt)]
        public static Vector4D LengthSquared2D(in Vector4DParam1_3 vector)
            => DotProduct2D(vector, vector);

        [MethodImpl(MaxOpt)]
        public static Vector4D LengthSquared3D(in Vector4DParam1_3 vector)
            => DotProduct3D(vector, vector);

        [MethodImpl(MaxOpt)]
        public static Vector4D LengthSquared4D(in Vector4DParam1_3 vector)
            => DotProduct4D(vector, vector);

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

        [UsesInstructionSet(InstructionSets.None)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Distance2D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
            => Length2D(Subtract(left, right));

        [UsesInstructionSet(InstructionSets.None)]
        [MethodImpl(MaxOpt)]
        public static Vector4D Distance3D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
            => Length3D(Subtract(left, right));


        [MethodImpl(MaxOpt)]
        public static Vector4D Distance4D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
            => Length4D(Subtract(left, right));

        #endregion

        #region DistanceSquared

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared2D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
            => LengthSquared2D(Subtract(left, right));

        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared3D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
            => LengthSquared3D(Subtract(left, right));


        [MethodImpl(MaxOpt)]
        public static Vector4D DistanceSquared4D(in Vector4DParam1_3 left, in Vector4DParam1_3 right)
            => LengthSquared4D(Subtract(left, right));

        #endregion

        #region Lerp

        [MethodImpl(MaxOpt)]
        public static Vector4D Lerp(in Vector4DParam1_3 from, in Vector4DParam1_3 to, double weight)
        {
            Debug.Assert(weight <= 1 && weight >= 0);

            // Lerp (Linear interpolate) interpolates between two values (here, vectors)
            // The general formula for it is 'from + (to - from) * weight'
            Vector4D offset = Subtract(to, from);
            offset = Multiply(offset, weight.LoadScalarBroadcast());
            return Add(from, offset);
        }

        #endregion

        #region Reflect

        public static Vector4D Reflect2D(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4D tmp = DotProduct2D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        public static Vector4D Reflect3D(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4D tmp = DotProduct3D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        public static Vector4D Reflect4D(in Vector4DParam1_3 incident, in Vector4DParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4D tmp = DotProduct4D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        #endregion

        #endregion
    }
}
