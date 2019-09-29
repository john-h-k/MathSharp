using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using static MathSharp.SoftwareFallbacks;
using static MathSharp.Vector.SingleConstants;
using static MathSharp.Vector.DoubleConstants;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Vector
    {
        #region Vector Maths

        #region Normalize

        [MethodImpl(MaxOpt)]
        public static HwVector2S Normalize(in HwVector2S vector)
            => Normalize2D(vector);

        [MethodImpl(MaxOpt)]
        public static HwVector3S Normalize(in HwVector3S vector)
            => Normalize3D(vector);

        [MethodImpl(MaxOpt)]
        public static HwVector4S Normalize(in HwVector4S vector)
            => Normalize4D(vector);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Normalize2D(Vector4FParam1_3 vector)
            => Divide(vector, Length2D(vector));

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Normalize3D(Vector4FParam1_3 vector)
            => Divide(vector, Length3D(vector));

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Normalize4D(Vector4FParam1_3 vector)
            => Divide(vector, Length4D(vector));

        #endregion

        #region Length

        public static HwVector2S Length(HwVector2S vector)
            => Length2D(vector);

        public static HwVector3S Length(HwVector3S vector)
            => Length3D(vector);

        public static HwVector4S Length(HwVector4S vector)
            => Length4D(vector);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Length2D(Vector4FParam1_3 vector)
            => Sqrt(DotProduct2D(vector, vector));

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Length3D(Vector4FParam1_3 vector)
            => Sqrt(DotProduct3D(vector, vector));

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Length4D(Vector4FParam1_3 vector)
            => Sqrt(DotProduct4D(vector, vector));

        #endregion

        #region LengthSquared

        public static HwVector2S LengthSquared(HwVector2S vector)
            => LengthSquared2D(vector);

        public static HwVector3S LengthSquared(HwVector3S vector)
            => LengthSquared3D(vector);

        public static HwVector4S LengthSquared(HwVector4S vector)
            => LengthSquared4D(vector);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS LengthSquared2D(Vector4FParam1_3 vector)
            => DotProduct2D(vector, vector);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS LengthSquared3D(Vector4FParam1_3 vector)
            => DotProduct3D(vector, vector);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS LengthSquared4D(Vector4FParam1_3 vector)
            => DotProduct4D(vector, vector);

        #endregion

        #region DotProduct
        public static HwVector2S DotProduct(in HwVector2S left, in HwVector2S right) => DotProduct2D(left, right);
        public static HwVector3S DotProduct(in HwVector3S left, in HwVector3S right) => DotProduct3D(left, right);
        public static HwVector4S DotProduct(in HwVector4S left, in HwVector4S right) => DotProduct4D(left, right);

        
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS DotProduct2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
                Vector4F result = Sse.And(mul, MaskWSingle);

                // Add X and Y horizontally, leaving the vector as (X+Y, Y, X+Y. ?)
                result = Sse3.HorizontalAdd(result, result);

                // MoveLowAndDuplicate makes a new vector from (X, Y, Z, W) to (X, X, Z, Z)
                return Sse3.MoveLowAndDuplicate(result);
            }
            else if (Sse.IsSupported)
            {
                Vector4F mul = Sse.Multiply(left, right);

                Vector4F temp = Sse.Shuffle(mul, mul, ShuffleValues._1_1_1_1);

                mul = Sse.AddScalar(mul, temp);

                mul = Sse.Shuffle(mul, mul, ShuffleValues._0_0_0_0);

                return mul;
            }

            return DotProduct2D_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS DotProduct3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
                Vector4F result = Sse.And(mul, MaskWSingle);

                // Doubly horizontally adding fills the final vector with the sum
                result = Sse3.HorizontalAdd(result, result);
                return Sse3.HorizontalAdd(result, result);
            }
            else if (Sse.IsSupported)
            {
                // Multiply to get the needed values
                Vector4F mul = Sse.Multiply(left, right);

                // Shuffle around the values and AddScalar them
                Vector4F temp = Sse.Shuffle(mul, mul, ShuffleValues._2_1_2_1);

                mul = Sse.AddScalar(mul, temp);

                temp = Sse.Shuffle(temp, temp, ShuffleValues._1_1_1_1);

                mul = Sse.AddScalar(mul, temp);

                return Sse.Shuffle(mul, mul, ShuffleValues._0_0_0_0);
            }

            return DotProduct3D_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS DotProduct4D(Vector4FParam1_3 left, Vector4FParam1_3 right)
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
                Vector4F mul = Sse.Multiply(left, right);

                // Double horizontal add is the same as broadcasting the sum of all 4
                mul = Sse3.HorizontalAdd(mul, mul);
                return Sse3.HorizontalAdd(mul, mul);
            }
            else if (Sse.IsSupported)
            {
                Vector4F copy = right;
                // Multiply the two vectors to get all the needed elements
                Vector4F mul = Sse.Multiply(left, copy);
                
                copy = Sse.Shuffle(copy, mul, ShuffleValues._1_0_0_0);
                copy = Sse.Add(copy, mul);
                mul = Sse.Shuffle(mul, copy, ShuffleValues._0_3_0_0);
                mul = Sse.AddScalar(mul, copy);

                return Sse.Shuffle(mul, mul, ShuffleValues._2_2_2_2);
            }

            return DotProduct4D_Software(left, right);
        }

        #endregion

        #region CrossProduct

        public static HwVector2S CrossProduct(in HwVector2S left, in HwVector2S right) => CrossProduct2D(left, right);
        public static HwVector3S CrossProduct(in HwVector3S left, in HwVector3S right) => CrossProduct3D(left, right);
        public static HwVector4S CrossProduct(in HwVector4S one, in HwVector4S two, in HwVector4S three) => CrossProduct4D(one, two, three);

        
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS CrossProduct2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
        {
            /* Cross product of A(x, y, _, _) and B(x, y, _, _) is
             * 'E = (Ax * By) - (Ay * Bx)'
             * 'E'. We expand this (like with DotProduct) to the whole vector
             */

            if (Sse.IsSupported)
            {
                // Transform B(x, y, ?, ?) to (y, x, y, x)
                Vector4F permute = Sse.Shuffle(right, right, ShuffleValues._0_1_0_1);

                // Multiply A(x, y, ?, ?) by B(y, x, y, x)
                // Resulting in (Ax * By, Ay * Bx, ?, ?)
                permute = Sse.Multiply(left, permute);

                // Create a vector of (Ay * Bx, ?, ?, ?, ?)
                Vector4F temp = Sse.Shuffle(permute, permute, ShuffleValues._0_0_0_1);

                // Subtract it to get ((Ax * By) - (Ay * Bx), ?, ?, ?) the desired result
                permute = Sse.Subtract(permute, temp);

                // Fill the vector with it (like DotProduct)
                return Sse.Shuffle(permute, permute, ShuffleValues._0_0_0_0);
            }

            return CrossProduct2D_Software(left, right);
        }

        
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS CrossProduct3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
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

                Vector4F leftHandSide1 = Sse.Shuffle(left, left, ShuffleValues._3_0_2_1);
                Vector4F rightHandSide1 = Sse.Shuffle(right, right, ShuffleValues._3_1_0_2);

                /*
                 * lhs2 goes from x, y, z, _ to z, x, y, _
                 * rhs2 goes from x, y, z, _ to y, z, x, _
                 */


                Vector4F leftHandSide2 = Sse.Shuffle(left, left, ShuffleValues._3_1_0_2);
                Vector4F rightHandSide2 = Sse.Shuffle(right, right, ShuffleValues._3_0_2_1);

                Vector4F mul1 = Sse.Multiply(leftHandSide1, rightHandSide1);

                Vector4F mul2 = Sse.Multiply(leftHandSide2, rightHandSide2);

                Vector4F resultNonMaskedW = Sse.Subtract(mul1, mul2);

                return Sse.And(resultNonMaskedW, MaskWSingle);

                // TODO reuse vectors (minimal register usage) - potentially prevent any stack spilling
            }

            return CrossProduct3D_Software(left, right);
        }

        // TODO 
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS CrossProduct4D(Vector4FParam1_3 one, Vector4FParam1_3 two, Vector4FParam1_3 three)
        {
            // hardware

            return CrossProduct4D_Software(one, two, three);
        }

        #endregion

        #region Distance

        public static HwVector2S Distance(in HwVector2S left, in HwVector2S right)
            => Distance2D(left, right);

        public static HwVector3S Distance(in HwVector3S left, in HwVector3S right)
            => Distance3D(left, right);

        public static HwVector4S Distance(in HwVector4S left, in HwVector4S right)
            => Distance4D(left, right);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Distance2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
            => Length2D(Subtract(left, right));

        
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Distance3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
            => Length3D(Subtract(left, right));


        
        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS Distance4D(Vector4FParam1_3 left, Vector4FParam1_3 right)
            => Length4D(Subtract(left, right));

        #endregion

        #region DistanceSquared

        public static HwVector2S DistanceSquared(in HwVector2S left, in HwVector2S right)
            => DistanceSquared2D(left, right);

        public static HwVector3S DistanceSquared(in HwVector3S left, in HwVector3S right)
            => DistanceSquared3D(left, right);

        public static HwVector4S DistanceSquared(in HwVector4S left, in HwVector4S right)
            => DistanceSquared4D(left, right);

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS DistanceSquared2D(Vector4FParam1_3 left, Vector4FParam1_3 right)
            => LengthSquared2D(Subtract(left, right));

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS DistanceSquared3D(Vector4FParam1_3 left, Vector4FParam1_3 right)
            => LengthSquared3D(Subtract(left, right));

        [MethodImpl(MaxOpt)]
        internal static HwVectorAnyS DistanceSquared4D(Vector4FParam1_3 left, Vector4FParam1_3 right)
            => LengthSquared4D(Subtract(left, right));

        #endregion

        #region Lerp

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS Lerp(Vector4FParam1_3 from, Vector4FParam1_3 to, float weight)
        {
            Debug.Assert(weight <= 1 && weight >= 0);

            // Lerp (Linear interpolate) interpolates between two values (here, vectors)
            // The general formula for it is 'from + (to - from) * weight'
            Vector4F offset = Subtract(to, from);
            offset = Multiply(offset, weight.LoadScalarBroadcast());
            return Add(from, offset);
        }

        #endregion

        #region Reflect

        public static HwVector2S Reflect(in HwVector2S incident, in HwVector2S normal)
            => Reflect2D(incident, normal);

        public static HwVector3S Reflect(in HwVector3S incident, in HwVector3S normal)
            => Reflect3D(incident, normal);

        public static HwVector4S Reflect(in HwVector4S incident, in HwVector4S normal)
            => Reflect4D(incident, normal);

        internal static HwVectorAnyS Reflect2D(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4F tmp = DotProduct2D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        internal static HwVectorAnyS Reflect3D(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4F tmp = DotProduct3D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        internal static HwVectorAnyS Reflect4D(Vector4FParam1_3 incident, Vector4FParam1_3 normal)
        {
            // reflection = incident - (2 * DotProduct(incident, normal)) * normal
            Vector4F tmp = DotProduct4D(incident, normal);
            tmp = Add(tmp, tmp);
            tmp = Multiply(tmp, normal);
            return Subtract(incident, tmp);
        }

        #endregion

        #endregion
    }
}
