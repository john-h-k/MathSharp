using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.Utils.Helpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Add
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(0f), Vector128.Create(0f), new Vector4(0f + 0f) },
                new object[] { Vector128.Create(1f), Vector128.Create(1f), new Vector4(1f + 1f) },
                new object[] { Vector128.Create(-1f), Vector128.Create(1f), new Vector4(-1f + 1f) },
                new object[] { Vector128.Create(-1f, 0f, -1f, 0f), Vector128.Create(1f, 10f, 1f, 10f), new Vector4(-1f + 1f, 0f + 10f, -1f + 1f, 0f + 10f) },
                new object[] { Vector128.Create(float.NegativeInfinity), Vector128.Create(float.PositiveInfinity), new Vector4(float.NegativeInfinity + float.PositiveInfinity) },
                new object[] { Vector128.Create(float.MinValue), Vector128.Create(float.MaxValue), new Vector4(float.MinValue + float.MaxValue) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Add_Theory(Vector128<float> left, Vector128<float> right, Vector4 expected)
        {
            Vector128<float> result = Vector.Add(left, right);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}