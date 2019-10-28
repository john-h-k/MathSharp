using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BasicMathsTests
{
    public class Negate
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector256.Create(1d), new Vector4d(-1d) },
                new object[] { Vector256.Create(-1d), new Vector4d(1d) },
                new object[] { Vector256.Create(-1d, double.NegativeInfinity, -1d, -0.00000000000000000001d), new Vector4d(1d, double.PositiveInfinity, 1d, 0.00000000000000000001d) },
                new object[] { Vector256.Create(double.NegativeInfinity), new Vector4d(double.PositiveInfinity) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Negate_Theory(Vector256<double> vector, Vector4d expected)
        {
            Vector256<double> result = Vector.Negate(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Fact]
        public void Negate_NegateZero_Passes()
        {
            Vector256<double> result = Vector.Negate(Vector256.Create(0d));

            Vector256<long> expected = Vector256.Create(long.MinValue);

            Assert.True(result.AsInt64().Equals(expected), $"Expected {expected}, got {result}");
        }
    }
}