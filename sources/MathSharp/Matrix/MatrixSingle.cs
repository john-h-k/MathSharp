using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Attributes;

namespace MathSharp.Matrix
{ 
    using VectorF = Vector128<float>;

    [Hva]
    [Aligned(16)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct MatrixSingle
    {
        internal VectorF _v1;
        internal VectorF _v2;
        internal VectorF _v3;
        internal VectorF _v4;

        internal VectorF this[uint index]
        {
            get => Unsafe.Add(ref _v1, (int)index);

            set => Unsafe.Add(ref _v1, (int)index) = value;
        }

        internal float this[uint x, uint y]
        {
            get => Unsafe.Add(ref Unsafe.As<VectorF, float>(ref _v1), (int)(x * 4 + y));

            set => Unsafe.Add(ref Unsafe.As<VectorF, float>(ref _v1), (int)(x * 4 + y)) = value;
        }
    }
}
