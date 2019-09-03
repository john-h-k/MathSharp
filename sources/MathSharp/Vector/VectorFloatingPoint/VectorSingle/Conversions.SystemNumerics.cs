using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;

namespace MathSharp
{
    public static unsafe partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static HwVector4 Load(this Vector4 vector)
        {
            return Load4D(&vector.X);
        }

        [MethodImpl(MaxOpt)]
        public static HwVector3 Load(this Vector3 vector)
        {
            return Load3DAligned(&vector.X); // Vector3 is special cased to be 16 bytes on the stack to allow this
        }

        [MethodImpl(MaxOpt)]
        public static HwVector2 Load(this Vector2 vector)
        {
            return Load2D(&vector.X);
        }

        [MethodImpl(MaxOpt)]
        public static HwVector4 Load(this Vector3 vector, float scalarW)
        {
            return Load3D(&vector.X, scalarW);
        }

        public static void Store(this HwVector4 vector, out Vector4 destination)
        {
            fixed (void* p = &destination)
            {
                Store4D(vector, (float*)p);
            }
        }

        public static void Store(this HwVector3 vector, out Vector3 destination)
        {
            fixed (void* p = &destination)
            {
                Store3D(vector, (float*)p);
            }
        }

        public static void Store(this HwVector2 vector, out Vector2 destination)
        {
            fixed (void* p = &destination)
            {
                Store2D(vector, (float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out float destination)
        {
            StoreScalar(vector, out destination);
        }
    }
}
