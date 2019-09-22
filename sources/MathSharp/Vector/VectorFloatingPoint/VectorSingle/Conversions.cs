using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Attributes;
using MathSharp.Utils;
using Microsoft.VisualBasic.CompilerServices;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    using Vector4F = Vector128<float>;

    public static unsafe partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static byte ExtractMask(Vector4F vector)
        {
            if (Sse.IsSupported)
            {
                return (byte)Sse.MoveMask(vector);
            }

            return SoftwareFallback(vector);

            static byte SoftwareFallback(Vector4F vector)
            {
                float s0 = X(vector);
                float s1 = Y(vector);
                float s2 = Z(vector);
                float s3 = W(vector);

                int e0 = Unsafe.As<float, int>(ref s0);
                int e1 = Unsafe.As<float, int>(ref s1);
                int e2 = Unsafe.As<float, int>(ref s2);
                int e3 = Unsafe.As<float, int>(ref s3);

                return (byte)(SignAsByteBool(e0) | (SignAsByteBool(e1) << 1) | (SignAsByteBool(e2) << 2) | (SignAsByteBool(e3) << 3));
            }

            static byte SignAsByteBool(int i) => i < 0 ? (byte)1 : (byte)0;
        }

        #region Loads

        [MethodImpl(MaxOpt)]
        public static HwVector4S Load4DAligned(float* p)
        {
            if (Sse.IsSupported)
            {
                return Sse.LoadAlignedVector128(p);
            }

            return SoftwareFallback(p);

            static HwVectorAnyS SoftwareFallback(float* p)
            {
                ThrowHelper.ThrowForUnaligned16BPointer(p, "Pointer not 16 byte aligned");

                return Vector128.Create(p[0], p[1], p[2], p[3]);
            }
        }

        public static HwVector3S Load3DAligned(float* p) => (HwVector3S)Load4DAligned(p);
        public static HwVector2S Load2DAligned(float* p) => (HwVector2S)Load4DAligned(p);


        [MethodImpl(MaxOpt)]
        public static HwVector4S Load4D(float* p)
        {
            if (Sse.IsSupported)
            {
                return Sse.LoadVector128(p);
            }

            return SoftwareFallback(p);

            static HwVectorAnyS SoftwareFallback(float* p)
            {
                return Vector128.Create(p[0], p[1], p[2], p[3]);
            }
        }

        [MethodImpl(MaxOpt)]
        public static HwVector3S Load3D(float* p)
        {
            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                Vector4F lo = Sse.LoadScalarVector128(&p[0]);
                Vector4F mid = Sse.LoadScalarVector128(&p[1]);
                Vector4F hi = Sse.LoadScalarVector128(&p[2]);
                hi = Vector.ZeroW(hi);

                // Construct a vector of (lo, mid, ?, ?)
                Vector4F loMid = Sse.UnpackLow(lo, mid);

                // Given the hi vector is zeroed (because of the ZeroW), the first two elements are (hi, 0)
                // Move these 2 values to the last 2 elements of the loMid vector
                // This results in (lo, mid, hi, 0), the desired vector
                return Sse.MoveLowToHigh(loMid, hi);
            }

            return SoftwareFallback(p);

            static HwVectorAnyS SoftwareFallback(float* p)
            {
                return Vector128.Create(p[0], p[1], p[2], 0);
            }
        }

        [MethodImpl(MaxOpt)]
        public static HwVector4S Load3D(float* p, float w)
        {
            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                Vector4F lo = Sse.LoadScalarVector128(&p[0]);
                Vector4F mid = Sse.LoadScalarVector128(&p[1]);
                Vector4F hi = Sse.LoadScalarVector128(&p[2]);
                Vector4F wVec = Vector128.CreateScalarUnsafe(w);

                // Construct a vector of (lo, mid, ?, ?)
                lo = Sse.UnpackLow(lo, mid);
                hi = Sse.UnpackLow(hi, wVec);

                // Given the hi vector is zeroed (because of the ZeroW), the first two elements are (hi, 0)
                // Move these 2 values to the last 2 elements of the loMid vector
                // This results in (lo, mid, hi, 0), the desired vector
                return Sse.MoveLowToHigh(lo, hi);
            }

            return SoftwareFallback(p, w);

            static HwVectorAnyS SoftwareFallback(float* p, float w)
            {
                return Vector128.Create(p[0], p[1], p[2], w);
            }
        }

        [MethodImpl(MaxOpt)]
        public static HwVector2S Load2D(float* p)
        {
            if (Sse.IsSupported)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being 0
                Vector4F lo = Sse.LoadScalarVector128(&p[0]);
                Vector4F hi = Sse.LoadScalarVector128(&p[1]);

                // Unpack these to (lo, mid, 0, 0), the desired vector
                return Sse.UnpackLow(lo, hi);
            }

            return SoftwareFallback(p);

            static HwVectorAnyS SoftwareFallback(float* p)
            {
                return Vector128.Create(p[0], p[1], 0f, 0f);
            }
        }
        
        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS LoadScalar(this float scalar)
        {
            return Vector128.CreateScalar(scalar);
        }

        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS LoadScalarBroadcast(this float scalar)
        {
            return Vector128.Create(scalar);
        }

        #endregion

        #region Stores

        public static void Store4DAligned(this HwVector4S vector, float* destination)
        {
            if (Sse.IsSupported)
            {
                Sse.StoreAligned(destination, vector);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector128<float> vector, float* destination)
            {
                ThrowHelper.ThrowForUnaligned16BPointer(destination, "Pointer not 16 byte aligned");

                destination[0] = X(vector);
                destination[1] = Y(vector);
                destination[2] = Z(vector);
                destination[3] = W(vector);
            }
        }

        public static void Store3DAligned(this HwVector3S vector, float* destination)
            => Store4DAligned((HwVector4S)vector, destination);

        public static void Store2DAligned(this HwVector2S vector, float* destination) 
            => Store4DAligned((HwVector4S)vector, destination);


        public static void Store4D(this HwVector4S vector, float* destination)
        {
            if (Sse.IsSupported)
            {
                Sse.Store(destination, vector);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector128<float> vector, float* destination)
            {
                destination[0] = X(vector);
                destination[1] = Y(vector);
                destination[2] = Z(vector);
                destination[3] = W(vector);
            }
        }

        public static void Store3D(this HwVector3S vector, float* destination)
        {
            if (Sse.IsSupported)
            {
                Vector4F hiBroadcast = Sse.Shuffle(vector, vector, ShuffleValues._2_2_2_2);

                Sse.StoreLow(destination, vector);
                Sse.StoreScalar(&destination[3], hiBroadcast);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector128<float> vector, float* destination)
            {
                destination[0] = X(vector);
                destination[1] = Y(vector);
                destination[2] = Z(vector);
            }
        }

        public static void Store2D(this HwVector2S vector, float* destination)
        {
            if (Sse.IsSupported)
            {
                Sse.StoreLow(destination, vector);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector128<float> vector, float* destination)
            {
                destination[0] = X(vector);
                destination[1] = Y(vector);
            }
        }

        public static void StoreScalar(this Vector128<float> scalar, float* destination)
        {
            *destination = scalar.ToScalar();
        }

        // remove pinning codegen as is unnecessary
        public static void StoreScalar(this Vector128<float> scalar, out float destination)
        {
            destination = scalar.ToScalar();
        }

        #endregion

        #region Movement


        [MethodImpl(MaxOpt)]
        public static HwVectorAnyS ScalarToVector(Vector4F scalar)
        {
            if (Avx2.IsSupported)
            {
                // TODO is this path better than Avx path or the same?
                return Avx2.BroadcastScalarToVector128(scalar);
            }
            else if (Avx.IsSupported)
            {
                return Avx.Permute(scalar, 0b_0000_0000);
            }
            else if (Sse.IsSupported)
            {
                return Sse.Shuffle(scalar, scalar, 0b_0000_0000);
            }

            return SoftwareFallback(scalar);

            static HwVectorAnyS SoftwareFallback(Vector4F scalar)
            {
                return Vector128.Create(X(scalar));
            }

        }
        #endregion
    }
}
