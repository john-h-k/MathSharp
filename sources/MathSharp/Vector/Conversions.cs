using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using static MathSharp.Utils.Helpers;

namespace MathSharp
{
    public static unsafe partial class Vector
    {
        #region Loads

        [MethodImpl(MaxOpt)]
        public static Vector128<U> Load2As<T, U>(in T p) where T : unmanaged where U : unmanaged
        {
            if (TypeHelper.IsType<U, float>())
            {
                return Load2(TypeHelper.As<T, float>(in p)).As<float, U>();
            }
            if (TypeHelper.IsType<U, double>())
            {
                return Load2(TypeHelper.As<T, double>(in p)).As<double, U>();
            }

            TypeHelper.ThrowForUnsupportedType<T>();
            return default;
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load4Aligned(in float p)
        {
            return Unsafe.As<float, Vector128<float>>(ref Unsafe.AsRef(in p));
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load4Aligned(float* p) => Load4(p);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load3Aligned(in float p) => Load4Aligned(in p);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load3Aligned(float* p) => Load4Aligned(p);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load2Aligned(in float p) => Load4Aligned(in p);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load2Aligned(float* p)  => Load4Aligned(p);

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load4(in float p) => Unsafe.As<float, Vector128<float>>(ref Unsafe.AsRef(in p));

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load4(float* p)
        {
            if (Sse.IsSupported)
            {
                return Sse.LoadVector128(p);
            }

            return SoftwareFallback(p);

            static Vector128<float> SoftwareFallback(float* p)
            {
                return Vector128.Create(p[0], p[1], p[2], p[3]);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load3(in float p)
        {
            fixed (float* pp = &p)
            {
                return Load3(pp);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load3(float* p)
        {
            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                Vector128<float> hi = Sse.LoadScalarVector128(&p[2]);
                hi = And(hi, SingleConstants.MaskY);
                return Sse.LoadLow(hi, p);
            }

            return SoftwareFallback(p);

            static Vector128<float> SoftwareFallback(float* p)
            {
                return Vector128.Create(p[0], p[1], p[2], 0);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load2(in float p)
        {
            return Vector128.CreateScalarUnsafe(Unsafe.As<float, double>(ref Unsafe.AsRef(in p))).AsSingle();
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load2(float* p)
        {
            if (Sse2.IsSupported)
            {
                return Sse2.LoadScalarVector128((double*)p).AsSingle();
            }
            if (Sse.IsSupported)
            {
                // Construct 2 separate vectors, each having the first element being the value
                // and the rest being undefined
                Vector128<float> upper = SingleConstants.Zero;

                return Sse.LoadLow(upper, p);
            }

            return SoftwareFallback(p);

            static Vector128<float> SoftwareFallback(float* p)
            {
                return Vector128.Create(p[0], p[1], 0f, 0f);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load4Aligned(double* p) => Load4Aligned(in *p);
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load3Aligned(double* p) => Load4Aligned(in *p);
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load2Aligned(double* p) => Load4Aligned(in *p);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load4Aligned(in double p) => Load4(p);
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load3Aligned(in double p) => Load4(p);
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load2Aligned(in double p) => Load4(p);

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load4(in double p) => Unsafe.As<double, Vector256<double>>(ref Unsafe.AsRef(in p));
        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load3(in double p)
        {
            fixed (double* dp = &p)
            {
                return Load3(dp);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<double> Load2(in double p) => Unsafe.As<double, Vector128<double>>(ref Unsafe.AsRef(in p));


        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load4(double* p)
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

            static Vector256<double> SoftwareFallback(double* p)
            {
                return Vector256.Create(p[0], p[1], p[2], p[3]);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load3(double* p)
        {
            if (Sse2.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                var lo = Sse2.LoadVector128(p);
                var hi = Sse2.LoadScalarVector128(&p[2]);
                hi = And(hi, DoubleConstants.MaskY.GetLower());

                return Vector256.Create(lo, hi);
            }

            if (Sse.IsSupported)
            {
                // Construct 3 separate vectors, each with the first element being the value
                // and the rest being undefined (shown as ?)
                var lo = Sse.LoadVector128((float*)p).AsDouble();
                var hi = Sse.LoadLow(Vector128<float>.Zero, (float*)&p[2]).AsDouble();
                hi = And(hi.AsSingle(), SingleConstants.MaskZW).AsDouble();

                return Vector256.Create(lo, hi);
            }

            return SoftwareFallback(p);

            static Vector256<double> SoftwareFallback(double* p)
            {
                return Vector256.Create(p[0], p[1], p[2], 0);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> Load2(double* p)
        {
            if (Sse2.IsSupported)
            {
                var lo = Sse2.LoadVector128(p);
                return lo.ToVector256();
            }

            if (Sse.IsSupported)
            {
                var lo = Sse.LoadVector128((float*)p);
                return lo.AsDouble().ToVector256();
            }

            return SoftwareFallback(p);

            static Vector256<double> SoftwareFallback(double* p)
            {
                return Vector128.Create(p[0]).ToVector256();
                //return Vector256.Create(p[0], p[1], 0f, 0f);
            }
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> LoadScalar(this double scalar)
        {
            return Vector256.CreateScalar(scalar);
        }

        [MethodImpl(MaxOpt)]
        public static Vector256<double> LoadScalarBroadcast(this double scalar)
        {
            return Vector256.Create(scalar);
        }

        #endregion

        #region Stores

        [MethodImpl(MaxOpt)]
        public static void Store2As<T, U>(this Vector128<T> vector, ref U destination) where T : unmanaged where U : unmanaged
        {
            if (TypeHelper.IsType<U, float>())
            {
                Store2(vector.As<T, float>(), ref Unsafe.As<U, float>(ref destination));
            }
            if (TypeHelper.IsType<U, double>())
            {
                Store2(vector.As<T, double>(), ref Unsafe.As<U, double>(ref destination));
            }

            TypeHelper.ThrowForUnsupportedType<T>();
            destination = default;
        }

        public static void Store4Aligned(this Vector128<float> vector, out float destination)
            => Store4(vector, out destination);
        public static void Store4Aligned(this Vector128<float> vector, float* destination)
            => Store4(vector, destination);

        public static void Store3Aligned(this Vector128<float> vector, out float destination)
            => Store4Aligned(vector, out destination);
        public static void Store3Aligned(this Vector128<float> vector, float* destination)
            => Store4Aligned(vector, destination);

        public static void Store2Aligned(this Vector128<float> vector, out float destination)
            => Store4Aligned(vector, out destination);
        public static void Store2Aligned(this Vector128<float> vector, float* destination) 
            => Store4Aligned(vector, destination);

        [MethodImpl(MaxOpt)]
        public static void Store4(this Vector128<float> vector, out float destination)
        {
            fixed (void* _ = &destination) { } // TODO use Unsafe.SkipInit<T>(out T);
            Unsafe.As<float, Vector128<float>>(ref destination) = vector;
        }

        [MethodImpl(MaxOpt)]
        public static void Store4(this Vector128<float> vector, float* destination)
        {
            if (Sse.IsSupported)
            {
                Sse.Store(destination, vector);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector128<float> vector, float* destination)
            {
                destination[0] = vector.GetElement(0);
                destination[1] = vector.GetElement(1);
                destination[2] = vector.GetElement(2);
                destination[3] = vector.GetElement(3);
            }
        }

        [MethodImpl(MaxOpt)]
        public static void Store3(this Vector128<float> vector, ref float destination)
        {
            fixed (float* p = &destination)
            {
                Store3(vector, p);
            }
        }

        [MethodImpl(MaxOpt)]
        public static void Store3(this Vector128<float> vector, float* destination)
        {
            if (Sse.IsSupported)
            {
                Vector128<float> hiBroadcast = Sse.Shuffle(vector, vector, ShuffleValues.ZZZZ);

                Sse.StoreLow(destination, vector);
                Sse.StoreScalar(&destination[3], hiBroadcast);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector128<float> vector, float* destination)
            {
                destination[0] = vector.GetElement(0);
                destination[1] = vector.GetElement(1);
                destination[2] = vector.GetElement(2);
            }
        }

        [MethodImpl(MaxOpt)]
        public static void Store2(this Vector128<float> vector, ref float destination)
        {
            Unsafe.As<float, Vector128<float>>(ref destination) = vector;
            //fixed (float* p = &destination)
            //{
            //    Store2(vector, p);
            //}
        }

        [MethodImpl(MaxOpt)]
        public static void Store2(this Vector128<float> vector, float* destination)
        {
            if (Sse.IsSupported)
            {
                Sse.StoreLow(destination, vector);

                return;
            }

            SoftwareFallback(vector, destination);

            static void SoftwareFallback(Vector128<float> vector, float* destination)
            {
                destination[0] = vector.GetElement(0);
                destination[1] = vector.GetElement(1);
            }
        }

        [MethodImpl(MaxOpt)]
        public static void StoreScalar(Vector128<float> scalar, float* destination)
        {
            *destination = scalar.ToScalar();
        }

        // remove pinning codegen as is unnecessary
        [MethodImpl(MaxOpt)]
        public static void StoreScalar(Vector128<float> scalar, out float destination)
        {
            destination = scalar.ToScalar();
        }

        public static void Store4Aligned(this Vector256<double> vector, double* destination)
           => Store4(vector, destination);

        public static void Store3Aligned(this Vector256<double> vector, double* destination)
            => Store4Aligned(vector, destination);

        public static void Store2Aligned(this Vector256<double> vector, double* destination)
            => Store4Aligned(vector, destination);


        public static void Store4(this Vector256<double> vector, double* destination)
        {
            if (Avx.IsSupported)
            {
                Avx.Store(destination, vector);

                return;
            }
            if (Sse2.IsSupported)
            {
                Sse2.Store(&destination[0], vector.GetLower());
                Sse2.Store(&destination[2], vector.GetUpper());

                return;
            }

            if (Sse.IsSupported)
            {
                Sse.Store((float*)&destination[0], vector.GetLower().AsSingle());
                Sse.Store((float*)&destination[2], vector.GetUpper().AsSingle());

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

        public static void Store3(this Vector256<double> vector, double* destination)
        {
            if (Avx.IsSupported)
            {
                var hiBroadcast = Sse2.Shuffle(vector.GetLower(), vector.GetLower(), ShuffleValues.ZZZZ);

                Sse2.Store(destination, vector.GetLower());
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


        public static void Store2(this Vector128<double> vector, ref double destination)
            => Unsafe.As<double, Vector128<double>>(ref destination) = vector;

        public static void Store2(this Vector256<double> vector, ref double destination)
            => Unsafe.As<double, Vector128<double>>(ref destination) = vector.GetLower();

        public static void Store2(this Vector256<double> vector, double* destination)
        {
            if (Sse2.IsSupported)
            {
                Sse2.Store(destination, vector.GetLower());

                return;
            }

            if (Sse.IsSupported)
            {
                Sse.Store((float*)destination, vector.GetLower().AsSingle());

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
        public static Vector256<double> ScalarToVector(Vector256<double> scalar)
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

            static Vector256<double> SoftwareFallback(Vector256<double> scalar)
            {
                return Vector256.Create(X(scalar));
            }

        }

        #endregion

        #region Movement


        [MethodImpl(MaxOpt)]
        public static Vector128<float> ScalarToVector(Vector128<float> scalar)
        {
            if (Avx2.IsSupported)
            {
                // TODO is path better than Avx path or the same?
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

            static Vector128<float> SoftwareFallback(Vector128<float> scalar)
            {
                return Vector128.Create(X(scalar));
            }

        }
        #endregion
    }
}
