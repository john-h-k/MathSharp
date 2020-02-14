using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;

namespace MathSharp
{
    
    using Vector2D = Vector128<double>;
    

    public static partial class Vector
    {
        #region Vector Maths

        // Uses the 'ones idiom', which is where
        // cmpeq xmmN, xmmN
        // is used, and the result is guaranteed to be all ones
        // it has no dependencies and is useful
        // For anyone looking at codegen, it is actually
        // [v]cmpps xmm0, xmm0, xmm0, 0x0
        // which is functionally identical
        /// <summary>
        /// Gets a new Vector256&lt;double&gt; with all elements initialized to one.
        /// </summary>
        public static Vector256<double> OneDouble
        {
            get
            {
                Vector256<double> v = ZeroDouble;
                return CompareEqual(v, v);
            }
        }

        /// <summary>
        /// Gets a new Vector256&lt;double&gt; with all elements initialized to zero.
        /// </summary>
        public static Vector256<double> ZeroDouble => Vector256<double>.Zero;

        #region Normalize

        /// <summary>
        /// Scales the Vector2D to unit length.
        /// </summary>
        /// <param name="vector">The Vector2D to normalize.</param>
        /// <returns>A normalized vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Normalize2D(Vector256<double> vector)
            => Divide(vector, Length2D(vector));

        /// <summary>
        /// Scales the Vector3D to unit length.
        /// </summary>
        /// <param name="vector">The Vector3D to normalize.</param>
        /// <returns>A normalized vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Normalize3D(Vector256<double> vector)
            => Divide(vector, Length3D(vector));

        /// <summary>
        /// Scales the Vector4D to unit length.
        /// </summary>
        /// <param name="vector">The Vector4D to normalize.</param>
        /// <returns>A normalized vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Normalize4D(Vector256<double> vector)
            => Divide(vector, Length4D(vector));

        #endregion

        #region Length
        /// <summary>
        /// Returns the length of the given Vector2D.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <returns>The length vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Length2D(Vector256<double> vector)
            => Sqrt(DotProduct2D(vector, vector));

        /// <summary>
        /// Returns the length of the given Vector3D.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <returns>The length vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Length3D(Vector256<double> vector)
            => Sqrt(DotProduct3D(vector, vector));

        /// <summary>
        /// Returns the length of the given Vector4D.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <returns>The length vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Length4D(Vector256<double> vector)
            => Sqrt(DotProduct4D(vector, vector));

        #endregion

        #region LengthSquared
        /// <summary>
        /// Returns the length of the given Vector2D squared.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <returns>The length vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> LengthSquared2D(Vector256<double> vector)
            => DotProduct2D(vector, vector);

        /// <summary>
        /// Returns the length of the given Vector3D squared.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <returns>The length vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> LengthSquared3D(Vector256<double> vector)
            => DotProduct3D(vector, vector);

        /// <summary>
        /// Returns the length of the given Vector4D squared.
        /// </summary>
        /// <param name="vector">The input vector.</param>
        /// <returns>The length vector.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> LengthSquared4D(Vector256<double> vector)
            => DotProduct4D(vector, vector);

        #endregion

        #region DotProduct

        /// <summary>
        /// Returns the dot product of the two given vectors.
        /// </summary>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns>The dot product of the two input vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> DotProduct2D(Vector256<double> left, Vector256<double> right)
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
                Vector2D shuf = Sse2.Shuffle(tmp, tmp, ShuffleValues.YXYX);

                var dot = Sse2.Add(tmp, shuf);

                return dot.ToVector256Unsafe().WithUpper(dot);
            }

