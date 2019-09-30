any_type_template = """
[DebuggerDisplay("{{" + nameof(DebuggerString) + "}}")]
public readonly struct {TYPE}
{{
    public readonly {TYPE_UNDERLYING} Value;

    internal string DebuggerString => Value.ToString();
    public override string ToString() => DebuggerString;

    public {TYPE}({TYPE_UNDERLYING} value)
    {{
        Value = value;
    }}

    public static implicit operator {TYPE_UNDERLYING}({TYPE} vector) => vector.Value;
    public static implicit operator {TYPE}({TYPE_UNDERLYING} vector) => new {TYPE}(vector);
}}
"""

scalar_variant_template = """    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator {OPERATOR}({TYPE} left, {TYPE_SCALAR} right) => Vector.Add(left, {TYPE_UNDERLYING_CREATOR}.Create(right));
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator {OPERATOR}({TYPE_SCALAR} left, {TYPE} right) => Vector.Add({TYPE_UNDERLYING_CREATOR}.Create(left), right);
"""

template = """
[DebuggerDisplay("{{" + nameof(DebuggerString) + "}}")]
public readonly struct {TYPE} : IEquatable<{TYPE}>
{{
    public readonly {TYPE_UNDERLYING} Value;
    
    internal string DebuggerString => $"{DEBUG_STRING}";
    public override string ToString() => DebuggerString;

    [MethodImpl(AggressiveInlining)]
    public {TYPE}({TYPE_UNDERLYING} value)
    {{
        Value = value;
    }}

    public bool AllTrue() => Vector.AllTrue(this);
    public bool AllFalse() => Vector.AllFalse(this);
    public bool AnyTrue() => Vector.AnyTrue(this);
    public bool AnyFalse() => Vector.AnyFalse(this);
    public bool ElementTrue(int index) => Vector.ElementTrue(this, index);
    public bool ElementFalse(int index) => Vector.ElementFalse(this, index);

    public override bool Equals(object? obj)
        => obj is {TYPE} other && Equals(other);

    public override int GetHashCode()
        => Value.GetHashCode();

    public bool Equals({TYPE} obj)
        => (this == obj).AllTrue();

    [MethodImpl(AggressiveInlining)]
    public static implicit operator {TYPE_UNDERLYING}({TYPE} vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator {TYPE}({TYPE_UNDERLYING} vector) => new {TYPE}(vector);
    [MethodImpl(AggressiveInlining)]
    public static implicit operator {TYPE_ANY}({TYPE} vector) => vector.Value;
    [MethodImpl(AggressiveInlining)]
    public static implicit operator {TYPE}({TYPE_ANY} vector) => vector.Value;

    [MethodImpl(AggressiveInlining)]
    public static explicit operator {TYPE_OTHER_DIMENSION_1}({TYPE} vector) => new {TYPE_OTHER_DIMENSION_1}(vector);
    [MethodImpl(AggressiveInlining)]
    public static explicit operator {TYPE_OTHER_DIMENSION_2}({TYPE} vector) => new {TYPE_OTHER_DIMENSION_2}(vector);

    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator ++({TYPE} left) => Vector.Add(left, Vector.{CONSTANTS}.One);
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator --({TYPE} left) => Vector.Subtract(left, Vector.{CONSTANTS}.One);
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator +({TYPE} left, {TYPE} right) => Vector.Add(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator -({TYPE} left, {TYPE} right) => Vector.Subtract(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator /({TYPE} left, {TYPE} right) => Vector.Divide(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator %({TYPE} left, {TYPE} right) => Vector.Remainder(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator *({TYPE} left, {TYPE} right) => Vector.Multiply(left, right);  // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator -({TYPE} vector) => Vector.Negate(vector);

    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator &({TYPE} left, {TYPE} right) => Vector.And<{TYPE_SCALAR}>(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator |({TYPE} left, {TYPE} right) => Vector.Or<{TYPE_SCALAR}>(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator ^({TYPE} left, {TYPE} right) => Vector.Xor<{TYPE_SCALAR}>(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator ~({TYPE} vector) => Vector.Not<{TYPE_SCALAR}>(vector);

    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator ==({TYPE} left, {TYPE} right) => Vector.CompareEqual(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator !=({TYPE} left, {TYPE} right) => Vector.CompareNotEqual(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator <({TYPE} left, {TYPE} right) => Vector.CompareLessThan(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator >({TYPE} left, {TYPE} right) => Vector.CompareGreaterThan(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator <=({TYPE} left, {TYPE} right) => Vector.CompareLessThanOrEqual(left, right); // SCALAR_VARIANT
    
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator >=({TYPE} left, {TYPE} right) => Vector.CompareGreaterThanOrEqual(left, right); // SCALAR_VARIANT
    
}}"""


def format_type(_type):
    _type_scalar = "float" if _type[-1] == "S" else "double"
    _type_underlying = "Vector128<float>" if _type[-1] == "S" else "Vector256<double>"
    _type_underlying_creator = "Vector128" if _type[-1] == "S" else "Vector256"
    _type_any = "HwVectorAny" + _type[-1]
    dims = ["2", "3", "4"]
    _type_dim = _type[-2]
    _other_dims = [dim for dim in dims if dim != _type_dim]
    _type_other_dimension_1 = _type[:-2] + _other_dims[0] + _type[-1]
    _type_other_dimension_2 = _type[:-2] + _other_dims[1] + _type[-1]
    _constants = "SingleConstants" if _type[-1] == "S" else "DoubleConstants"

    _debug_string = "<" + (", ".join(["{{Value.GetElement({0})}}".format(i) for i in range(int(_type_dim))])) + ">"

    new = ""
    for line in template.split("\n"):
        sv_str = " // SCALAR_VARIANT"
        if line.endswith(sv_str):
            line = line.replace(sv_str, "")

            operator_ind = line.index("operator ") + len("operator ")
            end_ind = line.index("(")
            operator = line[operator_ind:end_ind]
            
            new += line
            new += "\n"
            new += scalar_variant_template.format(TYPE=_type, 
                                                  TYPE_SCALAR=_type_scalar, 
                                                  TYPE_UNDERLYING_CREATOR=_type_underlying_creator,
                                                  OPERATOR=operator)
        else:
            new += line + "\n"
    
    return new.format(TYPE=_type,
                           TYPE_SCALAR=_type_scalar,
                           TYPE_ANY=_type_any,
                           TYPE_UNDERLYING=_type_underlying,
                           TYPE_OTHER_DIMENSION_1=_type_other_dimension_1,
                           TYPE_OTHER_DIMENSION_2=_type_other_dimension_2,
                           CONSTANTS=_constants,
                           DEBUG_STRING=_debug_string)

def format_any_type(_type):
    return any_type_template.format(TYPE=_type, TYPE_UNDERLYING="Vector128<float>" if _type[-1] == "S" else "Vector256<double>")

dims = ["2", "3", "4"]
types = ["S", "D"]


for t in types:
    with open(f"raw_hwvector_types_{t}.cs", "w") as f:
        f.write(format_any_type("HwVectorAny" + t))
    
        for dim in dims:
            f.write(format_type("HwVector" + dim + t))
    
