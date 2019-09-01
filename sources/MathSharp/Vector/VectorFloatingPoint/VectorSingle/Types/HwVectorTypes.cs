using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace MathSharp
{
    using Vector4F = Vector128<float>;

    // Used for overload resolution. All conversions are nops and the codegen around them is good

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector2
    {
        public readonly Vector4F Value;
        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}>";
        public override string ToString() => DebuggerString;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HwVector2(Vector4F value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4F(HwVector2 vector) => vector.Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVector2(Vector4F vector) => new HwVector2(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVectorAny(HwVector2 vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector3(HwVector2 vector) => new HwVector3(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector4(HwVector2 vector) => new HwVector4(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector2(HwVectorAny vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator +(HwVector2 left, HwVector2 right) => Vector.Add(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator -(HwVector2 left, HwVector2 right) => Vector.Subtract(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator /(HwVector2 left, HwVector2 right) => Vector.Divide(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator *(HwVector2 left, HwVector2 right) => Vector.Multiply(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator -(HwVector2 vector) => Vector.Negate(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator &(HwVector2 left, HwVector2 right) => Vector.And(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator |(HwVector2 left, HwVector2 right) => Vector.Or(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator ^(HwVector2 left, HwVector2 right) => Vector.Xor(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector2 operator ~(HwVector2 vector) => Vector.Not(vector);


        public HwVectorAny Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.Length(this);
        }

        public HwVectorAny LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.LengthSquared(this);
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector3
    {
        public readonly Vector4F Value;

        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}>";
        public override string ToString() => DebuggerString;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HwVector3(Vector4F value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4F(HwVector3 vector) => vector.Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVector3(Vector4F vector) => new HwVector3(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVectorAny(HwVector3 vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector2(HwVector3 vector) => new HwVector2(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector4(HwVector3 vector) => new HwVector4(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector3(HwVectorAny vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator +(HwVector3 left, HwVector3 right) => Vector.Add(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator -(HwVector3 left, HwVector3 right) => Vector.Subtract(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator /(HwVector3 left, HwVector3 right) => Vector.Divide(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator *(HwVector3 left, HwVector3 right) => Vector.Multiply(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator -(HwVector3 vector) => Vector.Negate(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator &(HwVector3 left, HwVector3 right) => Vector.And(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator |(HwVector3 left, HwVector3 right) => Vector.Or(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator ^(HwVector3 left, HwVector3 right) => Vector.Xor(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector3 operator ~(HwVector3 vector) => Vector.Not(vector);


        public HwVectorAny Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.Length(this);
        }

        public HwVectorAny LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.LengthSquared(this);
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector4
    {
        public readonly Vector4F Value;
        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}, {Value.GetElement(3)}>";
        public override string ToString() => DebuggerString;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HwVector4(Vector4F value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4F(HwVector4 vector) => vector.Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVector4(Vector4F vector) => new HwVector4(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HwVectorAny(HwVector4 vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector2(HwVector4 vector) => new HwVector2(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector3(HwVector4 vector) => new HwVector3(vector);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator HwVector4(HwVectorAny vector) => vector.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator +(HwVector4 left, HwVector4 right) => Vector.Add(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator -(HwVector4 left, HwVector4 right) => Vector.Subtract(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator /(HwVector4 left, HwVector4 right) => Vector.Divide(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator *(HwVector4 left, HwVector4 right) => Vector.Multiply(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator -(HwVector4 vector) => Vector.Negate(vector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator &(HwVector4 left, HwVector4 right) => Vector.And(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator |(HwVector4 left, HwVector4 right) => Vector.Or(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator ^(HwVector4 left, HwVector4 right) => Vector.Xor(left, right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HwVector4 operator ~(HwVector4 vector) => Vector.Not(vector);


        public HwVectorAny Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.Length(this);
        }

        public HwVectorAny LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector.LengthSquared(this);
        }
    }

    public readonly struct HwVectorAny
    {
        public readonly Vector128<float> Value;

        public HwVectorAny(Vector128<float> value)
        {
            Value = value;
        }

        public static implicit operator Vector4F(HwVectorAny vector) => vector.Value;
        public static implicit operator HwVectorAny(Vector4F vector) => new HwVectorAny(vector);
    }
}
