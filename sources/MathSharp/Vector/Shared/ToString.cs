using System;
using System.Diagnostics;
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
            public readonly object Vector;

            public VectorFormatter(
                ReadOnlySpan<char> delimiter, 
                ReadOnlySpan<char> start,
                ReadOnlySpan<char> end, 
                int elemCount, 
                object vector,
                ReadOnlySpan<char> format = default, 
                IFormatProvider? formatProvider = null
                )
            {
                Delimiter = delimiter;
                Start = start;
                End = end;
                ElemCount = elemCount;
                Debug.Assert(vector is Vector64<T> || vector is Vector128<T> || vector is Vector256<T>);
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

                for (int i = 0; i < ElemCount; i++)
                {
                    var elem = GetElement(i);
                    var str = FormatElem(elem);

                    b.Append(str);

                    if (i != ElemCount - 1) b.Append(Delimiter);
                }

                b.Append(End);

                return b.ToString();
            }

            private T GetElement(int i)
            {
                if (Vector is Vector64<T> vector64) return vector64.GetElement(i);
                if (Vector is Vector128<T> vector128) return vector128.GetElement(i);
                if (Vector is Vector256<T> vector256) return vector256.GetElement(i);

                return default;
            }
        }

        private static int RationaliseElemCount<T>(int elemCount) where T : struct
        {
            if (elemCount == -1) return Vector128<T>.Count;
            if (elemCount < 0 || elemCount > Vector128<T>.Count) throw new ArgumentOutOfRangeException(nameof(elemCount), $"Out of range [0, {Vector128<T>.Count}]");
            return elemCount;
        }

        public static string? ToString<T>(Vector128<T> vector, string? format = null, IFormatProvider? provider = null, int elemCount = -1, string delimiter = ", ", string start = "<", string end = ">") where T : struct
        {
            if ((format != null || provider != null) && !(default(T) is IFormattable))
            {
                throw new ArgumentException($"Format or format provider was provided, yet type {typeof(T).Name} is not {nameof(IFormattable)}");
            }

            elemCount = RationaliseElemCount<T>(elemCount);

            var formatter = new VectorFormatter<T>(delimiter, start, end, elemCount, vector, format, provider);

            return formatter.ToString();
        }
    }
}
