using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp
{
    using Vector4D = Vector256<double>;

    // Used for overload resolution. All conversions are nops and the codegen around them is good

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector2D
    {
        public readonly Vector4D Value;

        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}>";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HwVector2D(Vector4D value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4D(HwVector2D vector) => vector.Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVector2D(Vector4D vector) => new HwVector2D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVectorAnyD(HwVector2D vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector3D(HwVector2D vector) => new HwVector3D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector4D(HwVector2D vector) => new HwVector4D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector2D(HwVectorAnyD vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator +(HwVector2D left, HwVector2D right) => Vector.Add(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator -(HwVector2D left, HwVector2D right) => Vector.Subtract(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator /(HwVector2D left, HwVector2D right) => Vector.Divide(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator *(HwVector2D left, HwVector2D right) => Vector.Multiply(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator -(HwVector2D vector) => Vector.Negate(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator &(HwVector2D left, HwVector2D right) => Vector.And(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator |(HwVector2D left, HwVector2D right) => Vector.Or(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator ^(HwVector2D left, HwVector2D right) => Vector.Xor(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2D operator ~(HwVector2D vector) => Vector.Not(vector);


        public HwVectorAnyD Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.Length(this);
        }

        public HwVectorAnyD LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.LengthSquared(this);
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector3D
    {
        public readonly Vector4D Value;

        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}>";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HwVector3D(Vector4D value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4D(HwVector3D vector) => vector.Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVector3D(Vector4D vector) => new HwVector3D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVectorAnyD(HwVector3D vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector2D(HwVector3D vector) => new HwVector2D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector4D(HwVector3D vector) => new HwVector4D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector3D(HwVectorAnyD vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator +(HwVector3D left, HwVector3D right) => Vector.Add(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator -(HwVector3D left, HwVector3D right) => Vector.Subtract(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator /(HwVector3D left, HwVector3D right) => Vector.Divide(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator *(HwVector3D left, HwVector3D right) => Vector.Multiply(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator -(HwVector3D vector) => Vector.Negate(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator &(HwVector3D left, HwVector3D right) => Vector.And(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator |(HwVector3D left, HwVector3D right) => Vector.Or(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator ^(HwVector3D left, HwVector3D right) => Vector.Xor(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3D operator ~(HwVector3D vector) => Vector.Not(vector);


        public HwVectorAnyD Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.Length(this);
        }

        public HwVectorAnyD LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.LengthSquared(this);
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector4D
    {
        public readonly Vector4D Value;

        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}, {Value.GetElement(3)}>";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HwVector4D(Vector4D value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4D(HwVector4D vector) => vector.Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVector4D(Vector4D vector) => new HwVector4D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVectorAnyD(HwVector4D vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector2D(HwVector4D vector) => new HwVector2D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector3D(HwVector4D vector) => new HwVector3D(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector4D(HwVectorAnyD vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator +(HwVector4D left, HwVector4D right) => Vector.Add(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator -(HwVector4D left, HwVector4D right) => Vector.Subtract(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator /(HwVector4D left, HwVector4D right) => Vector.Divide(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator *(HwVector4D left, HwVector4D right) => Vector.Multiply(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator -(HwVector4D vector) => Vector.Negate(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator &(HwVector4D left, HwVector4D right) => Vector.And(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator |(HwVector4D left, HwVector4D right) => Vector.Or(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator ^(HwVector4D left, HwVector4D right) => Vector.Xor(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4D operator ~(HwVector4D vector) => Vector.Not(vector);


        public HwVectorAnyD Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.Length(this);
        }

        public HwVectorAnyD LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.LengthSquared(this);
        }
    }

    public readonly struct HwVectorAnyD
    {
        public readonly Vector256<double> Value;

        public HwVectorAnyD(Vector256<double> value)
        {
            Value = value;
        }

        public static implicit operator Vector4D(HwVectorAnyD vector) => vector.Value;
        public static implicit operator HwVectorAnyD(Vector4D vector) => new HwVectorAnyD(vector);
    }
}
