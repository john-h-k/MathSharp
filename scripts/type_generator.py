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

template = """
[DebuggerDisplay("{{" + nameof(DebuggerString) + "}}")]
public readonly struct {TYPE}
{{
    public readonly {TYPE_UNDERLYING} Value;
    
    internal string DebuggerString => $"{DEBUG_STRING}";
    public override string ToString() => DebuggerString;

    [MethodImpl(AggressiveInlining)]
    public {TYPE}({TYPE_UNDERLYING} value)
    {{
        Value = value;
    }}

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
    public static {TYPE} operator +({TYPE} left, {TYPE} right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator +({TYPE} left, {TYPE_SCALAR} right) => Vector.Add(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator -({TYPE} left, {TYPE} right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator -({TYPE} left, {TYPE_SCALAR} right) => Vector.Subtract(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator /({TYPE} left, {TYPE} right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator /({TYPE} left, {TYPE_SCALAR} right) => Vector.Divide(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator %({TYPE} left, {TYPE} right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator %({TYPE} left, {TYPE_SCALAR} right) => Vector.Remainder(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator *({TYPE} left, {TYPE} right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator *({TYPE} left, float right) => Vector.Multiply(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator -({TYPE} vector) => Vector.Negate(vector);

    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator &({TYPE} left, {TYPE} right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator &({TYPE} left, {TYPE_SCALAR} right) => Vector.And(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator |({TYPE} left, {TYPE} right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator |({TYPE} left, {TYPE_SCALAR} right) => Vector.Or(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator ^({TYPE} left, {TYPE} right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator ^({TYPE} left, {TYPE_SCALAR} right) => Vector.Xor(left, right);
    [MethodImpl(AggressiveInlining)]
    public static {TYPE} operator ~({TYPE} vector) => Vector.Not(vector);
}}"""


def format_type(_type):
    _type_scalar = "float" if _type[-1] == "S" else "double"
    _type_underlying = "Vector128<float>" if _type[-1] == "S" else "Vector256<double>"
    _type_any = "HwVectorAny" + _type[-1]
    dims = ["2", "3", "4"]
    _type_dim = _type[-2]
    _other_dims = [dim for dim in dims if dim != _type_dim]
    _type_other_dimension_1 = _type[:-2] + _other_dims[0] + _type[-1]
    _type_other_dimension_2 = _type[:-2] + _other_dims[1] + _type[-1]
    _constants = "SingleConstants" if _type[-1] == "S" else "DoubleConstants"

    _debug_string = "<" + (", ".join(["{{Value.GetElement({0})}}".format(i) for i in range(int(_type_dim))])) + ">"
    
    return template.format(TYPE=_type,
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
    
