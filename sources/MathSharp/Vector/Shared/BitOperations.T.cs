using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

// ReSharper disable InconsistentNaming

namespace MathSharp
{
    public static partial class Vector
    {
        /// <summary>
        /// Select elements from 2 vectors, <paramref name="left"/> and <paramref name="right"/>, based off of the vector <paramref name="selector"/>
        /// </summary>
        /// <typeparam name="T">The type of each element in <paramref name="left"/> and <paramref name="right"/></typeparam>
        /// <typeparam name="U">The type of each element in <paramref name="selector"/></typeparam>
        /// <param name="left">The vector where elements are chosen from if the equivalent element in <paramref name="selector"/> is false</param>
        /// <param name="right">The vector where elements are chosen from if the equivalent element in <paramref name="selector"/> is true</param>
        /// <param name="selector">The selector used to select elements from <paramref name="left"/> and <paramref name="right"/></param>
        /// <returns>A new <see cref="Vector128{T}"/> with the elements selected by <paramref name="selector"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> Select<T, U>(Vector128<T> left, Vector128<T> right, Vector128<U> selector)
            where T : struct where U : struct
            => Or(And(selector.As<U, T>(), right), AndNot(selector.As<U, T>(), left));

        /// <summary>
        /// Select the elements from <paramref name="vector"/> where the equivalent element in
        /// <paramref name="selector"/> is true
        /// </summary>
        /// <typeparam name="T">The type of each element in <paramref name="vector"/></typeparam>
        /// <typeparam name="U">The type of each element in <paramref name="selector"/></typeparam>
        /// <param name="vector">The vector to select elements from</param>
        /// <param name="selector">The vector to use to select elements from <paramref name="vector"/></param>
        /// <returns>A new <see cref="Vector128{T}"/> with the elements selected by <paramref name="selector"/> retained and the others zeroed</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> SelectWhereTrue<T, U>(Vector128<T> vector, Vector128<U> selector)
            where T : struct where U : struct
            => And(selector.As<U, T>(), vector);

        /// <summary>
        /// Select the elements from <paramref name="vector"/> where the equivalent element in
        /// <paramref name="selector"/> is false
        /// </summary>
        /// <typeparam name="T">The type of each element in <paramref name="vector"/></typeparam>
        /// <typeparam name="U">The type of each element in <paramref name="selector"/></typeparam>
        /// <param name="vector">The vector to select elements from</param>
        /// <param name="selector">The vector to use to select elements from <paramref name="vector"/></param>
        /// <returns>A new <see cref="Vector128{T}"/> with the elements selected by <paramref name="selector"/> retained and the others zeroed</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> SelectWhereFalse<T, U>(Vector128<T> vector, Vector128<U> selector)
            where T : struct where U : struct
            => AndNot(selector.As<U, T>(), vector);

