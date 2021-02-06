using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using SysVector2 = System.Numerics.Vector2;

namespace MathSharp
{
    public static unsafe partial class Vector
    {
        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load(this Vector4 vector)
        {
            return Load4(&vector.X);
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load(this Vector3 vector)
        {
            return Load3Aligned(&vector.X); // Vector3 is special cased to be 16 bytes on the stack to allow this
        }

        [MethodImpl(MaxOpt)]
        public static Vector128<float> Load(this SysVector2 vector)
        {
            return Load2(&vector.X);
        }

        public static void Store(this Vector128<float> vector, out Vector4 destination)
        {
            fixed (void* p = &destination)
            {
                Store4(vector, (float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out Vector3 destination)
        {
            fixed (void* p = &destination)
            {
                Store3(vector, (float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out SysVector2 destination)
        {
            fixed (void* p = &destination)
            {
                Store2(vector, (float*)p);
            }
        }

        public static void Store(this Vector128<float> vector, out float destination)
        {
            StoreScalar(vector, out destination);
        }
    }
}
