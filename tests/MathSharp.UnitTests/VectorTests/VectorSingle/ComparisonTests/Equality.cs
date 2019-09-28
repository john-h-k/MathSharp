using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.ComparisonTests
{
    public class Equality
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(0f), Vector128.Create(0f), new []{ true, true, true, true } },
                new object[] { Vector128.Create(0f), Vector128.Create(1f), new []{ false, false, false, false } },
                new object[] { Vector128.Create(0f, 1f, 0f, 1f), Vector128.Create(0f), new []{ true, false, true, false } },
                new object[] { Vector128.Create(float.NaN), Vector128.Create(0f), new []{ false, false, false, false } },
                new object[]
                {
                    Vector128.Create(0f, float.MaxValue, float.MinValue, float.PositiveInfinity),
                    Vector128.Create(float.Epsilon, float.MaxValue - 1E+32f, float.MinValue + 1E+32f, float.NegativeInfinity),
                    new []{ false, false, false, false }
                }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public static void Equality_Theory(Vector128<float> left, Vector128<float> right, bool[] expected)
        {
            Vector128<int> result = Vector.Equality(left, right).Value.AsInt32();

            Assert.True(AreAllEqual(expected, result));
        }
    }
}