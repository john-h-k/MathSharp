using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    
    

    public static partial class Vector
    {
        #region Vector Maths

        #region Normalize

        // Normalizing a Vector is
        // vector / Sqrt(Length(vector)
        // which can be estimated as
        // vector * ReciprocalSqrt(Length(vector))

        /// <summary>
        /// Normalizes the first 2 elements of <paramref name="vector"/>
        /// </summary>
        /// <param name="vector">The vector to normalize</param>
        /// <returns><paramref name="vector"/><paramref name="vector"/> with the first 2 elements normalized</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Normalize2D(Vector128<float> vector)
            => Divide(vector, Length2D(vector));

        /// <summary>
        /// Normalizes the first 3 elements of <paramref name="vector"/>
        /// </summary>
        /// <param name="vector">The vector to normalize</param>
        /// <returns><paramref name="vector"/><paramref name="vector"/> with the first 3 elements normalized</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Normalize3D(Vector128<float> vector)
            => Divide(vector, Length3D(vector));

        /// <summary>
        /// Normalizes the first 4 elements of <paramref name="vector"/>
        /// </summary>
        /// <param name="vector">The vector to normalize</param>
        /// <returns><paramref name="vector"/><paramref name="vector"/> with the first 4 elements normalized</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Normalize4D(Vector128<float> vector)
            => Divide(vector, Length4D(vector));

        #endregion

        #region NormalizeApprox

        [MethodImpl(MaxOpt)]
        public static Vector128<float> NormalizeApprox2D(Vector128<float> vector)
            => Multiply(vector, ReciprocalSqrtApprox(LengthSquared2D(vector)));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> NormalizeApprox3D(Vector128<float> vector)
            => Multiply(vector, ReciprocalSqrtApprox(LengthSquared3D(vector)));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> NormalizeApprox4D(Vector128<float> vector)
            => Multiply(vector, ReciprocalSqrtApprox(LengthSquared4D(vector)));

        #endregion

        #region Length

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Length2D(Vector128<float> vector)
            => Sqrt(Dot2D(vector, vector));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Length3D(Vector128<float> vector)
            => Sqrt(Dot3D(vector, vector));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Length4D(Vector128<float> vector)
            => Sqrt(Dot4D(vector, vector));

        #endregion

        #region LengthSquared

        [MethodImpl(MaxOpt)]
        public static Vector128<float> LengthSquared2D(Vector128<float> vector)
            => Dot2D(vector, vector);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> LengthSquared3D(Vector128<float> vector)
            => Dot3D(vector, vector);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> LengthSquared4D(Vector128<float> vector)
            => Dot4D(vector, vector);

        #endregion

        #region DotProduct

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Dot2D(Vector128<float> left, Vector128<float> right)
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
                Vector128<float> mul = Sse.Multiply(left, right);

                // Set W to zero
                Vector128<float> result = Sse.And(mul, SingleConstants.MaskW);

                // Add X and Y horizontally, leaving the vector as (X+Y, Z+0, X+Y. Z+0)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                return Sse3.MoveLowAndDuplicate(result);
            }
            else if (Sse.IsSupported)
            {
                Vector128<float> mul = Sse.Multiply(left, right);

                Vector128<float> temp = Sse.Shuffle(mul, mul, ShuffleValues.YYYY);

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, ShuffleValues.XXXX);

                return mul;
            }

            return Dot2D_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Dot3D(Vector128<float> left, Vector128<float> right)
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
                Vector128<float> mul = Sse.Multiply(left, right);

                // Set W to zero
                Vector128<float> result = Sse.And(mul, SingleConstants.MaskW);

                // Doubly horizontally adding fills the final vector with the sum
                result = Sse3.HorizontalAdd(result, result);
                return Sse3.HorizontalAdd(result, result);
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector128<float> mul = Sse.Multiply(left, right);

                // Shuffle around the values and AddScalar them
                Vector128<float> temp = Sse.Shuffle(mul, mul, ShuffleValues.YZYZ);

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, ShuffleValues.YYYY);

                mul = Sse.AddScalar(mul, temp);

                return Sse.Shuffle(mul, mul, ShuffleValues.XXXX);
            }

            return Dot3D_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Dot4D(Vector128<float> left, Vector128<float> right)
        {
            if (Sse41.IsSupported)
            {
                // This multiplies the first 4 elems of each and broadcasts it into each element of the returning vector
                const byte control = 0b_1111_1111;
                return Sse41.DotProduct(left, right, control);
            }
            else if (Sse3.IsSupported)
            {
                // Multiply the two vectors to get all the needed elements
                Vector128<float> mul = Sse.Multiply(left, right);

                // Double horizontal add is the same as broadcasting the sum of all 4
                mul = Sse3.HorizontalAdd(mul, mul);
                return Sse3.HorizontalAdd(mul, mul);
            }
            else if (Sse.IsSupported)
            {
                Vector128<float> copy = right;
                // Multiply the two vectors to get all the needed elements
                Vector128<float> mul = Sse.Multiply(left, copy);
                
                copy = Sse.Shuffle(copy, mul, ShuffleValues.XXXY);
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, ShuffleValues.XXWX);
                mul = Sse.Add(mul, copy);

                return Sse.Shuffle(mul, mul, ShuffleValues.ZZZZ);
            }

            return Dot4D_Software(left, right);
        }

        #endregion

        #region CrossProduct

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Cross2D(Vector128<float> left, Vector128<float> right)
        {
            /* Cross product of A(x, y, _, _) and B(x, y, _, _) is
             * 'E = (Ax * By) - (Ay * Bx)'
             * 'E'. We expand this (like with DotProduct) to the whole vector
             */

            if (Sse.IsSupported)
            {
                // Transform B(x, y, ?, ?) to (y, x, y, x)
                Vector128<float> permute = Sse.Shuffle(right, right, ShuffleValues.YXYX);

                // Multiply A(x, y, ?, ?) by B(y, x, y, x)
                // Resulting in (Ax * By, Ay * Bx, ?, ?)
                permute = Sse.Multiply(left, permute);

                // Create a vector of (Ay * Bx, ?, ?, ?, ?)
                Vector128<float> temp = Sse.Shuffle(permute, permute, ShuffleValues.YXXX);

                // Subtract it to get ((Ax * By) - (Ay * Bx), ?, ?, ?) the desired result
                permute = Sse.Subtract(permute, temp);

                // Fill the vector with it (like DotProduct)
                return Sse.Shuffle(permute, permute, ShuffleValues.XXXX);
            }

            return Cross2D_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Cross3D(Vector128<float> left, Vector128<float> right)
        {
            if (Sse.IsSupported)
            {

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
                 */

                /*
                 * lhs1 goes from x, y, z, _ to y, z, x, _
                 * rhs1 goes from x, y, z, _ to z, x, y, _
                 */

                Vector128<float> leftHandSide1 = Sse.Shuffle(left, left, ShuffleValues.YZXW);
                Vector128<float> rightHandSide1 = Sse.Shuffle(right, right, ShuffleValues.ZXYW);

                /*
                 * lhs2 goes from x, y, z, _ to z, x, y, _
                 * rhs2 goes from x, y, z, _ to y, z, x, _
                 */


                Vector128<float> leftHandSide2 = Sse.Shuffle(left, left, ShuffleValues.ZXYW);
                Vector128<float> rightHandSide2 = Sse.Shuffle(right, right, ShuffleValues.YZXW);

                Vector128<float> mul1 = Sse.Multiply(leftHandSide1, rightHandSide1);

                Vector128<float> mul2 = Sse.Multiply(leftHandSide2, rightHandSide2);

                Vector128<float> resultNonMaskedW = Sse.Subtract(mul1, mul2);

                return Sse.And(resultNonMaskedW, SingleConstants.MaskW);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return Cross3D_Software(left, right);
        }

        // TODO 
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Cross4D(Vector128<float> one, Vector128<float> two, Vector128<float> three)
        {
            if (Sse.IsSupported)
            {
                //float xTmp1 = z2 * w3 - w2 * z3;
                //float yTmp1 = w2 * z3 - z2 * w3;
                //float zTmp1 = y2 * w3 - w2 * y3;
                //float wTmp1 = z2 * y3 - y2 * z3;

                var shuf1 = Shuffle(two, ShuffleValues.ZWYZ);
                var shuf2 = Shuffle(three, ShuffleValues.WZWY);
                var shuf3 = Shuffle(two, ShuffleValues.WZWY);
                var shuf4 = Shuffle(three, ShuffleValues.ZWYZ);

                var tmp1 = Subtract(Multiply(shuf1, shuf2), Multiply(shuf3, shuf4));

                //float xTmp2 = y1 - (y2 * w3 - w2 * y3);
                //float yTmp2 = x1 - (w2 * x3 - x2 * w3);
                //float zTmp2 = x1 - (x2 * w3 - w2 * x3);
                //float wTmp2 = x1 - (z2 * x3 - x2 * z3);

                var shuf0 = Shuffle(one, ShuffleValues.YXXX);
                shuf1 = Shuffle(two, ShuffleValues.YWXZ);
                shuf2 = Shuffle(three, ShuffleValues.WXWX);
                shuf3 = Shuffle(two, ShuffleValues.WXWX);
                shuf4 = Shuffle(three, ShuffleValues.YWXZ);

                var tmp2 = Subtract(shuf0, Subtract(Multiply(shuf1, shuf2), Multiply(shuf3, shuf4)));

                //float xTmp3 = z1 + (y2 * z3 - z2 * y3);
                //float yTmp3 = z1 + (z2 * x3 - x2 * z3);
                //float zTmp3 = y1 + (x2 * y3 - y2 * x3);
                //float wTmp3 = y1 + (y2 * x3 - x2 * y3);

                shuf0 = Shuffle(one, ShuffleValues.ZZYY);
                shuf1 = Shuffle(two, ShuffleValues.YZXY);
                shuf2 = Shuffle(three, ShuffleValues.ZXYX);
                shuf3 = Shuffle(two, ShuffleValues.ZXYX);
                shuf4 = Shuffle(three, ShuffleValues.YZXY);

                var tmp3 = Add(shuf0, Subtract(Multiply(shuf1, shuf2), Multiply(shuf3, shuf4)));

                //float x = xTmp1 * xTmp2 * xTmp3 * w1;
                //float y = yTmp1 * yTmp2 * yTmp3 * w1;
                //float z = zTmp1 * zTmp2 * zTmp3 * w1;
                //float w = wTmp1 * wTmp2 * wTmp3 * z1;

                var result = Multiply(tmp1, tmp2);
                result = Multiply(result, tmp3);

                return Multiply(result, Shuffle(one, ShuffleValues.WWWZ));
            }

            return Cross4D_Software(one, two, three);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Cross4D_Software(Vector128<float> one, Vector128<float> two, Vector128<float> three) 
            => SoftwareFallbacks.Cross4D_Software(one, two, three);

        #endregion

        #region Distance

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Distance2D(Vector128<float> left, Vector128<float> right)
            => Length2D(Subtract(left, right));

        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Distance3D(Vector128<float> left, Vector128<float> right)
            => Length3D(Subtract(left, right));


        
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Distance4D(Vector128<float> left, Vector128<float> right)
            => Length4D(Subtract(left, right));

        #endregion

        #region DistanceSquared

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DistanceSquared2D(Vector128<float> left, Vector128<float> right)
            => LengthSquared2D(Subtract(left, right));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DistanceSquared3D(Vector128<float> left, Vector128<float> right)
            => LengthSquared3D(Subtract(left, right));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> DistanceSquared4D(Vector128<float> left, Vector128<float> right)
            => LengthSquared4D(Subtract(left, right));

        #endregion

        #region Lerp

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Lerp(Vector128<float> from, Vector128<float> to, Vector128<float> weight)
        {
            Debug.Assert(CompareLessThanOrEqual(weight, Vector128.Create(1f)).AllTrue() 
                                  && CompareGreaterThanOrEqual(weight, Vector128<float>.Zero).AllTrue());

            // Lerp (Linear interpolate) interpolates between two values (here, vectors)
            // The general formula for it is 'from + (to - from) * weight'
            Vector128<float> offset = Subtract(to, from);
            offset = Multiply(offset, weight);
            return Add(from, offset);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Lerp(Vector128<float> from, Vector128<float> to, float weight)
        {
            Debug.Assert(weight <= 1 && weight >= 0);

            // Lerp (Linear interpolate) interpolates between two values (here, vectors)
            // The general formula for it is 'from + (to - from) * weight'
            Vector128<float> offset = Subtract(to, from);
            offset = Multiply(offset, Vector128.Create(weight));
            return Add(from, offset);
        }

        #endregion

        #region Reflect

        public static Vector128<float> Reflect2D(Vector128<float> incident, Vector128<float> normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector128<float> tmp = Dot2D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        public static Vector128<float> Reflect3D(Vector128<float> incident, Vector128<float> normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector128<float> tmp = Dot3D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        public static Vector128<float> Reflect4D(Vector128<float> incident, Vector128<float> normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector128<float> tmp = Dot4D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        #endregion

        #endregion
    }
}
