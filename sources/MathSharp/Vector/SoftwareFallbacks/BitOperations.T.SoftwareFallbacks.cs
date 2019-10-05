using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp
{
    internal static partial class SoftwareFallbacks
    {
        #region Vector128

        [MethodImpl(MaxOpt)]
        public static Vector128<T> Or_Software<T>(in Vector128<T> left, in Vector128<T> right) where T : struct 
            => Or_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector128<T> And_Software<T>(in Vector128<T> left, in Vector128<T> right) where T : struct 
            => And_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector128<T> Xor_Software<T>(in Vector128<T> left, in Vector128<T> right) where T : struct
            => Xor_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector128<T> Not_Software<T>(in Vector128<T> vector) where T : struct
            => Not_Software(vector.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector128<T> AndNot_Software<T>(in Vector128<T> left, in Vector128<T> right) where T : struct 
            => AndNot_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        #endregion

        #region Vector256

        [MethodImpl(MaxOpt)]
        public static Vector256<T> Or_Software<T>(in Vector256<T> left, in Vector256<T> right) where T : struct 
            => Or_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector256<T> And_Software<T>(in Vector256<T> left, in Vector256<T> right) where T : struct 
            => And_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector256<T> Xor_Software<T>(in Vector256<T> left, in Vector256<T> right) where T : struct 
            => Xor_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector256<T> Not_Software<T>(in Vector256<T> vector) where T : struct 
            => Not_Software(vector.AsUInt64()).As<ulong, T>();

        [MethodImpl(MaxOpt)]
        public static Vector256<T> AndNot_Software<T>(in Vector256<T> left, in Vector256<T> right) where T : struct 
            => AndNot_Software(left.AsUInt64(), right.AsUInt64()).As<ulong, T>();

        #endregion
    }
}
