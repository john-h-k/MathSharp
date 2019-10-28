using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using MathSharp.StorageTypes;

namespace MathSharp
{
    // ReSharper disable once InconsistentNaming
    public readonly struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        public readonly float E11;
        public readonly float E12;
        public readonly float E13;
        public readonly float E14;

        public readonly float E21;
        public readonly float E22;
        public readonly float E23;
        public readonly float E24;

        public readonly float E31;
        public readonly float E32;
        public readonly float E33;
        public readonly float E34;

        public readonly float E41;
        public readonly float E42;
        public readonly float E43;
        public readonly float E44;

        public Matrix4x4(float e11, float e12, float e13, float e14, float e21, float e22, float e23, float e24, float e31, float e32, float e33, float e34, float e41, float e42, float e43, float e44)
        {
            E11 = e11;
            E12 = e12;
            E13 = e13;
            E14 = e14;
            E21 = e21;
            E22 = e22;
            E23 = e23;
            E24 = e24;
            E31 = e31;
            E32 = e32;
            E33 = e33;
            E34 = e34;
            E41 = e41;
            E42 = e42;
            E43 = e43;
            E44 = e44;
        }

        public Matrix4x4(Vector4F e1, Vector4F e2, Vector4F e3, Vector4F e4)
        {
            // TODO use Unsafe.SkipInit<T>(out T)
            this = default;
            Unsafe.As<float, Vector4F>(ref Unsafe.AsRef(in E11)) = e1;
            Unsafe.As<float, Vector4F>(ref Unsafe.AsRef(in E21)) = e2;
            Unsafe.As<float, Vector4F>(ref Unsafe.AsRef(in E31)) = e3;
            Unsafe.As<float, Vector4F>(ref Unsafe.AsRef(in E41)) = e4;
        }

        public bool Equals(Matrix4x4 other)
        {
            return E11.Equals(other.E11) 
                   && E12.Equals(other.E12) 
                   && E13.Equals(other.E13) 
                   && E14.Equals(other.E14) 
                   && E21.Equals(other.E21) 
                   && E22.Equals(other.E22) 
                   && E23.Equals(other.E23) 
                   && E24.Equals(other.E24) 
                   && E31.Equals(other.E31) 
                   && E32.Equals(other.E32) 
                   && E33.Equals(other.E33) 
                   && E34.Equals(other.E34) 
                   && E41.Equals(other.E41) 
                   && E42.Equals(other.E42) 
                   && E43.Equals(other.E43) 
                   && E44.Equals(other.E44);
        }

        public override bool Equals(object? obj) => obj is Matrix4x4 other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = E11.GetHashCode();
                hashCode = (hashCode * 397) ^ E12.GetHashCode();
                hashCode = (hashCode * 397) ^ E13.GetHashCode();
                hashCode = (hashCode * 397) ^ E14.GetHashCode();
                hashCode = (hashCode * 397) ^ E21.GetHashCode();
                hashCode = (hashCode * 397) ^ E22.GetHashCode();
                hashCode = (hashCode * 397) ^ E23.GetHashCode();
                hashCode = (hashCode * 397) ^ E24.GetHashCode();
                hashCode = (hashCode * 397) ^ E31.GetHashCode();
                hashCode = (hashCode * 397) ^ E32.GetHashCode();
                hashCode = (hashCode * 397) ^ E33.GetHashCode();
                hashCode = (hashCode * 397) ^ E34.GetHashCode();
                hashCode = (hashCode * 397) ^ E41.GetHashCode();
                hashCode = (hashCode * 397) ^ E42.GetHashCode();
                hashCode = (hashCode * 397) ^ E43.GetHashCode();
                hashCode = (hashCode * 397) ^ E44.GetHashCode();
                return hashCode;
            }
        }
    }
}
