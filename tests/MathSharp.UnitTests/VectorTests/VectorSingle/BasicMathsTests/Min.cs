using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Min
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(0f), Vector128.Create(1f), new Vector4(MathF.Min(0f, 1f)) },
                new object[] { Vector128.Create(1f), Vector128.Create(-1f), new Vector4(MathF.Min(1f, -1f)) },
                new object[] { Vector128.Create(0.99999999999999999999999999999999999999999f), Vector128.Create(1f), new Vector4(MathF.Min(0.99999999999999999999999999999999999999999f, 1f)) },
                new object[] { Vector128.Create(float.NegativeInfinity), Vector128.Create(float.PositiveInfinity), new Vector4(MathF.Min(float.NegativeInfinity, float.PositiveInfinity)) },
                new object[] { Vector128.Create(float.NaN), Vector128.Create(0f), new Vector4(0f) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Min_Theory(Vector128<float> left, Vector128<float> right, Vector4 expected)
        {
            Vector128<float> result = Vector.Min(left, right);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}