        /// <summary>
        /// Perform a bitwise AND operation on 2 <see cref="Vector128{T}"/>s
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be AND'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be AND'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector128{T}"/> containing each element of <paramref name="left"/> AND'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> And<T>(Vector128<T> left, Vector128<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Sse.IsSupported)
                {
                    return Sse.And(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Sse2.IsSupported)
                {
                    return Sse2.And(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
                if (Sse.IsSupported)
                {
                    return Sse.And(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (Sse2.IsSupported)
            {
                return Sse2.And(left.AsByte(), right.AsByte()).As<byte, T>();
            }
            if (Sse.IsSupported)
            {
                return Sse.And(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.And_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise OR operation on 2 <see cref="Vector128{T}"/>s
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be OR'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be OR'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector128{T}"/> containing each element of <paramref name="left"/> OR'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> Or<T>(Vector128<T> left, Vector128<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Sse.IsSupported)
                {
                    return Sse.Or(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Sse2.IsSupported)
                {
                    return Sse2.Or(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
                if (Sse.IsSupported)
                {
                    return Sse.Or(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (Sse2.IsSupported)
            {
                return Sse2.Or(left.AsByte(), right.AsByte()).As<byte, T>();
            }
            if (Sse.IsSupported)
            {
                return Sse.Or(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.Or_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise XOR operation on 2 <see cref="Vector128{T}"/>s
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be XOR'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be XOR'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector128{T}"/> containing each element of <paramref name="left"/> XOR'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> Xor<T>(Vector128<T> left, Vector128<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Sse.IsSupported)
                {
                    return Sse.Xor(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Sse2.IsSupported)
                {
                    return Sse2.Xor(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
                if (Sse.IsSupported)
                {
                    return Sse.Xor(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (Sse2.IsSupported)
            {
                return Sse2.Xor(left.AsByte(), right.AsByte()).As<byte, T>();
            }
            if (Sse.IsSupported)
            {
                return Sse.Xor(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.Xor_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise NOT operation on <paramref name="left"/>, and then a bitwise AND operation on <paramref name="left"/> and <paramref name="right"/>
        /// Equivalent to calling <code>And(Not(left), right)</code>
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be NOT'ed, and then AND'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be AND'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector128{T}"/> containing each element of <paramref name="left"/> AND'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> AndNot<T>(Vector128<T> left, Vector128<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Sse.IsSupported)
                {
                    return Sse.AndNot(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Sse2.IsSupported)
                {
                    return Sse2.AndNot(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
                if (Sse.IsSupported)
                {
                    return Sse.AndNot(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (Sse2.IsSupported)
            {
                return Sse2.AndNot(left.AsByte(), right.AsByte()).As<byte, T>();
            }
            if (Sse.IsSupported)
            {
                return Sse.AndNot(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.AndNot_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise NOT operation on a <see cref="Vector128{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="vector">The vector which will be NOT'ed</param>
        /// <returns>A new <see cref="Vector128{T}"/> with each element of <paramref name="vector"/> after being NOT'ed</returns>
        [MethodImpl(MaxOpt)]
        public static Vector128<T> Not<T>(Vector128<T> vector) where T : struct
        {
            return Xor(vector, SingleConstants.AllBitsSet.As<float, T>());
        }

        /// <summary>
        /// Select elements from 2 vectors, <paramref name="left"/> and <paramref name="right"/>, based off of the vector <paramref name="selector"/>
        /// </summary>
        /// <typeparam name="T">The type of each element in <paramref name="left"/> and <paramref name="right"/></typeparam>
        /// <typeparam name="U">The type of each element in <paramref name="selector"/></typeparam>
        /// <param name="left">The vector where elements are chosen from if the equivalent element in <paramref name="selector"/> is false</param>
        /// <param name="right">The vector where elements are chosen from if the equivalent element in <paramref name="selector"/> is true</param>
        /// <param name="selector">The selector used to select elements from <paramref name="left"/> and <paramref name="right"/></param>
        /// <returns>A new <see cref="Vector256{T}"/> with the elements selected by <paramref name="selector"/> retained and the others zeroed</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> Select<T, U>(Vector256<T> left, Vector256<T> right, Vector256<U> selector)
            where T : struct where U : struct
            => Or(And(selector.As<U, T>(), right), AndNot(selector.As<U, T>(), left));

        /// <summary>
        /// Select the elements from <paramref name="vector"/> where the equivalent element in
        /// <paramref name="selector"/> is true
        /// </summary>
        /// <typeparam name="T">The type of each element in <paramref name="vector"/></typeparam>
        /// <typeparam name="U">The type of each element in <paramref name="selector"/></typeparam>
        /// <param name="vector">The vector to select elements from</param>
        /// <param name="selector">The vector to use to select elements from <paramref name="vector"/></param>
        /// <returns>A new <see cref="Vector256{T}"/> with the elements selected by <paramref name="selector"/> retained and the others zeroed</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> SelectWhereTrue<T, U>(Vector256<T> vector, Vector256<U> selector)
            where T : struct where U : struct
            => And(selector.As<U, T>(), vector);

        /// <summary>
        /// Select the elements from <paramref name="vector"/> where the equivalent element in
        /// <paramref name="selector"/> is false
        /// </summary>
        /// <typeparam name="T">The type of each element in <paramref name="vector"/></typeparam>
        /// <typeparam name="U">The type of each element in <paramref name="selector"/></typeparam>
        /// <param name="vector">The vector to select elements from</param>
        /// <param name="selector">The vector to use to select elements from <paramref name="vector"/></param>
        /// <returns>A new <see cref="Vector256{T}"/> with the elements selected by <paramref name="selector"/> retained and the others zeroed</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> SelectWhereFalse<T, U>(Vector256<T> vector, Vector256<U> selector)
            where T : struct where U : struct
            => AndNot(selector.As<U, T>(), vector);

        /// <summary>
        /// Perform a bitwise AND operation on 2 <see cref="Vector256{T}"/>s
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be AND'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be AND'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector256{T}"/> containing each element of <paramref name="left"/> AND'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> And<T>(Vector256<T> left, Vector256<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Avx.IsSupported)
                {
                    return Avx.And(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Avx.IsSupported)
                {
                    return Avx.And(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
            }

            if (Avx.IsSupported)
            {
                return Avx.And(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.And_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise OR operation on 2 <see cref="Vector256{T}"/>s
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be OR'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be OR'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector256{T}"/> containing each element of <paramref name="left"/> OR'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> Or<T>(Vector256<T> left, Vector256<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Avx.IsSupported)
                {
                    return Avx.Or(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Avx.IsSupported)
                {
                    return Avx.Or(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
            }

            if (Avx.IsSupported)
            {
                return Avx.Or(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.Or_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise XOR operation on 2 <see cref="Vector256{T}"/>s
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be XOR'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be XOR'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector256{T}"/> containing each element of <paramref name="left"/> XOR'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> Xor<T>(Vector256<T> left, Vector256<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Avx.IsSupported)
                {
                    return Avx.Xor(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Avx.IsSupported)
                {
                    return Avx.Xor(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
            }

            if (Avx.IsSupported)
            {
                return Avx.Xor(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.Xor_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise NOT operation on <paramref name="left"/>, and then a bitwise AND operation on <paramref name="left"/> and <paramref name="right"/>
        /// Equivalent to calling <code>And(Not(left), right)</code>
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="left">The left vector which will be NOT'ed, and then AND'ed with <paramref name="right"/></param>
        /// <param name="right">The right vector which will be AND'ed with <paramref name="left"/></param>
        /// <returns>A new <see cref="Vector256{T}"/> containing each element of <paramref name="left"/> AND'ed with the equivalent element of <paramref name="right"/></returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> AndNot<T>(Vector256<T> left, Vector256<T> right) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                if (Avx.IsSupported)
                {
                    return Avx.AndNot(left.AsSingle(), right.AsSingle()).As<float, T>();
                }
            }

            if (typeof(T) == typeof(double))
            {
                if (Avx.IsSupported)
                {
                    return Avx.AndNot(left.AsDouble(), right.AsDouble()).As<double, T>();
                }
            }

            if (Avx.IsSupported)
            {
                return Avx.AndNot(left.AsSingle(), right.AsSingle()).As<float, T>();
            }

            return SoftwareFallbacks.AndNot_Software(left, right);
        }

        /// <summary>
        /// Perform a bitwise NOT operation on a <see cref="Vector128{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of each element in the vector</typeparam>
        /// <param name="vector">The vector which will be NOT'ed</param>
        /// <returns>A new <see cref="Vector256{T}"/> with each element of <paramref name="vector"/> after being NOT'ed</returns>
        [MethodImpl(MaxOpt)]
        public static Vector256<T> Not<T>(Vector256<T> vector) where T : struct
        {
            return Xor(vector, DoubleConstants.AllBitsSet.As<double, T>());
        }
    }
}
