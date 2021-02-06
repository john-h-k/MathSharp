using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MathSharp.StorageTypes
{
    public readonly struct Vector2D : IEquatable<Vector2D>, IEquatable<Vector2DAligned>
    {
        public readonly double X, Y;

        public Vector2D(Vector2D xy) => this = xy;

        public unsafe Vector2D(Vector2DAligned xy) => this = *(Vector2D*)&xy;

        public Vector2D(double xy) : this(xy, xy) { }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
            => obj is Vector2D other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public bool Equals(Vector2D other) => X.Equals(other.X) && Y.Equals(other.Y);
        public bool Equals(Vector2DAligned other) => Equals(new Vector2D(other));

        public override unsafe string? ToString()
        {
            fixed (Vector2D* p = &this)
            {
                return Vector.ToString(VectorExtensions.ToVector256(p), elemCount: 2);
            }
        }
    }

    public readonly struct Vector3D : IEquatable<Vector3D>, IEquatable<Vector3DAligned>
    {
        public readonly double X, Y, Z;

        public Vector3D(Vector3D xyz) => this = xyz;

        public unsafe Vector3D(Vector3DAligned xyz) => this = *(Vector3D*)&xyz;

        public Vector3D(double xyz) : this(xyz, xyz, xyz) { }

        public Vector3D(Vector2D xy, double z) : this(xy.X, xy.Y, z) { }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object? obj)
            => obj is Vector2D other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);


        public bool Equals(Vector3D other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public bool Equals(Vector3DAligned other) => Equals(new Vector3D(other));


        public override unsafe string? ToString()
        {
            fixed (Vector3D* p = &this)
            {
                return Vector.ToString(VectorExtensions.ToVector256(p), elemCount: 3);
            }
        }
    }


    public readonly struct Vector4D : IEquatable<Vector4D>, IEquatable<Vector4DAligned>
    {
        public readonly double X, Y, Z, W;

        public Vector4D(Vector4D xyzw) => this = xyzw;
        public unsafe Vector4D(Vector4DAligned xy) => this = *(Vector4D*)&xy;

        public Vector4D(double xyzw) : this(xyzw, xyzw, xyzw, xyzw) { }

        public Vector4D(Vector2D xy, Vector2D zw) : this(xy.X, xy.Y, zw.X, zw.Y) { }

        public Vector4D(Vector2D xy, double z, double w) : this(xy.X, xy.Y, z, w) { }

        public Vector4D(Vector3D xyz, double w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public Vector4D(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override bool Equals(object? obj)
            => obj is Vector4D other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

        public bool Equals(Vector4D other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public bool Equals(Vector4DAligned other) => Equals(new Vector4D(other));

        public override unsafe string? ToString()
        {
            fixed (Vector4D* p = &this)
            {
                return Vector.ToString(VectorExtensions.ToVector256(p), elemCount: 4);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public readonly struct Vector2DAligned : IEquatable<Vector2DAligned>, IEquatable<Vector2D>
    {
        public readonly double X, Y;

        public Vector2DAligned(Vector2DAligned xy) => this = xy;
        public Vector2DAligned(Vector2D xy) : this(xy.X, xy.Y) { }

        public Vector2DAligned(double xy) : this(xy, xy) { }

        public Vector2DAligned(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object? obj)
            => obj is Vector2DAligned other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public bool Equals(Vector2DAligned other) => X.Equals(other.X) && Y.Equals(other.Y);
        public bool Equals(Vector2D other) => Equals(new Vector2DAligned(other));

        public override unsafe string? ToString()
        {
            fixed (Vector2DAligned* p = &this)
            {
                return Vector.ToString(VectorExtensions.ToVector256(p), elemCount: 2);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public readonly struct Vector3DAligned : IEquatable<Vector3DAligned>, IEquatable<Vector3D>
    {
        public readonly double X, Y, Z;

        public Vector3DAligned(Vector3DAligned xyz) => this = xyz;
        public Vector3DAligned(Vector3D xyz) : this(xyz.X, xyz.Y, xyz.Z) { }

        public Vector3DAligned(double xyz) : this(xyz, xyz, xyz) { }

        public Vector3DAligned(Vector2D xy, double z) : this(xy.X, xy.Y, z) { }

        public Vector3DAligned(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object? obj)
            => obj is Vector3DAligned other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public bool Equals(Vector3DAligned other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public bool Equals(Vector3D other) => Equals(new Vector3DAligned(other));

        public override unsafe string? ToString()
        {
            fixed (Vector3DAligned* p = &this)
            {
                return Vector.ToString(VectorExtensions.ToVector256(p), elemCount: 3);
            }
        }
    }


    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public readonly struct Vector4DAligned : IEquatable<Vector4DAligned>, IEquatable<Vector4D>
    {
        public readonly double X, Y, Z, W;

        public Vector4DAligned(Vector4DAligned xyzw) => this = xyzw;

        public unsafe Vector4DAligned(Vector4D xyzw) => this = *(Vector4DAligned*)&xyzw;

        public Vector4DAligned(double xyzw) : this(xyzw, xyzw, xyzw, xyzw) { }

        public Vector4DAligned(Vector2D xy, Vector2D zw) : this(xy.X, xy.Y, zw.X, zw.Y) { }

        public Vector4DAligned(Vector2D xy, double z, double w) : this(xy.X, xy.Y, z, w) { }

        public Vector4DAligned(Vector3D xyz, double w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public Vector4DAligned(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override bool Equals(object? obj)
            => obj is Vector4DAligned other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

        public bool Equals(Vector4DAligned other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public bool Equals(Vector4D other) => Equals(new Vector4DAligned(other));

        public override unsafe string? ToString()
        {
            fixed (Vector4DAligned* p = &this)
            {
                return Vector.ToString(VectorExtensions.ToVector256(p), elemCount: 4);
            }
        }
    }

    public static unsafe partial class VectorExtensions
    {
        public static Vector256<double> ToVector256(Vector2D* p) => Vector.Load2((double*)p);
        public static Vector256<double> ToVector256(Vector3D* p) => Vector.Load3((double*)p);
        public static Vector256<double> ToVector256(Vector4D* p) => Vector.Load4((double*)p);

        public static Vector256<double> ToVector256(Vector2DAligned* vector) => Vector.Load2Aligned((double*)vector);
        public static Vector256<double> ToVector256(Vector3DAligned* vector) => Vector.Load3Aligned((double*)vector);
        public static Vector256<double> ToVector256(Vector4DAligned* vector) => Vector.Load4Aligned((double*)vector);

        public static void StoreToVector(this Vector256<double> vector, Vector2D* destination) => vector.Store2(&destination->X);
        public static void StoreToVector(this Vector256<double> vector, out Vector2D destination)
        {
            fixed (Vector2D* p = &destination)
            {
                StoreToVector(vector, p);
            }
        }

        public static void StoreToVector(this Vector256<double> vector, Vector3D* destination) => vector.Store3(&destination->X);
        public static void StoreToVector(this Vector256<double> vector, out Vector3D destination)
        {
            fixed (Vector3D* p = &destination)
            {
                StoreToVector(vector, p);
            }
        }

        public static void StoreToVector(this Vector256<double> vector, Vector4D* destination) => vector.Store4(&destination->X);
        public static void StoreToVector(this Vector256<double> vector, out Vector4D destination)
        {
            fixed (Vector4D* p = &destination)
            {
                StoreToVector(vector, p);
            }
        }

        public static void StoreToVector(this Vector256<double> vector, Vector2DAligned* destination) => Vector.Store2Aligned(vector, &destination->X);
        public static void StoreToVector(this Vector256<double> vector, out Vector2DAligned destination)
        {
            fixed (Vector2DAligned* p = &destination)
            {
                StoreToVector(vector, p);
            }
        }

        public static void StoreToVector(this Vector256<double> vector, Vector3DAligned* destination) => Vector.Store3Aligned(vector, &destination->X);
        public static void StoreToVector(this Vector256<double> vector, out Vector3DAligned destination)
        {
            fixed (Vector3DAligned* p = &destination)
            {
                StoreToVector(vector, p);
            }
        }

        public static void StoreToVector(this Vector256<double> vector, Vector4DAligned* destination) => Vector.Store4Aligned(vector, &destination->X);
        public static void StoreToVector(this Vector256<double> vector, out Vector4DAligned destination)
        {
            fixed (Vector4DAligned* p = &destination)
            {
                StoreToVector(vector, p);
            }
        }
    }
}
