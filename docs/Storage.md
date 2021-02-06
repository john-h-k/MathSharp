# Storage

MathSharp operations are designed to occur with locals and arguments, which are located on the stack or SIMD registers, allowing them to be fast and efficient. Because of this,
the types worked with are generally larger than you would expect or have specific alignment requirements, and don't distinguish between different dimensions. Generally, you should use other types
for fields and members of types, and then use SIMD types for passing parameters or returns. This will result in the best performance and the minimum number of conversions between storage and SIMD types.

By default, MathSharp supports using the `System.Numerics` types as storage types, and provides `Load()` extension methods in `Vector, Matrix, Quaternion`, and `Store(out T store)` methods too.
Writing your own storage types is trivial. Use the `FromVector` and `ToVector` methods to convert the types. As an example, here is a custom Vector3 storage type.

```cs
public struct MyVec3
{
    public float X, Y, Z;

    public Vector128<float> Load() => Vector.FromVector3D(in x);
}

public static class MyVec3Extensions
{
    public static void Store(this Vector128<float> vector, out MyVec3 vec3) => Vector.ToVector3D(vector, vec3);
}
```

You can use the aligned variants of these methods (e.g `FromVector3DAligned`) if you can guarantee the passed value is always 16 byte aligned for `float`s, and 32 byte aligned for `double`s.

Passing by `in` is very important, because it ensures you are actually passing a reference. We don't take `ref` because we want to allow readonly members to be passed.
The load and from methods also have pointer overloads. Where these can be called without pinning, they should be used for potential performance benefits.