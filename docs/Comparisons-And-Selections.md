# Comparisons

MathSharp offers all the comparisons you'd expect - but they don't necessarily work in the way you'd expect from working with other libraries.
Most comparison results return the same type you give them, rather than `bool` or multiple `bool`s. You might find this confusing, but it'll make sense soon.

Let's start with the simple comparisons. You want to check if all or any elements of a vector are true or false for a comparison. This is really easily achieved with extension methods.
For example - test if 2 vectors are equal:

```cs
public bool AreEqual(Vector128<float> left, Vector128<right> right)
{
    var comp = Vector.CompareEqual(left, right);
    return comp.AllTrue(); // returns true if all elements in the comparison are true, else false
}
```

Tada! It's that simple. The 4 simplest comparison extensions are:

```cs
AllTrue() - true if all elements of the comparison are true
AllFalse() - true if all elements of the comparison are false
AllFalse() - true if any elements of the comparison are true
AnyFalse() - true if any elements of the comparison are false
```

First, a quick brush up on bitwise operations. A bitwise `and`/`&`, results in a bit being set if both inputs are set. Else it is zero.
A bitwise `or`/`|`, results in a bit being set if either or both inputs are set. If both are zero, it is zero. E.g

```cs
// 0b indicates a binary literal. & is and, | is or
0b_1 & 0b_1 -> 1
0b_0 & 0b_1 -> 0
0b_1 & 0b_0 -> 0
0b_0 & 0b_0 -> 0

0b_1 | 0b_1 -> 1
0b_0 | 0b_1 -> 1
0b_1 | 0b_0 -> 1
0b_0 | 0b_0 -> 0
```

Also remember that all values on your computer are binary. Floating point types are complex, but they're still a chunk of binary data and nothing more.
So, let's give an example operation we want to do. Say we have a vector, and we want to multiply all numbers on it by another vector, if they are equal to the numbers in the other vector, else, we want to square root them.
E,g `<99, -5, 6, -2.3>, <99, 7, 0.1, -2.3>` would become `<99  * 99, Sqrt(-5), Sqrt(6), -2.3 * -2.3>`. Let's write this using `System.Numerics`

```cs
public Vector4 Foo(Vector4 left, Vector4 right)
{
    var mul = left * right;
    var sqrt = Vector4.SquareRoot(mul);

    // No way to vectorise this :()
    var x = left.X == right.X ? left.X * right.X : MathF.Sqrt(left.X);
    var y = left.Y == right.Y ? left.Y * right.Y : MathF.Sqrt(left.Y);
    var z = left.Z == right.Z ? left.Z * right.Z : MathF.Sqrt(left.Z);
    var w = left.W == right.W ? left.W * right.W : MathF.Sqrt(left.W);
    return new Vector4(x, y, z, w);
}
```

On my system, this takes about 3 nanoseconds. Not bad eh!

Now let's look at MathSharp implementation:

```cs
public Vector128<float> Foo(Vector128<float> left, Vector128<float> right)
{
    var comp = Vector.CompareEqual(left, right);
    var mul = Vector.Multiply(left, right);
    var sqrt = Vector.Sqrt(mul);

    return Vector.Select(sqrt, mul, comp);
}
```

Again on my system, this takes 0.3 nanoseconds. That's a 10x speedup!
So, what does it do?

First, it does the comparison. `Vector.CompareEqual` returns a `Vector128<float>`, as mentioned above. However, what's important is what is in that return.
Each element is either all-1s or all-0s as binary. This means, that you can use them as masks. For example, to select all elements of a vector that are equal to another, and zero the rest, you just do

```cs
Vector.And(left, Vector.CompareEqual(left, right));
```

E.g, with `<1, 2, 3, 4>` and `<1, 3, 3, 5>`, the comparison returns `<0xFFFFFFFF, 0, 0xFFFFFFFF, 0>`. When we perform the `and`, as we learnt above, the elements with 0 are discarded, and the ones with `0xFFFFFFFF` (which is all bits set) are kept.

Given the commonness of this operation, MathSharp provides 3 selection helper methods:

```cs
SelectWhereTrue(selector, vector)
SelectWhereFalse(selector, vector)
Select(selector, left, right)
```

`SelectWhereTrue` and `SelectWhereFalse` are reasonably self explanatory. They select elements from `vector` which correspond to `true` elements of the `selector` (usually created with a `Vector.Compare...` method).
`Select` works like a ternary expression, selecting elements from `left` where `selector` is true, and from `right` where `selector` is `false`. In the above example, we select the multiplied value when the values are equal, else we select the square root value.