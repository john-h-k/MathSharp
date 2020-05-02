using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Attributes;

namespace MathSharp
{
    [Hva]
    [Aligned(16)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct MatrixSingle
    {
        // This stuff is internal because

        internal Vector128<float> _v0;
        internal Vector128<float> _v1;
        internal Vector128<float> _v2;
        internal Vector128<float> _v3;

        internal Vector128<float> this[uint index]
        {
            get => Unsafe.Add(ref _v0, (int)index);

            set => Unsafe.Add(ref _v0, (int)index) = value;
        }

        internal float this[uint x, uint y]
        {
            get => Unsafe.Add(ref Unsafe.As<Vector128<float>, float>(ref _v0), (int)(x * 4 + y));

            set => Unsafe.Add(ref Unsafe.As<Vector128<float>, float>(ref _v0), (int)(x * 4 + y)) = value;
        }

        internal MatrixSingle(Vector128<float> v0, Vector128<float> v1, Vector128<float> v2, Vector128<float> v3)
        {
            _v0 = v0;
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
        }
        internal MatrixSingle(Vector128<float> v)
        {
            _v0 = v;
            _v1 = v;
            _v2 = v;
            _v3 = v;
        }

        internal MatrixSingle(
            float _11, float _12, float _13, float _14,
            float _21, float _22, float _23, float _24,
            float _31, float _32, float _33, float _34,
            float _41, float _42, float _43, float _44)
            : this(
                Vector128.Create(_11, _12, _13, _14),
                Vector128.Create(_21, _22, _23, _24),
                Vector128.Create(_31, _32, _33, _34),
                Vector128.Create(_41, _42, _43, _44))
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixSingle Create(Vector128<float> v1, Vector128<float> v2, Vector128<float> v3, Vector128<float> v4)
        {
            return new MatrixSingle(v1, v2, v3, v4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixSingle Create(Vector128<float> v)
        {
            return new MatrixSingle(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixSingle Create(
            float _11, float _12, float _13, float _14,
            float _21, float _22, float _23, float _24,
            float _31, float _32, float _33, float _34,
            float _41, float _42, float _43, float _44)
        {
            return Create(
                Vector128.Create(_11, _12, _13, _14),
                Vector128.Create(_21, _22, _23, _24),
                Vector128.Create(_31, _32, _33, _34),
                Vector128.Create(_41, _42, _43, _44)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MatrixSingle Create(float value)
        {
            return Create(Vector128.Create(value));
        }

#if !NO_OPERATORS

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixSingle operator +(MatrixSingle left, MatrixSingle right)
            => Matrix.Add(left, right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixSingle operator -(in MatrixSingle left, in MatrixSingle right)
            => Matrix.Subtract(left, right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixSingle operator *(in MatrixSingle left, float scalar)
            => Matrix.ScalarMultiply(left, scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixSingle operator *(float scalar, in MatrixSingle right)
            => Matrix.ScalarMultiply(right, scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixSingle operator *(in MatrixSingle left, Vector128<float> vectorOfScalar)
            => Matrix.ScalarMultiply(left, vectorOfScalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixSingle operator *(Vector128<float> vectorOfScalar, in MatrixSingle right)
            => Matrix.ScalarMultiply(right, vectorOfScalar);

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixSingle operator -(in MatrixSingle matrix)
            => Matrix.Negate(matrix);

        #endregion

#endif
    }
}
