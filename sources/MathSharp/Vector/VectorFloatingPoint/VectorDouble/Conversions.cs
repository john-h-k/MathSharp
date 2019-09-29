using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4D = Vector256<double>;
    using Vector2D = Vector128<double>;

    public static unsafe partial class Vector
    {
        #region Loads

        [MethodImpl(MaxOpt)]
        public static HwVector4D Load4DAligned(double* p) 
            => Load4D(p);
        [MethodImpl(MaxOpt)]
        public static HwVector3D Load3DAligned(double* p) 
            => (HwVector3D)Load4DAligned(p);
        [MethodImpl(MaxOpt)]
        public static HwVector2D Load2DAligned(double* p) 
            => (HwVector2D)Load4DAligned(p);


        [MethodImpl(MaxOpt)]
        public static HwVector4D Load4D(double* p)
        {
            if (Avx.IsSupported)
            {
                return Avx.LoadVector256(p);
            }
            if (Sse2.IsSupported)
            {
                return FromLowHigh(Sse2.LoadVector128(p), Sse2.LoadVector128(p + 2));
            }
            if (Sse.IsSupported)
            {
                return FromLowHigh(Sse.LoadVector128((float*)p).AsDouble(),
                    Sse.LoadVector128(((float*)p) + 4).AsDouble());
            }

            return SoftwareFallback(p);

            static HwVectorAnyD SoftwareFallback(double* p)
            {
                return Vector256.Create(p[0], p[1], p[2], p[3]);
            }
        }

        [MethodImpl(MaxOpt)]
        public static HwVector3D Load3D(double* p)
        {
            if (Sse2.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                Vector2D lo = Sse2.LoadVector128(p);
                Vector2D hi = Sse2.LoadScalarVector128(&p[2]);
                hi = And(hi, DoubleConstants.MaskZ128);

                return Vector256.Create(lo, hi);
            }

            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                Vector2D lo = Sse.LoadVector128((float*)p).AsDouble();
                Vector2D hi = Sse.LoadLow(Vector128<float>.Zero, (float*)&p[2]).AsDouble();
                hi = And(hi.AsSingle(), MaskZAndWSingle).AsDouble();

                return Vector256.Create(lo, hi);
            }

            return SoftwareFallback(p);

            static HwVectorAnyD SoftwareFallback(double* p)
            {
                return Vector256.Create(p[0], p[1], p[2], 0);
            }
        }

        [MethodImpl(MaxOpt)]
        public static HwVector2D Load2D(double* p)
        {
            if (Sse2.IsSupported)
            {
                Vector2D lo = Sse2.LoadVector128(p);
                return lo.ToVector256();
            }

            if (Sse.IsSupported)
            {
                var lo = Sse.LoadVector128((float*)p);
                return lo.AsDouble().ToVector256();
            }

            return SoftwareFallback(p);

            static HwVectorAnyD SoftwareFallback(double* p)
            {
                return Vector256.Create(p[0], p[1], 0f, 0f);
            }
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD LoadScalar(this double scalar)
        {
            return Vector256.CreateScalar(scalar);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD LoadScalarBroadcast(this double scalar)
        {
            return Vector256.Create(scalar);
        }

        #endregion

        #region Stores

        public static void Store4DAligned(this HwVector4D vector, double* destination) 
            => Store4D(vector, destination);

        public static void Store3DAligned(this HwVector3D vector, double* destination)
            => Store4DAligned((HwVector4D)vector, destination);

        public static void Store2DAligned(this HwVector2D vector, double* destination)
            => Store4DAligned((HwVector4D)vector, destination);


        public static void Store4D(this HwVector4D vector, double* destination)
        {
            if (Avx.IsSupported)
            {
                Avx.Store(destination, vector);

                return;
            }
            if (Sse2.IsSupported)
            {
                Sse2.Store(&destination[0], vector.Value.GetLower());
                Sse2.Store(&destination[2], vector.Value.GetUpper());

                return;
            }

            if (Sse.IsSupported)
            {
                Sse.Store((float*)&destination[0], vector.Value.GetLower().AsSingle());
                Sse.Store((float*)&destination[2], vector.Value.GetUpper().AsSingle());

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector256<double> vector, double* destination)
            {
                destination[0] = X(vector);
                destination[1] = Y(vector);
                destination[2] = Z(vector);
                destination[3] = W(vector);
            }
        }

        public static void Store3D(this HwVector3D vector, double* destination)
        {
            if (Avx.IsSupported)
            {
               var hiBroadcast = Sse2.Shuffle(vector.Value.GetLower(), vector.Value.GetLower(), ShuffleValues._2_2_2_2);

                Sse2.Store(destination, vector.Value.GetLower());
                Sse2.StoreScalar(&destination[3], hiBroadcast);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector256<double> vector, double* destination)
            {
                destination[0] = X(vector);
                destination[1] = Y(vector);
                destination[2] = Z(vector);
            }
        }

        public static void Store2D(this HwVector2D vector, double* destination)
        {
            if (Sse2.IsSupported)
            {
                Sse2.Store(destination, vector.Value.GetLower());

                return;
            }

            if (Sse.IsSupported)
            {
                Sse.Store((float*)destination, vector.Value.GetLower().AsSingle());

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector256<double> vector, double* destination)
            {
                destination[0] = X(vector);
                destination[1] = Y(vector);
            }
        }

        public static void StoreScalar(this Vector256<double> scalar, double* destination)
        {
            *destination = scalar.ToScalar();
        }

        // remove pinning codegen as is unnecessary
        public static void StoreScalar(this Vector256<double> scalar, out double destination)
        {
            destination = scalar.ToScalar();
        }

        #endregion

        #region Movement


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyD ScalarToVector(Vector4D scalar)
        {
            if (Avx2.IsSupported)
            {
                // TODO is this path better than Avx path or the same?
                return Avx2.BroadcastScalarToVector256(scalar.GetLower());
            }
            else if (Avx.IsSupported)
            {
                return Avx.Permute(scalar, 0b_0000_0000);
            }

            return SoftwareFallback(scalar);

            static HwVectorAnyD SoftwareFallback(Vector4D scalar)
            {
                return Vector256.Create(X(scalar));
            }

        }
        #endregion
    }
}
