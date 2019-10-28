using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BasicMathsTests
{
    public class Divide
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector256.Create(0d), Vector256.Create(0d), new Vector4d(0d / 0d) },
                new object[] { Vector256.Create(1d), Vector256.Create(1d), new Vector4d(1d / 1d) },
                new object[] { Vector256.Create(-1d), Vector256.Create(1d), new Vector4d(-1d / 1d) },
                new object[] { Vector256.Create(-1d, 0d, -1d, 0d), Vector256.Create(1d, 10d, 1d, 10d), new Vector4d(-1d / 1d, 0d / 10d, -1d / 1d, 0d / 10d) },
                new object[] { Vector256.Create(double.NegativeInfinity), Vector256.Create(double.PositiveInfinity), new Vector4d(double.NegativeInfinity / double.PositiveInfinity) },
                new object[] { Vector256.Create(double.MinValue), Vector256.Create(double.MaxValue), new Vector4d(double.MinValue / double.MaxValue) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Divide_Theory(Vector256<double> left, Vector256<double> right, Vector4d expected)
        {
            Vector256<double> result = Vector.Divide(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}