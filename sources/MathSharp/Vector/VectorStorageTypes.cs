using System.Runtime.InteropServices;

namespace MathSharp
{
    public readonly struct Vector2
    {
        public readonly float X, Y;

        public Vector2(Vector2 xy) => this = xy;

        public unsafe Vector2(Vector2Aligned xy) => this = *(Vector2*)&xy;

        public Vector2(float xy) : this(xy, xy) { }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    public readonly struct Vector3
    {
        public readonly float X, Y, Z;

        public Vector3(Vector3 xyz) => this = xyz;

        public unsafe Vector3(Vector3Aligned xyz) => this = *(Vector3*)&xyz;

        public Vector3(float xyz) : this(xyz, xyz, xyz) { }

        public Vector3(Vector2 xy, float z) : this(xy.X, xy.Y, z) { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }


    public readonly struct Vector4
    {
        public readonly float X, Y, Z, W;

        public Vector4(Vector4 xyzw) => this = xyzw;
        public unsafe Vector4(Vector4Aligned xy) => this = *(Vector4*)&xy;

        public Vector4(float xyzw) : this(xyzw, xyzw, xyzw, xyzw) { }

        public Vector4(Vector2 xy, Vector2 zw) : this(xy.X, xy.Y, zw.X, zw.Y) { }

        public Vector4(Vector2 xy, float z, float w) : this(xy.X, xy.Y, z, w) { }

        public Vector4(Vector3 xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 32)]
    public readonly struct Vector2Aligned
    {
        public readonly float X, Y;

        public Vector2Aligned(Vector2Aligned xy) => this = xy;
        public Vector2Aligned(Vector2 xy) : this(xy.X, xy.Y) { }


        public Vector2Aligned(float xy) : this(xy, xy) { }

        public Vector2Aligned(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 32)]
    public readonly struct Vector3Aligned
    {
        public readonly float X, Y, Z;

        public Vector3Aligned(Vector3Aligned xyz) => this = xyz;
        public Vector3Aligned(Vector3 xyz) : this(xyz.X, xyz.Y, xyz.Z) { }

        public Vector3Aligned(float xyz) : this(xyz, xyz, xyz) { }

        public Vector3Aligned(Vector2 xy, float z) : this(xy.X, xy.Y, z) { }

        public Vector3Aligned(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }


    [StructLayout(LayoutKind.Sequential, Size = 32)]
    public readonly struct Vector4Aligned
    {
        public readonly float X, Y, Z, W;

        public Vector4Aligned(Vector4Aligned xyzw) => this = xyzw;

        public unsafe Vector4Aligned(Vector4 xyzw) => this = *(Vector4Aligned*)&xyzw;

        public Vector4Aligned(float xyzw) : this(xyzw, xyzw, xyzw, xyzw) { }

        public Vector4Aligned(Vector2 xy, Vector2 zw) : this(xy.X, xy.Y, zw.X, zw.Y) { }

        public Vector4Aligned(Vector2 xy, float z, float w) : this(xy.X, xy.Y, z, w) { }

        public Vector4Aligned(Vector3 xyz, float w) : this(xyz.X, xyz.Y, xyz.Z, w) { }

        public Vector4Aligned(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}
