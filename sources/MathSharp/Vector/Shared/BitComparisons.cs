using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static Vector128<byte> CompareBit8Equal(Vector128<byte> left, Vector128<byte> right)
            => CompareBit8Equal(left.AsSByte(), right.AsSByte()).AsByte();

        [MethodImpl(MaxOpt)]
        public static Vector128<sbyte> CompareBit8Equal(Vector128<sbyte> left, Vector128<sbyte> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector128<sbyte> SoftwareFallback(Vector128<sbyte> left, Vector128<sbyte> right)
            {
                Vector128<sbyte> result = default;

                for (var i = 0; i < Vector128<sbyte>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? (sbyte)-1 : (sbyte)0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<byte> CompareBit8Equal(Vector256<byte> left, Vector256<byte> right)
            => CompareBit8Equal(left.AsSByte(), right.AsSByte()).AsByte();

        [MethodImpl(MaxOpt)]
        public static Vector256<sbyte> CompareBit8Equal(Vector256<sbyte> left, Vector256<sbyte> right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector256<sbyte> SoftwareFallback(Vector256<sbyte> left, Vector256<sbyte> right)
            {
                Vector256<sbyte> result = default;

                for (var i = 0; i < Vector256<sbyte>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? (sbyte)-1 : (sbyte)0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<ushort> CompareBit16Equal(Vector128<ushort> left, Vector128<ushort> right)
            => CompareBit16Equal(left.AsInt16(), right.AsInt16()).AsUInt16();

        [MethodImpl(MaxOpt)]
        public static Vector128<short> CompareBit16Equal(Vector128<short> left, Vector128<short> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector128<short> SoftwareFallback(Vector128<short> left, Vector128<short> right)
            {
                Vector128<short> result = default;

                for (var i = 0; i < Vector128<short>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? (short)-1 : (short)0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<ushort> CompareBit16Equal(Vector256<ushort> left, Vector256<ushort> right)
            => CompareBit16Equal(left.AsInt16(), right.AsInt16()).AsUInt16();

        [MethodImpl(MaxOpt)]
        public static Vector256<short> CompareBit16Equal(Vector256<short> left, Vector256<short> right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector256<short> SoftwareFallback(Vector256<short> left, Vector256<short> right)
            {
                Vector256<short> result = default;

                for (var i = 0; i < Vector256<short>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? (short)-1 : (short)0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<uint> CompareBit32Equal(Vector128<uint> left, Vector128<uint> right)
            => CompareBit32Equal(left.AsInt32(), right.AsInt32()).AsUInt32();

        [MethodImpl(MaxOpt)]
        public static Vector128<int> CompareBit32Equal(Vector128<int> left, Vector128<int> right)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector128<int> SoftwareFallback(Vector128<int> left, Vector128<int> right)
            {
                Vector128<int> result = default;

                for (var i = 0; i < Vector128<int>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? -1 : 0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<uint> CompareBit32Equal(Vector256<uint> left, Vector256<uint> right)
            => CompareBit32Equal(left.AsInt32(), right.AsInt32()).AsUInt32();

        [MethodImpl(MaxOpt)]
        public static Vector256<int> CompareBit32Equal(Vector256<int> left, Vector256<int> right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector256<int> SoftwareFallback(Vector256<int> left, Vector256<int> right)
            {
                Vector256<int> result = default;

                for (var i = 0; i < Vector256<int>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? -1 : 0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<ulong> CompareBit64Equal(Vector128<ulong> left, Vector128<ulong> right)
            => CompareBit64Equal(left.AsInt64(), right.AsInt64()).AsUInt64();

        [MethodImpl(MaxOpt)]
        public static Vector128<long> CompareBit64Equal(Vector128<long> left, Vector128<long> right)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector128<long> SoftwareFallback(Vector128<long> left, Vector128<long> right)
            {
                Vector128<long> result = default;

                for (var i = 0; i < Vector256<long>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? -1 : 0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<ulong> CompareBit64Equal(Vector256<ulong> left, Vector256<ulong> right)
            => CompareBit64Equal(left.AsInt64(), right.AsInt64()).AsUInt64();

        [MethodImpl(MaxOpt)]
        public static Vector256<long> CompareBit64Equal(Vector256<long> left, Vector256<long> right)
        {
            if (Avx2.IsSupported)
            {
                return Avx2.CompareEqual(left, right);
            }

            return SoftwareFallback(left, right);

            static Vector256<long> SoftwareFallback(Vector256<long> left, Vector256<long> right)
            {
                Vector256<long> result = default;

                for (var i = 0; i < Vector256<int>.Count; i++)
                {
                    result = result.WithElement(i, left.GetElement(i) == right.GetElement(i) ? -1 : 0);
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static int MoveMask(Vector128<byte> vector)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.MoveMask(vector);
            }

            return SoftwareFallback(vector);

            static int SoftwareFallback(Vector128<byte> vector)
            {
                var result = 0;

                for (var i = 0; i < Vector128<byte>.Count; i++)
                {
                    result |= (vector.GetElement(i) >> 31) << i;
                }

                return result;
            }
        }

        [MethodImpl(MaxOpt)]
        public static int MoveMask(Vector128<float> vector)
        {
            if (Sse.IsSupported)
            {
                return Sse.MoveMask(vector);
            }

            return SoftwareFallback(vector);

            static int SoftwareFallback(Vector128<float> vector)
            {
                var result = 0;

                for (var i = 0; i < Vector128<float>.Count; i++)
                {
                    result |= (vector.AsInt32().GetElement(i) >> 31) << i;
                }

                return result;
            }
        }
    }
}
