using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;
using static MathSharp.Utils.Helpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Abs
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(0f), new Vector4(0f) }, 
                new object[] { Vector128.Create(1f), new Vector4(1f) }, 
                new object[] { Vector128.Create(-1f), new Vector4(1f) },
                new object[] { Vector128.Create(-1f, 0f, -1f, 0f), new Vector4(1f, 0f, 1f, 0f) },
                new object[] { Vector128.Create(float.NegativeInfinity), new Vector4(float.PositiveInfinity) },
                new object[] { Vector128.Create(float.NaN, -111f, -0.0000001f, 0.0000001f), new Vector4(float.NaN, 111f, 0.0000001f, 0.0000001f) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Abs_Theory(Vector128<float> vector, Vector4 expected)
        {
            Vector128<float> result = Vector.Abs(vector);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}