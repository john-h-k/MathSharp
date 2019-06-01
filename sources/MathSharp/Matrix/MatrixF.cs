using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics;
using MathSharp.Attributes;

namespace MathSharp.Matrix
{ 
    using VectorF = Vector128<float>;

    [Hva]
    [Aligned(16)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct MatrixF
    {
        public VectorF _v1;
        public VectorF _v2;
        public VectorF _v3;
        public VectorF _v4;
    }
}
