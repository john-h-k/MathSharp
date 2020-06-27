# What is MathSharp

MathSharp is a library designed around blazing-fast, platform-agnostic linear algebra and 3D maths.

## What are the advantages of MathSharp over its competitors?

MathSharp is faster! Check our [benchmarks](https://github.com/john-h-k/MathSharp/blob/master/README.md). We offer a more complete feature set than `System.Numerics`, with faster inclusion of new features (such as colour and collision support, which are currently in the works). We also offer fully-fledged double-precision vector, quaternion, and matrix types in .NET Core 3.0, which `System.Numerics` doesn't.

## That sounds great, but what does using MathSharp look like?

Let's take a relatively simple example. You have some code, using the `System.Numeric`s types, that just finds the cross product of 2 pairs of vectors, then finds the dot of that.

```cs
public void DotAndCross(Vector3 left0, Vector3 left1, Vector3 right0, Vector3 right1, out Vector3 result)
{
    var leftDot = new Vector3(Vector3.Dot(left0, left1));
    var rightDot = new Vector3(Vector3.Dot(right0, right1));
    result = Vector3.Cross(leftDot, rightDot);
}
```

So, the first step is take the incoming `Vector3` params by reference. This isn't always necessary, but it allows you to take advantage of better loading on some platforms.

Then, we convert the `System.Numerics` types to the native SIMD types by using MathSharp's extension methods. `Load()` takes a `Vector3`, and returns a `Vector128<float>`.
We then use the static methods in `MathSharp.Vector` to perform our operations. Here, we want to perform a 3D dot and cross.

We then finally use the `Vector.Store` method to store out SIMD type into our storage type.

```cs
using MathSharp; // Include at the top

public void DotAndCross(in Vector3 left0, in Vector3 left1, in Vector3 right0, in Vector3 right1, out Vector3 result)
{
    var leftDot = Vector.Dot3D(left0.Load(), left1.Load());
    var rightDot = Vector.Dot3D(right0.Load(), right1.Load());

    Vector.Store(Vector.Cross3D(leftDot, rightDot), out result);
}
```

Tada! You've just used MathSharp without even making a breaking change. This is still a great way to use it - but not the best.

The idiomatic, fastest way to use MathSharp, is to pass around the SIMD types as arguments and returns, and use the so-called storage types - such as `System.Numerics` and `OpenTk.Maths` types, as well as your custom storage types, for fields and members of other types. For more information about this as well as creating your own storage types, check out [storage](storage.md). For example, rewriting the above method to be idiomatic MathSharp would be

```cs
using MathSharp; // Include at the top

public Vector128<float> DotAndCross(Vector128<float>  left0, Vector128<float>  left1, Vector128<float> right0, Vector128<float>  right1)
{
    var leftDot = Vector.Dot3D(left0, left1);
    var rightDot = Vector.Dot3D(right0, right1);

    return Vector.Cross3D(leftDot, rightDot);
}
```

The key MathSharp types are:

* `Vector` - a static class containing methods to work with vectors
* `Matrix` - a static class containing methods to work with matrixes
* `Quaternion` - a static class containing methods to work with quaternions
* `MatrixSingle` - a struct used to represent a single-precision 4x4 matrix SIMD type

Comparisons in MathSharp are extremely powerful - allowing you to perform a far wide variety
of comparison and selection operations, and to use those comparisons in ways that other libraries simply don't. If you've ever worked with SIMD before, these comparisons will be intuitive to you. If not, they're still easy to learn, and unlock a wide range of operations. Check out our [comparisons](comparisons-and-selections.md) doc for more.

