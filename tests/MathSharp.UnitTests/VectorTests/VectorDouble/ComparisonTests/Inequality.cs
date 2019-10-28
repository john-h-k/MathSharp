using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.ComparisonTests
{
    public class Inequality
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector256.Create(0d), Vector256.Create(0d), new []{ true, true, true, true } },
                new object[] { Vector256.Create(0d), Vector256.Create(1d), new []{ false, false, false, false } },
                new object[] { Vector256.Create(0d, 1d, 0d, 1d), Vector256.Create(0d), new []{ true, false, true, false } },
                new object[] { Vector256.Create(double.NaN), Vector256.Create(0d), new []{ false, false, false, false } },
                new object[]
                {
                    Vector256.Create(0d, double.MaxValue, double.MinValue, double.PositiveInfinity),
                    Vector256.Create(double.Epsilon, double.MaxValue - 1E+301d, double.MinValue + 1E+301d, double.NegativeInfinity),
                    new []{ false, false, false, false }
                }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public static void Inequality_Theory(Vector256<double> left, Vector256<double> right, bool[] expected)
        {
            Vector256<long> result = Vector.CompareNotEqual(left, right).AsInt64();

            Assert.True(AreAllNotEqual(expected, result));
        }
    }
}