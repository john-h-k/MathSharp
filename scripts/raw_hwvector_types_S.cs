
[DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
public readonly struct HwVectorAnyS
{
    public readonly Vector128<float> Value;

    internal string DebuggerString => Value.ToString();
    public override string ToString() => DebuggerString;

    public HwVectorAnyS(Vector128<float> value)
    {
        Value = value;
    }

    public static implicit operator Vector128<float>(HwVectorAnyS vector) => vector.Value;
    public static implicit operator HwVectorAnyS(Vector128<float> vector) => new HwVectorAnyS(vector);
}

[DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
public readonly struct HwVector2S
{
    public readonly Vector128<float> Value;
    
    internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}>";
    public override string ToString() => DebuggerString;

    [MethodImpl(AggressiveInlining)]
    public HwVector2S(Vector128<float> value)
    {
        Value = value;
    }

    [MethodImpl(AggressiveInlining)]
    public static implicit operator Vector128<float>(HwVector2S vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVector2S(Vector128<float> vector) => new HwVector2S(vector);
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVectorAnyS(HwVector2S vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVector2S(HwVectorAnyS vector) => vector.Value;

    [MethodImpl(AggressiveInlining)]
    public static explicit operator HwVector3S(HwVector2S vector) => new HwVector3S(vector);
    [MethodImpl(AggressiveInlining)]
    public static explicit operator HwVector4S(HwVector2S vector) => new HwVector4S(vector);

    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator ++(HwVector2S left) => Vector.Add(left, Vector.SingleConstants.One);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator --(HwVector2S left) => Vector.Subtract(left, Vector.SingleConstants.One);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator +(HwVector2S left, HwVector2S right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator +(HwVector2S left, float right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator -(HwVector2S left, HwVector2S right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator -(HwVector2S left, float right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator /(HwVector2S left, HwVector2S right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator /(HwVector2S left, float right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator %(HwVector2S left, HwVector2S right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator %(HwVector2S left, float right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator *(HwVector2S left, HwVector2S right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator *(HwVector2S left, float right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator -(HwVector2S vector) => Vector.Negate(vector);

    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator &(HwVector2S left, HwVector2S right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator &(HwVector2S left, float right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator |(HwVector2S left, HwVector2S right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator |(HwVector2S left, float right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator ^(HwVector2S left, HwVector2S right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator ^(HwVector2S left, float right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector2S operator ~(HwVector2S vector) => Vector.Not(vector);
}
[DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
public readonly struct HwVector3S
{
    public readonly Vector128<float> Value;
    
    internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}>";
    public override string ToString() => DebuggerString;

    [MethodImpl(AggressiveInlining)]
    public HwVector3S(Vector128<float> value)
    {
        Value = value;
    }

    [MethodImpl(AggressiveInlining)]
    public static implicit operator Vector128<float>(HwVector3S vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVector3S(Vector128<float> vector) => new HwVector3S(vector);
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVectorAnyS(HwVector3S vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVector3S(HwVectorAnyS vector) => vector.Value;

    [MethodImpl(AggressiveInlining)]
    public static explicit operator HwVector2S(HwVector3S vector) => new HwVector2S(vector);
    [MethodImpl(AggressiveInlining)]
    public static explicit operator HwVector4S(HwVector3S vector) => new HwVector4S(vector);

    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator ++(HwVector3S left) => Vector.Add(left, Vector.SingleConstants.One);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator --(HwVector3S left) => Vector.Subtract(left, Vector.SingleConstants.One);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator +(HwVector3S left, HwVector3S right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator +(HwVector3S left, float right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator -(HwVector3S left, HwVector3S right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator -(HwVector3S left, float right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator /(HwVector3S left, HwVector3S right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator /(HwVector3S left, float right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator %(HwVector3S left, HwVector3S right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator %(HwVector3S left, float right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator *(HwVector3S left, HwVector3S right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator *(HwVector3S left, float right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator -(HwVector3S vector) => Vector.Negate(vector);

    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator &(HwVector3S left, HwVector3S right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator &(HwVector3S left, float right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator |(HwVector3S left, HwVector3S right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator |(HwVector3S left, float right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator ^(HwVector3S left, HwVector3S right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator ^(HwVector3S left, float right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector3S operator ~(HwVector3S vector) => Vector.Not(vector);
}
[DebuggerDisplay("{" + nameof(DebuggerString) + "}")]
public readonly struct HwVector4S
{
    public readonly Vector128<float> Value;
    
    internal string DebuggerString => $"<{Value.GetElement(0)}, {Value.GetElement(1)}, {Value.GetElement(2)}, {Value.GetElement(3)}>";
    public override string ToString() => DebuggerString;

    [MethodImpl(AggressiveInlining)]
    public HwVector4S(Vector128<float> value)
    {
        Value = value;
    }

    [MethodImpl(AggressiveInlining)]
    public static implicit operator Vector128<float>(HwVector4S vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVector4S(Vector128<float> vector) => new HwVector4S(vector);
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVectorAnyS(HwVector4S vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator HwVector4S(HwVectorAnyS vector) => vector.Value;

    [MethodImpl(AggressiveInlining)]
    public static explicit operator HwVector2S(HwVector4S vector) => new HwVector2S(vector);
    [MethodImpl(AggressiveInlining)]
    public static explicit operator HwVector3S(HwVector4S vector) => new HwVector3S(vector);

    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator ++(HwVector4S left) => Vector.Add(left, Vector.SingleConstants.One);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator --(HwVector4S left) => Vector.Subtract(left, Vector.SingleConstants.One);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator +(HwVector4S left, HwVector4S right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator +(HwVector4S left, float right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator -(HwVector4S left, HwVector4S right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator -(HwVector4S left, float right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator /(HwVector4S left, HwVector4S right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator /(HwVector4S left, float right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator %(HwVector4S left, HwVector4S right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator %(HwVector4S left, float right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator *(HwVector4S left, HwVector4S right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator *(HwVector4S left, float right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator -(HwVector4S vector) => Vector.Negate(vector);

    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator &(HwVector4S left, HwVector4S right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator &(HwVector4S left, float right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator |(HwVector4S left, HwVector4S right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator |(HwVector4S left, float right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator ^(HwVector4S left, HwVector4S right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator ^(HwVector4S left, float right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static HwVector4S operator ~(HwVector4S vector) => Vector.Not(vector);
}