using System;
using System.Runtime.Intrinsics;
using MathSharp.StorageTypes;

namespace MathSharp.Quaternion
{
    public readonly struct QuaternionF : IEquatable<QuaternionF>
    {
        public readonly float X, Y, Z, W;

        public QuaternionF(QuaternionF xyzw) => this = xyzw;

        public QuaternionF(float xyzw) : this(xyzw, xyzw, xyzw, xyzw) { }

        public QuaternionF(Vector2F xy, Vector2F zw) : this(xy.X, xy.Y, zw.X, zw.Y) { }

        public QuaternionF(Vector2F xy, float z, float w) : this(xy.X, xy.Y, z, w) { }

        public QuaternionF(Vector3F xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public QuaternionF(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override bool Equals(object? obj)
            => obj is QuaternionF other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(QuaternionF other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

        public override unsafe string? ToString()
        {
            fixed (QuaternionF* p = &this)
            {
                return Vector.ToString(QuaternionExtensions.ToVector128(p), elemCount: 4);
            }
        }
    }

    public static unsafe partial class QuaternionExtensions
    {
        public static Vector128<float> ToVector128(QuaternionF* p) => Vector.FromVector4D((float*)p);

        public static void StoreToQuaternion(Vector128<float> quaternion, QuaternionF* destination) => Vector.ToVector4D(quaternion, (float*)destination);

        public static void StoreToQuaternion(Vector128<float> quaternion, out QuaternionF destination)
        {
            fixed (QuaternionF* p = &destination)
            {
                StoreToQuaternion(quaternion, p);
            }
        }
    }
}
