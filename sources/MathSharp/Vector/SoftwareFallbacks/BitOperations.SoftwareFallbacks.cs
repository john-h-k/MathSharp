using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    internal static partial class SoftwareFallbacks
    {
        #region Vector128

        public static Vector128<float> Shuffle_Software(Vector128<float> left, Vector128<float> right, byte control)
        {
            const byte e0Mask = 0b_0000_0011, e1Mask = 0b_0000_1100, e2Mask = 0b_0011_0000, e3Mask = 0b_1100_0000;

            int e0Selector = control & e0Mask;
            float e0 = left.GetElement(e0Selector);

            int e1Selector = (control & e1Mask) >> 2;
            float e1 = left.GetElement(e1Selector);

            int e2Selector = (control & e2Mask) >> 4;
            float e2 = right.GetElement(e2Selector);

            int e3Selector = (control & e3Mask) >> 6;
            float e3 = right.GetElement(e3Selector);

            return Vector128.Create(e0, e1, e2, e3);
        }

        public static Vector256<double> Shuffle_Software(Vector256<double> left, Vector256<double> right, byte control)
        {
            const byte e0Mask = 0b_0000_0011, e1Mask = 0b_0000_1100, e2Mask = 0b_0011_0000, e3Mask = 0b_1100_0000;

            int e0Selector = control & e0Mask;
            double e0 = left.GetElement(e0Selector);

            int e1Selector = (control & e1Mask) >> 2;
            double e1 = left.GetElement(e1Selector);

            int e2Selector = (control & e2Mask) >> 4;
            double e2 = right.GetElement(e2Selector);

            int e3Selector = (control & e3Mask) >> 6;
            double e3 = right.GetElement(e3Selector);

            return Vector256.Create(e0, e1, e2, e3);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<ulong> Or_Software(Vector128<ulong> left, Vector128<ulong> right)
        {
            return Vector128.Create(
                X(left) | X(right),
                Y(left) | Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<ulong> And_Software(Vector128<ulong> left, Vector128<ulong> right)
        {
            return Vector128.Create(
                X(left) & X(right),
                Y(left) & Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<ulong> Xor_Software(Vector128<ulong> left, Vector128<ulong> right)
        {
            return Vector128.Create(
                X(left) ^ X(right),
                Y(left) ^ Y(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<ulong> Not_Software(Vector128<ulong> vector)
        {
            return Vector128.Create(
                ~X(vector),
                ~Y(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<ulong> AndNot_Software(Vector128<ulong> left, Vector128<ulong> right)
        {
            return And_Software(Not_Software(left), right);
        }

        #endregion

        #region Vector256

        [MethodImpl(MaxOpt)]
        public static Vector256<ulong> Or_Software(Vector256<ulong> left, Vector256<ulong> right)
        {
            return Vector256.Create(
                X(left) | X(right),
                Y(left) | Y(right),
                Z(left) | Z(right),
                W(left) | W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<ulong> And_Software(Vector256<ulong> left, Vector256<ulong> right)
        {
            return Vector256.Create(
                X(left) & X(right),
                Y(left) & Y(right),
                Z(left) & Z(right),
                W(left) & W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<ulong> Xor_Software(Vector256<ulong> left, Vector256<ulong> right)
        {
            return Vector256.Create(
                X(left) ^ X(right),
                Y(left) ^ Y(right),
                Z(left) ^ Z(right),
                W(left) ^ W(right)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<ulong> Not_Software(Vector256<ulong> vector)
        {
            return Vector256.Create(
                ~X(vector),
                ~Y(vector),
                ~Z(vector),
                ~W(vector)
            );
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<ulong> AndNot_Software(Vector256<ulong> left, Vector256<ulong> right)
        {
            return And_Software(Not_Software(left), right);
        }

        #endregion    }
    }
}
