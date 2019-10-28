using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class IsNotNaN
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[]
                {
                    Vector128.Create(0.0f), new[] { false, false, false, false }
                },
                new object[]
                {
                    Vector128.Create(float.NaN), new[] { true, true, true, true }
                },
                new object[]
                {
                    Vector128.Create(float.PositiveInfinity), new[] { false, false, false, false }
                },
                new object[]
                {
                    Vector128.Create(float.NegativeInfinity), new[] { false, false, false, false }
                },
                new object[]
                {
                    Vector128.Create(0.0f, float.NaN, 0.0f, float.NaN), new[] { false, true, false, true }
                },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public static void IsNotNaN_Theory(Vector128<float> values, bool[] expected)
        {

            var isNaN = Vector.IsNotNaN(values);

            Assert.True(TestHelpers.AreAllNotEqual(expected, isNaN.AsInt32()));
        }
    }
}