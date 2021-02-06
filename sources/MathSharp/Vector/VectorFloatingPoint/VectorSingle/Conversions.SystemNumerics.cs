using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp
{
    public static unsafe partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load(this in Vector4 vector)
        {
            return FromVector4D(in vector.X);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load(this in Vector3 vector)
        {
            return FromVector3DAligned(in vector.X); // Vector3 is special cased to be 16 bytes on the stack to allow this
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load(this in Vector2 vector)
        {
            return FromVector2D(in vector.X);
        }

        public static void Store(this Vector128<float> vector, out Vector4 destination)
        {
            fixed (void* p = &destination)
            {
                ToVector4D(vector, (float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out Vector3 destination)
        {
            fixed (void* p = &destination)
            {
                ToVector3D(vector, (float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out Vector2 destination)
        {
            fixed (void* p = &destination)
            {
                ToVector2D(vector, (float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out float destination)
        {
            StoreScalar(vector, out destination);
        }
    }
}
