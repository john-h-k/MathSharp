using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;
using static MathSharp.Vector;

namespace MathSharp
{
    using Vector4F = Vector128<float>;
    using Vector4FParam1_3 = Vector128<float>;

    public static partial class Matrix
    {
        public static MatrixSingle Identity { get; } = new MatrixSingle(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        public static Vector128<float> IdentityRow0
        {
            [MethodImpl(MaxOpt)]
            get
            {
                if (Sse41.IsSupported)
                {
                    return Vector128.CreateScalar(1f);
                }

                return _identityRow0;
            }
        }

        public static Vector128<float> IdentityRow1
        {
            [MethodImpl(MaxOpt)]
            get
            {
                if (Sse41.IsSupported)
                {
                    return Sse41.Insert(Vector128<float>.Zero, Vector128.CreateScalarUnsafe(1.0f), 0x1);
                }

                return _identityRow1;
            }
        }

        public static Vector128<float> IdentityRow2
        {
            [MethodImpl(MaxOpt)]
            get
            {
                if (Sse41.IsSupported)
                {
                    return Sse41.Insert(Vector128<float>.Zero, Vector128.CreateScalarUnsafe(1.0f), 0x2);
                }

                return _identityRow2;
            }
        }

        public static Vector128<float> IdentityRow3
        {
            [MethodImpl(MaxOpt)]
            get
            {
                if (Sse41.IsSupported)
                {
                    return Sse41.Insert(Vector128<float>.Zero, Vector128.CreateScalarUnsafe(1.0f), 0x3);
                }

                return _identityRow3;
            }
        }

        private static readonly Vector128<float> _identityRow0 = Vector128.Create(1f, 0f, 0f, 0f);
        private static readonly Vector128<float> _identityRow1 = Vector128.Create(0f, 1f, 0f, 0f);
        private static readonly Vector128<float> _identityRow2 = Vector128.Create(0f, 0f, 1f, 0f);
        private static readonly Vector128<float> _identityRow3 = Vector128.Create(0f, 0f, 0f, 1f);

        public static bool IsIdentity(MatrixSingle matrix)
        {
            return CompareEqual(matrix, Identity);
        }

        public static bool CompareEqual(MatrixSingle left, MatrixSingle right)
        {
            Vector128<float> row0 = Vector.CompareEqual(left._v0, right._v0);
            Vector128<float> row1 = Vector.CompareEqual(left._v1, right._v1);
            Vector128<float> row2 = Vector.CompareEqual(left._v2, right._v2);
            Vector128<float> row3 = Vector.CompareEqual(left._v3, right._v3);

            row0 = And(row0, row1);
            row2 = And(row2, row3);
            row0 = And(row0, row2);

            return row0.AllTrue();
        }

        public static Vector128<float> GetTranslation(MatrixSingle matrix)
        {
            Vector128<float> vec = matrix._v3;
            return And(vec, SingleConstants.MaskW);
        }

        public static MatrixSingle SetTranslation(MatrixSingle matrix, Vector4FParam1_3 translation)
        {
            // (X, Y, Z, W) - we must keep W
            Vector4F old = matrix._v3;

            // Make W of translation zero

            Vector4F newTranslation = And(translation, SingleConstants.MaskW);
            // Mask out everything but W
            old = And(old, SingleConstants.MaskXYZ);

            // Or them together to get X Y Z from translation and W from old
            newTranslation = Or(newTranslation, old);

            matrix._v3 = newTranslation;

            return matrix;
        }

        private static readonly Vector4F BillboardEpsilon = Vector128.Create(1e-4f);

        public static MatrixSingle CreateBillboard(Vector4FParam1_3 objectPosition, Vector4FParam1_3 cameraPosition,
            Vector4FParam1_3 cameraUpVector, Vector4FParam1_3 cameraForwardVector)
        {
            Vector4F z = Vector.Subtract(objectPosition, cameraPosition);

            Vector4F norm = LengthSquared3D(z);

            z = CompareLessThan(norm, BillboardEpsilon).AnyTrue()
                ? Vector.Negate(cameraForwardVector)
                : Multiply(z, Divide(SingleConstants.AllBitsSet, Sqrt(norm)));

            Vector4F x = Normalize3D(CrossProduct3D(cameraUpVector, z));

            Vector4F y = CrossProduct3D(z, x);

            // We need W to be zero for x, y, and z, and 1.0f for objectPosition. They are currently undefined
            x = And(x, SingleConstants.MaskW);
            y = And(y, SingleConstants.MaskW);
            z = And(z, SingleConstants.MaskW);

            // Get objectPosition to be (X, Y, Z, 0) and the mask to be (0, 0, 0, 1.0f) and OR them
            Vector4F newObjectPosition = And(objectPosition, SingleConstants.MaskW);
            newObjectPosition = Or(newObjectPosition, And(SingleConstants.MaskXYZ, SingleConstants.AllBitsSet));

            return new MatrixSingle(x, y, z, newObjectPosition);
        }

        public static Vector128<float> Transform2D(Vector128<float> position, MatrixSingle matrix)
        {
            var x = PermuteWithX(position);
            var y = PermuteWithY(position);

            x = Multiply(x, matrix._v0);
            y = Multiply(y, matrix._v1);

            x = Vector.Add(x, y);
            x = Vector.Add(x, matrix._v3);

            return x;
        }

        public static Vector128<float> Transform3D(Vector128<float> position, MatrixSingle matrix)
        {
            var x = PermuteWithX(position);
            var y = PermuteWithY(position);
            var z = PermuteWithZ(position);

            x = Multiply(x, matrix._v0);
            y = Multiply(y, matrix._v1);
            z = Multiply(z, matrix._v2);

            x = Vector.Add(x, y);
            x = Vector.Add(x, z);
            x = Vector.Add(x, matrix._v3);

            return x;
        }

        public static Vector128<float> Transform4D(Vector128<float> position, MatrixSingle matrix)
        {
            var x = PermuteWithX(position);
            var y = PermuteWithY(position);
            var z = PermuteWithZ(position);
            var w = PermuteWithW(position);

            x = Multiply(x, matrix._v0);
            y = Multiply(y, matrix._v1);
            z = Multiply(z, matrix._v2);
            w = Multiply(w, matrix._v3);

            x = Vector.Add(x, y);
            x = Vector.Add(x, z);
            x = Vector.Add(x, w);

            return x;
        }

        private unsafe struct CanonicalBasis
        {
            public Vector128<float>* E0;
            public Vector128<float>* E1;
            public Vector128<float>* E2;
        }

        private unsafe struct VectorBasis
        {
            public Vector128<float>* E0;
            public Vector128<float>* E1;
            public Vector128<float>* E2;
        }

        private const float DecomposeEpsilon = 0.0001f;

        public static unsafe bool Decompose(MatrixSingle matrix, out Vector128<float> scale, out Vector128<float> rotation, out Vector128<float> translation)
        {
            // TODO use Unsafe.SkipInit<T>(out T)
            scale = default;

            translation = matrix._v3;

            CanonicalBasis canonicalBasis;
            Vector128<float>** pCanonicalBasis = (Vector128<float>**)&canonicalBasis;
            var r0 = IdentityRow0;
            var r1 = IdentityRow1;
            var r2 = IdentityRow2;
            canonicalBasis.E0 = &r0;
            canonicalBasis.E1 = &r1;
            canonicalBasis.E2 = &r2;

            MatrixSingle tmp;

            VectorBasis vectorBasis;
            Vector128<float>** pVectorBasis = (Vector128<float>**)&vectorBasis;
            vectorBasis.E0 = &tmp._v0;
            vectorBasis.E1 = &tmp._v1;
            vectorBasis.E2 = &tmp._v2;

            tmp._v0 = matrix._v0;
            tmp._v1 = matrix._v1;
            tmp._v2 = matrix._v2;
            tmp._v3 = IdentityRow3;

            ref var pFloat0 = ref Unsafe.As<Vector128<float>, float>(ref scale);
            ref var pFloat1 = ref Unsafe.Add(ref pFloat0, 1);
            ref var pFloat2 = ref Unsafe.Add(ref pFloat0, 2);
            ref var pFloat3 = ref Unsafe.Add(ref pFloat0, 3);
            pFloat0 = Length3D(*vectorBasis.E0).ToScalar();
            pFloat1 = Length3D(*vectorBasis.E0).ToScalar();
            pFloat2 = Length3D(*vectorBasis.E0).ToScalar();
            pFloat3 = 0.0f;

            Rank3Decompose(out int a, out int b, out int c, pFloat0, pFloat1, pFloat2);

            if (Unsafe.Add(ref pFloat0, a) < DecomposeEpsilon)
            {
                *pVectorBasis[a] = *pCanonicalBasis[a];
            }

            *pVectorBasis[a] = Normalize3D(*pVectorBasis[a]);

            if (Unsafe.Add(ref pFloat0, b) < DecomposeEpsilon)
            {
                var absX = MathF.Abs(GetX(*pVectorBasis[a]));
                var absY = MathF.Abs(GetY(*pVectorBasis[a]));
                var absZ = MathF.Abs(GetZ(*pVectorBasis[a]));

                Rank3Decompose(out _, out _, out int cc, absX, absY, absZ);

                *pVectorBasis[b] = CrossProduct3D(*pVectorBasis[a], *pCanonicalBasis[cc]);
            }

            *pVectorBasis[b] = Normalize3D(*pVectorBasis[b]);

            if (Unsafe.Add(ref pFloat0, c) < DecomposeEpsilon)
            {
                *pVectorBasis[c] = CrossProduct3D(*pVectorBasis[a], *pVectorBasis[b]);
            }

            *pVectorBasis[c] = Normalize3D(*pVectorBasis[c]);

            float det = GetX(Determinant(tmp));

            if (det < 0.0f)
            {
                Unsafe.Add(ref pFloat0, a) = -Unsafe.Add(ref pFloat0, a);
                *pVectorBasis[a] = Vector.Negate(*pVectorBasis[a]);

                det = -det;
            }

            det -= 1.0f;
            det *= det;

            if (DecomposeEpsilon < det)
            {
                rotation = default;
                return false;
            }

            rotation = QuaternionFromRotation(tmp);
            return true;
        }

        private static Vector128<float> QuaternionFromRotation(MatrixSingle matrix)
        {
            throw new NotImplementedException();
        }

        private static readonly Vector128<float> Sign = Vector128.Create(1.0f, -1.0f, 1.0f, -1.0f);
        public static Vector128<float> Determinant(MatrixSingle matrix)
        { 
            var v0 = Permute(matrix._v2, ShuffleValues.YXXX);
            var v1 = Permute(matrix._v3, ShuffleValues.ZZYY);
            var v2 = Permute(matrix._v2, ShuffleValues.YXXX);
            var v3 = Permute(matrix._v3, ShuffleValues.WWWZ);
            var v4 = Permute(matrix._v2, ShuffleValues.ZZYY);
            var v5 = Permute(matrix._v3, ShuffleValues.WWWZ);

            var p0 = Multiply(v0, v1);
            var p1 = Multiply(v2, v3);
            var p2 = Multiply(v4, v5);

            v0 = Permute(matrix._v2, ShuffleValues.ZZYY);
            v1 = Permute(matrix._v3, ShuffleValues.YXXX);
            v2 = Permute(matrix._v2, ShuffleValues.WWWZ);
            v3 = Permute(matrix._v3, ShuffleValues.YXXX);
            v4 = Permute(matrix._v2, ShuffleValues.WWWZ);
            v5 = Permute(matrix._v3, ShuffleValues.ZZYY);

            p0 = FastNegateMultiplyAdd(v0, v1, p0);
            p1 = FastNegateMultiplyAdd(v2, v3, p1);
            p2 = FastNegateMultiplyAdd(v4, v5, p2);

            v0 = Permute(matrix._v1, ShuffleValues.WWWZ);
            v1 = Permute(matrix._v1, ShuffleValues.ZZYY);
            v2 = Permute(matrix._v1, ShuffleValues.YXXX);

            var s = Multiply(matrix._v0, Sign);
            var r = Multiply(v0, p0);
            r = FastNegateMultiplyAdd(v1, p1, r);
            r = FastMultiplyAdd(v2, p2, r);

            return DotProduct4D(s, r);
        }

        [MethodImpl(MaxOpt)]
        private static void Rank3Decompose(out int a, out int b, out int c, float x, float y, float z)
        {
            if (x < y)
            {
                if (y < z)
                {
                    a = 2;
                    b = 1;
                    c = 0;
                }
                else
                {
                    a = 1;

                    if (x < z)
                    {
                        b = 2;
                        c = 0;
                    }
                    else
                    {
                        b = 0;
                        c = 2;
                    }
                }
            }
            else
            {
                if (x < z)
                {
                    a = 2;
                    b = 0;
                    c = 1;
                }
                else
                {
                    a = 0;

                    if (y < z)
                    {
                        b = 2;
                        c = 1;
                    }
                    else
                    {
                        b = 1;
                        c = 2;
                    }
                }
            }
        }
    }
}