            return DotProduct2D_Software(left, right);
        }
        
        /// <summary>
        /// Returns the dot product of the two given vectors.
        /// </summary>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns>The dot product of the two input vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> DotProduct3D(Vector256<double> left, Vector256<double> right)
        {
            // We can use AVX to vectorize the multiplication
            if (Avx.IsSupported)
            {
                Vector256<double> mul = Avx.Multiply(left, right);

                // Set W to zero
                Vector256<double> result = Avx.And(mul, DoubleConstants.MaskW);

                // We now have (X, Y, Z, 0) correctly, and want to add them together and fill with that result
                result = Avx.HorizontalAdd(result, result);

                // Now we have (X + Y, X + Y, Z + 0, Z + 0)
                result = Avx.Add(result, Avx.Permute2x128(result, result, 0b_0000_0001));
                // We switch the 2 halves, and add that to the original, getting the result in all elems

                return result;
            }

            return DotProduct3D_Software(left, right);
        }
        
        /// <summary>
        /// Returns the dot product of the two given vectors.
        /// </summary>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns>The dot product of the two input vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> DotProduct4D(Vector256<double> left, Vector256<double> right)
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
        
        /// <summary>
        /// Returns the cross product of the two given vectors.
        /// </summary>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns>The dot product of the two input vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CrossProduct2D(Vector256<double> left, Vector256<double> right)
        {
            /* Cross product of A(x, y, _, _) and B(x, y, _, _) is
             * 'E = (Ax * By) - (Ay * Bx)'
             * 'E'. We expand this (like with DotProduct) to the whole vector
             */

            if (Avx.IsSupported)
            {
                // Transform B(x, y, ?, ?) to (y, x, y, x)
                Vector256<double> permute = Avx.Shuffle(right, right, ShuffleValues.YXYX);

                // Multiply A(x, y, ?, ?) by B(y, x, y, x)
                // Resulting in (Ax * By, Ay * Bx, ?, ?)
                permute = Avx.Multiply(left, permute);

                // Create a vector of (Ay * Bx, ?, ?, ?, ?)
                Vector256<double> temp = Avx.Shuffle(permute, permute, ShuffleValues.YXXX);

                // Subtract it to get ((Ax * By) - (Ay * Bx), ?, ?, ?) the desired result
                permute = Avx.Subtract(permute, temp);

                // Fill the vector with it (like DotProduct)
                return Avx.Shuffle(permute, permute, ShuffleValues.XXXX);
            }

            return CrossProduct2D_Software(left, right);
        }
        
        /// <summary>
        /// Returns the cross product of the two given vectors.
        /// </summary>
        /// <param name="left">The first input vector.</param>
        /// <param name="right">The second input vector.</param>
        /// <returns>The dot product of the two input vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CrossProduct3D(Vector256<double> left, Vector256<double> right)
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

                Vector256<double> leftHandSide1 = Avx2.Permute4x64(left, ShuffleValues.YZXW);
                Vector256<double> rightHandSide1 = Avx2.Permute4x64(right, ShuffleValues.ZXYW);
                /*
                 * lhs2 goes from x, y, z, _ to z, x, y, _
                 * rhs2 goes from x, y, z, _ to y, z, x, _
                 */

                Vector256<double> leftHandSide2 = Avx2.Permute4x64(left, ShuffleValues.ZXYW);
                Vector256<double> rightHandSide2 = Avx2.Permute4x64(right, ShuffleValues.YZXW);

                Vector256<double> mul1 = Avx.Multiply(leftHandSide1, rightHandSide1);

                Vector256<double> mul2 = Avx.Multiply(leftHandSide2, rightHandSide2);

                Vector256<double> resultNonMaskedW = Avx.Subtract(mul1, mul2);

                return Avx.And(resultNonMaskedW, DoubleConstants.MaskW);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return CrossProduct3D_Software(left, right);
        }

        /// <summary>
        /// Returns the cross product of the three given vectors.
        /// </summary>
        /// <param name="one">The first input vector.</param>
        /// <param name="two">The second input vector.</param>
        /// <param name="three">The third input vector.</param>
        /// <returns>The dot product of the two input vectors.</returns>
        // TODO 
        [MethodImpl(MaxOpt)]
        public static Vector256<double> CrossProduct4D(Vector256<double> one, Vector256<double> two, Vector256<double> three)
        {
            // hardware

            return SoftwareFallbacks.CrossProduct4D_Software(one, two, three);
        }

        #endregion

        #region Distance
        /// <summary>
        /// Computes the euclidean distance between two vectors.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Distance2D(Vector256<double> left, Vector256<double> right)
            => Length2D(Subtract(left, right));
        
        /// <summary>
        /// Computes the euclidean distance between two vectors.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Distance3D(Vector256<double> left, Vector256<double> right)
            => Length3D(Subtract(left, right));
        
        /// <summary>
        /// Computes the euclidean distance between two vectors.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Distance4D(Vector256<double> left, Vector256<double> right)
            => Length4D(Subtract(left, right));

        #endregion

        #region DistanceSquared

        /// <summary>
        /// Compute the euclidean distance between two vectors squared.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> DistanceSquared2D(Vector256<double> left, Vector256<double> right)
            => LengthSquared2D(Subtract(left, right));

        /// <summary>
        /// Compute the euclidean distance between two vectors squared.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> DistanceSquared3D(Vector256<double> left, Vector256<double> right)
            => LengthSquared3D(Subtract(left, right));

        /// <summary>
        /// Compute the euclidean distance between two vectors squared.
        /// </summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> DistanceSquared4D(Vector256<double> left, Vector256<double> right)
            => LengthSquared4D(Subtract(left, right));

        #endregion

        #region Lerp

        /// <summary>
        /// Returns a new vector that is a linear blend of the two given vectors.
        /// </summary>
        /// <param name="from">The first input vector.</param>
        /// <param name="to">The second input vector.</param>
        /// <param name="weight">The blend factor. a when blend=0, b when blend=1.</param>
        /// <returns>The linear interpolated blend of the two vectors.</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Lerp(Vector256<double> from, Vector256<double> to, double weight)
        {
            Debug.Assert(weight <= 1 && weight >= 0);

            // Lerp (Linear interpolate) interpolates between two values (here, vectors)
            // The general formula for it is 'from + (to - from) * weight'
            Vector256<double> offset = Subtract(to, from);
            offset = Multiply(offset, weight.LoadScalarBroadcast());
            return Add(from, offset);
        }

        #endregion

        #region Reflect

        /// <summary>
        /// Calculates the reflection of an incident ray.
        /// </summary>
        /// <param name="incident">The incident ray's vector.</param>
        /// <param name="normal">The normal of the mirror upon which the ray is reflecting.</param>
        /// <returns>The vector of the reflected ray.</returns>
        public static Vector256<double> Reflect2D(Vector256<double> incident, Vector256<double> normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector256<double> tmp = DotProduct2D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        /// <summary>
        /// Calculates the reflection of an incident ray.
        /// </summary>
        /// <param name="incident">The incident ray's vector.</param>
        /// <param name="normal">The normal of the mirror upon which the ray is reflecting.</param>
        /// <returns>The vector of the reflected ray.</returns>
        public static Vector256<double> Reflect3D(Vector256<double> incident, Vector256<double> normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector256<double> tmp = DotProduct3D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        /// <summary>
        /// Calculates the reflection of an incident ray.
        /// </summary>
        /// <param name="incident">The incident ray's vector.</param>
        /// <param name="normal">The normal of the mirror upon which the ray is reflecting.</param>
        /// <returns>The vector of the reflected ray.</returns>
        public static Vector256<double> Reflect4D(Vector256<double> incident, Vector256<double> normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector256<double> tmp = DotProduct4D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        #endregion

        #endregion
    }
}
