using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
// ReSharper disable MemberCanBePrivate.Local

namespace MathSharp
{
    public static partial class Vector
    {

        private readonly ref struct VectorFormatter<T> where T : struct
        {
            public readonly ReadOnlySpan<char> Delimiter;
            public readonly ReadOnlySpan<char> Start;
            public readonly ReadOnlySpan<char> End;
            public readonly int ElemCount;
            public readonly ReadOnlySpan<char> Format;
            public readonly IFormatProvider? FormatProvider;
            public readonly Vector256<T> Vector;

            public VectorFormatter(
                ReadOnlySpan<char> delimiter,
                ReadOnlySpan<char> start,
                ReadOnlySpan<char> end,
                int elemCount,
                Vector256<T> vector,
                ReadOnlySpan<char> format = default,
                IFormatProvider? formatProvider = null
            )
            {
                Delimiter = delimiter;
                Start = start;
                End = end;
                ElemCount = elemCount;
                Vector = vector;
                Format = format;
                FormatProvider = formatProvider;
            }

            public string? FormatElem(T value)
            {
                if (value is IFormattable formattable)
                {
                    return formattable.ToString(Format.ToString(), FormatProvider);
                }

                Debug.Assert(Format == null && FormatProvider == null);

                return value.ToString();
            }

            public override string? ToString()
            {
                StringBuilder b = new StringBuilder();

                b.Append(Start);

                for (var i = 0; i < ElemCount; i++)
                {
                    var elem = Vector.GetElement(i);
                    var str = FormatElem(elem);

                    b.Append(str);

                    if (i != ElemCount - 1) b.Append(Delimiter);
                }

                b.Append(End);

                return b.ToString();
            }
        }

        private static Vector256<T> AsVector256<T>(Vector64<T> vector) where T : struct 
            => vector.ToVector128Unsafe().ToVector256Unsafe();
        private static Vector256<T> AsVector256<T>(Vector128<T> vector) where T : struct
            => vector.ToVector256Unsafe();

        public static string? ToString<T>(Vector64<T> vector, string? format = null, IFormatProvider? provider = null, int elemCount = -1, string delimiter = ", ", string start = "<", string end = ">") where T : struct
            => InternalToString<T, Vector64<T>>(AsVector256(vector), format, provider, elemCount, delimiter, start, end);
        public static string? ToString<T>(Vector128<T> vector, string? format = null, IFormatProvider? provider = null, int elemCount = -1, string delimiter = ", ", string start = "<", string end = ">") where T : struct
            => InternalToString<T, Vector128<T>>(AsVector256(vector), format, provider, elemCount, delimiter, start, end);
        public static string? ToString<T>(Vector256<T> vector, string? format = null, IFormatProvider? provider = null, int elemCount = -1, string delimiter = ", ", string start = "<", string end = ">") where T : struct 
            => InternalToString<T, Vector256<T>>(vector, format, provider, elemCount, delimiter, start, end);

        private static string? InternalToString<T, TVec>(Vector256<T> vector, string? format, IFormatProvider? formatProvider, int elemCount,
            string delimiter, string start, string end) where T : struct
        {
            if ((format != null || formatProvider != null) && !(default(T) is IFormattable))
            {
                throw new ArgumentException($"Format or format provider was provided, yet type {typeof(T).Name} is not {nameof(IFormattable)}");
            }

            elemCount = RationaliseElemCount<T, TVec>(elemCount);

            var formatter = new VectorFormatter<T>(delimiter, start, end, elemCount, vector, format, formatProvider);

            return formatter.ToString();
        }
        private static int RationaliseElemCount<T, TVec>(int elemCount) where T : struct
        {
            var count = Unsafe.SizeOf<TVec>() / Unsafe.SizeOf<T>();
            if (elemCount == -1) return count;
            if (elemCount < 0 || elemCount > count) throw new ArgumentOutOfRangeException(nameof(elemCount), $"Out of range [0, {count}]");
            return elemCount;
        }
    }
}
