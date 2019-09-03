using System.Runtime.Intrinsics;

namespace MathSharp
{
    public static partial class Vector
    {
        public static Vector128<T> And<T>(in Vector128<T> left, in Vector128<T> right) where T : struct
        {
            var l = left.AsByte();
            var r = right.AsByte();
            return And(l, r).As<byte, T>();
        }

        public static Vector128<T> Or<T>(in Vector128<T> left, in Vector128<T> right) where T : struct
        {
            var l = left.AsByte();
            var r = right.AsByte();
            return Or(l, r).As<byte, T>();
        }

        public static Vector128<T> Xor<T>(in Vector128<T> left, in Vector128<T> right) where T : struct
        {
            var l = left.AsByte();
            var r = right.AsByte();
            return Xor(l, r).As<byte, T>();
        }

        public static Vector128<T> AndNot<T>(in Vector128<T> left, in Vector128<T> right) where T : struct
        {
            var l = left.AsByte();
            var r = right.AsByte();
            return AndNot(l, r).As<byte, T>();
        }

        public static Vector128<T> Not<T>(in Vector128<T> vector) where T : struct
        {
            var v = vector.AsByte();
            return Not(v).As<byte, T>();
        }
    }
}
