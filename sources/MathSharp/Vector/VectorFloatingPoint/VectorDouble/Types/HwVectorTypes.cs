using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace MathSharp
{
    // Used for overload resolution. All conversions are nops and the codegen around them is good

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVectorAnyD
    {
        public readonly Vector256<double> Value;

        internal string DebuggerString => Value.ToString();
        public override string ToString() => DebuggerString;

        public HwVectorAnyD(Vector256<double> value)
        {
            Value = value;
        }

        public static implicit operator Vector256<double>(HwVectorAnyD vector) => vector.Value;
        public static implicit operator HwVectorAnyD(Vector256<double> vector) => new HwVectorAnyD(vector);
    }

    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector2D
    {
        public readonly Vector256<double> Value;

        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}>";
        public override string ToString() => DebuggerString;

        [MethodImpl(AggressiveInlining)]
        public HwVector2D(Vector256<double> value)
        {
            Value = value;
        }

        [MethodImpl(AggressiveInlining)]
        public static implicit operator Vector256<double>(HwVector2D vector) => vector.Value;
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVector2D(Vector256<double> vector) => new HwVector2D(vector);
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVectorAnyD(HwVector2D vector) => vector.Value;
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVector2D(HwVectorAnyD vector) => vector.Value;

        [MethodImpl(AggressiveInlining)]
        public static explicit operator HwVector3D(HwVector2D vector) => new HwVector3D(vector);
        [MethodImpl(AggressiveInlining)]
        public static explicit operator HwVector4D(HwVector2D vector) => new HwVector4D(vector);

        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator ++(HwVector2D left) => Vector.Add(left, Vector.DoubleConstants.One);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator --(HwVector2D left) => Vector.Subtract(left, Vector.DoubleConstants.One);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator +(HwVector2D left, HwVector2D right) => Vector.Add(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator +(HwVector2D left, double right) => Vector.Add(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator -(HwVector2D left, HwVector2D right) => Vector.Subtract(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator -(HwVector2D left, double right) => Vector.Subtract(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator /(HwVector2D left, HwVector2D right) => Vector.Divide(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator /(HwVector2D left, double right) => Vector.Divide(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator %(HwVector2D left, HwVector2D right) => Vector.Remainder(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator %(HwVector2D left, double right) => Vector.Remainder(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator *(HwVector2D left, HwVector2D right) => Vector.Multiply(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator *(HwVector2D left, float right) => Vector.Multiply(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator -(HwVector2D vector) => Vector.Negate(vector);

        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator &(HwVector2D left, HwVector2D right) => Vector.And(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator &(HwVector2D left, double right) => Vector.And(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator |(HwVector2D left, HwVector2D right) => Vector.Or(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator |(HwVector2D left, double right) => Vector.Or(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator ^(HwVector2D left, HwVector2D right) => Vector.Xor(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator ^(HwVector2D left, double right) => Vector.Xor(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector2D operator ~(HwVector2D vector) => Vector.Not(vector);
    }
    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector3D
    {
        public readonly Vector256<double> Value;

        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}>";
        public override string ToString() => DebuggerString;

        [MethodImpl(AggressiveInlining)]
        public HwVector3D(Vector256<double> value)
        {
            Value = value;
        }

        [MethodImpl(AggressiveInlining)]
        public static implicit operator Vector256<double>(HwVector3D vector) => vector.Value;
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVector3D(Vector256<double> vector) => new HwVector3D(vector);
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVectorAnyD(HwVector3D vector) => vector.Value;
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVector3D(HwVectorAnyD vector) => vector.Value;

        [MethodImpl(AggressiveInlining)]
        public static explicit operator HwVector2D(HwVector3D vector) => new HwVector2D(vector);
        [MethodImpl(AggressiveInlining)]
        public static explicit operator HwVector4D(HwVector3D vector) => new HwVector4D(vector);

        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator ++(HwVector3D left) => Vector.Add(left, Vector.DoubleConstants.One);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator --(HwVector3D left) => Vector.Subtract(left, Vector.DoubleConstants.One);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator +(HwVector3D left, HwVector3D right) => Vector.Add(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator +(HwVector3D left, double right) => Vector.Add(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator -(HwVector3D left, HwVector3D right) => Vector.Subtract(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator -(HwVector3D left, double right) => Vector.Subtract(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator /(HwVector3D left, HwVector3D right) => Vector.Divide(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator /(HwVector3D left, double right) => Vector.Divide(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator %(HwVector3D left, HwVector3D right) => Vector.Remainder(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator %(HwVector3D left, double right) => Vector.Remainder(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator *(HwVector3D left, HwVector3D right) => Vector.Multiply(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator *(HwVector3D left, float right) => Vector.Multiply(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator -(HwVector3D vector) => Vector.Negate(vector);

        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator &(HwVector3D left, HwVector3D right) => Vector.And(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator &(HwVector3D left, double right) => Vector.And(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator |(HwVector3D left, HwVector3D right) => Vector.Or(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator |(HwVector3D left, double right) => Vector.Or(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator ^(HwVector3D left, HwVector3D right) => Vector.Xor(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator ^(HwVector3D left, double right) => Vector.Xor(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector3D operator ~(HwVector3D vector) => Vector.Not(vector);
    }
    [DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
    public readonly struct HwVector4D
    {
        public readonly Vector256<double> Value;

        internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}, {Value.GetElement(3)}>";
        public override string ToString() => DebuggerString;

        [MethodImpl(AggressiveInlining)]
        public HwVector4D(Vector256<double> value)
        {
            Value = value;
        }

        [MethodImpl(AggressiveInlining)]
        public static implicit operator Vector256<double>(HwVector4D vector) => vector.Value;
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVector4D(Vector256<double> vector) => new HwVector4D(vector);
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVectorAnyD(HwVector4D vector) => vector.Value;
        [MethodImpl(AggressiveInlining)]
        public static implicit operator HwVector4D(HwVectorAnyD vector) => vector.Value;

        [MethodImpl(AggressiveInlining)]
        public static explicit operator HwVector2D(HwVector4D vector) => new HwVector2D(vector);
        [MethodImpl(AggressiveInlining)]
        public static explicit operator HwVector3D(HwVector4D vector) => new HwVector3D(vector);

        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator ++(HwVector4D left) => Vector.Add(left, Vector.DoubleConstants.One);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator --(HwVector4D left) => Vector.Subtract(left, Vector.DoubleConstants.One);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator +(HwVector4D left, HwVector4D right) => Vector.Add(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator +(HwVector4D left, double right) => Vector.Add(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator -(HwVector4D left, HwVector4D right) => Vector.Subtract(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator -(HwVector4D left, double right) => Vector.Subtract(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator /(HwVector4D left, HwVector4D right) => Vector.Divide(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator /(HwVector4D left, double right) => Vector.Divide(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator %(HwVector4D left, HwVector4D right) => Vector.Remainder(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator %(HwVector4D left, double right) => Vector.Remainder(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator *(HwVector4D left, HwVector4D right) => Vector.Multiply(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator *(HwVector4D left, float right) => Vector.Multiply(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator -(HwVector4D vector) => Vector.Negate(vector);

        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator &(HwVector4D left, HwVector4D right) => Vector.And(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator &(HwVector4D left, double right) => Vector.And(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator |(HwVector4D left, HwVector4D right) => Vector.Or(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator |(HwVector4D left, double right) => Vector.Or(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator ^(HwVector4D left, HwVector4D right) => Vector.Xor(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator ^(HwVector4D left, double right) => Vector.Xor(left, right);
        [MethodImpl(AggressiveInlining)]
        public static HwVector4D operator ~(HwVector4D vector) => Vector.Not(vector);
    }
}
