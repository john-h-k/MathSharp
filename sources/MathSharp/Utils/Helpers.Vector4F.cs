using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp.Utils
{
    internal static partial class Helpers
    {
        

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector256<double> ToScalarVector256(Vector256<double> vector)
        {
            return Vector256.CreateScalar(vector.ToScalar());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector256<double> DuplicateToVector256(Vector128<double> vector)
        {
            return Vector256.Create(vector, vector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T X<T>(Vector128<T> vector) where T : struct => vector.GetElement(0);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Y<T>(Vector128<T> vector) where T : struct => vector.GetElement(1);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Z<T>(Vector128<T> vector) where T : struct => vector.GetElement(2);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T W<T>(Vector128<T> vector) where T : struct => vector.GetElement(3);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T X<T>(Vector256<T> vector) where T : struct => vector.GetElement(0);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Y<T>(Vector256<T> vector) where T : struct => vector.GetElement(1);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Z<T>(Vector256<T> vector) where T : struct => vector.GetElement(2);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T W<T>(Vector256<T> vector) where T : struct => vector.GetElement(3);

        public static readonly float NoBitsSetSingle = 0f; 
        public static readonly float AllBitsSetSingle = BitConverter.Int32BitsToSingle(-1);
    }
}
