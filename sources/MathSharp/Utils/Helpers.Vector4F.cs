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
            return Vector256.Create(vector.ToScalar());
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

        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static float Y(Vector128<float> vector) => vector.GetElement(1);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static float Z(Vector128<float> vector) => vector.GetElement(2);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static float W(Vector128<float> vector) => vector.GetElement(3);

        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static double X(Vector128<double> vector) => vector.GetElement(0);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static double Y(Vector128<double> vector) => vector.GetElement(1);

        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static double X(Vector256<double> vector) => vector.GetElement(0);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static double Y(Vector256<double> vector) => vector.GetElement(1);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static double Z(Vector256<double> vector) => vector.GetElement(2);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static double W(Vector256<double> vector) => vector.GetElement(3);

        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static ulong X(Vector256<ulong> vector) => vector.GetElement(0);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static ulong Y(Vector256<ulong> vector) => vector.GetElement(1);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static ulong Z(Vector256<ulong> vector) => vector.GetElement(2);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static ulong W(Vector256<ulong> vector) => vector.GetElement(3);

        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static long X(Vector256<long> vector) => vector.GetElement(0);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static long Y(Vector256<long> vector) => vector.GetElement(1);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static long Z(Vector256<long> vector) => vector.GetElement(2);
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static long W(Vector256<long> vector) => vector.GetElement(3);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(Vector4 left, Vector128<float> right)
            => left.X .Equals(X(right)) && left.Y.Equals(Y(right)) && left.Z.Equals(Z(right)) && left.W.Equals(W(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(Vector3 left, Vector128<float> right)
            => left.X.Equals(X(right)) && left.Y.Equals(Y(right)) && left.Z.Equals(Z(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(Vector2 left, Vector128<float> right)
            => left.X.Equals(X(right)) && left.Y.Equals(Y(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreEqual(float left, Vector128<float> right)
            => left.Equals(X(right));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(float left, float right, float interval = 0.000000001f)
        {
            if (float.IsNaN(left) && float.IsNaN(right))
                return true;

            return MathF.Abs(left - right) < interval;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(Vector4 left, Vector128<float> right, float interval = 0.000000001f)
        {
            return AreApproxEqual(left.X, X(right), interval)
                   && AreApproxEqual(left.Y, Y(right), interval)
                   && AreApproxEqual(left.Z, Z(right), interval)
                   && AreApproxEqual(left.W, W(right), interval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(Vector3 left, Vector128<float> right, float interval = 0.000000001f)
        {
            return AreApproxEqual(left.X, X(right), interval)
                   && AreApproxEqual(left.Y, Y(right), interval)
                   && AreApproxEqual(left.Z, Z(right), interval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(Vector2 left, Vector128<float> right, float interval = 0.000000001f)
        {
            return AreApproxEqual(left.X, X(right), interval)
                   && AreApproxEqual(left.Y, Y(right), interval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool AreApproxEqual(float left, Vector128<float> right)
        {
            return AreApproxEqual(left, X(right));
        }

        public static readonly float NoBitsSetSingle = 0f; 
        public static readonly float AllBitsSetSingle = Unsafe.As<int, float>(ref Unsafe.AsRef(-1));

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector2 ByValToSlowVector2(Vector128<float> vec)
        {
            return new Vector2(X(vec), Y(vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector3 ByValToSlowVector3(Vector128<float> vec)
        {
            return new Vector3(X(vec), Y(vec), Z(vec));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4 ByValToSlowVector4(Vector128<float> vec)
        {
            return new Vector4(X(vec), Y(vec), Z(vec), W(vec));
        }
    }
}
