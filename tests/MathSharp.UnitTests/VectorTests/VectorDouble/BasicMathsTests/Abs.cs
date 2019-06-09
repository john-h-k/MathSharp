using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BasicMathsTests
{
    public class Abs
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector256.Create(0d), new Vector4d(0d) }, 
                new object[] { Vector256.Create(1d), new Vector4d(1d) }, 
                new object[] { Vector256.Create(-1d), new Vector4d(1d) },
                new object[] { Vector256.Create(-1d, 0d, -1d, 0d), new Vector4d(1d, 0d, 1d, 0d) },
                new object[] { Vector256.Create(double.NegativeInfinity), new Vector4d(double.PositiveInfinity) },
                new object[] { Vector256.Create(double.NaN, -111d, -0.0000001d, 0.0000001d), new Vector4d(double.NaN, 111d, 0.0000001d, 0.0000001d) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Abs_Theory(Vector256<double> vector, Vector4d expected)
        {
            Vector256<double> result = Vector.Abs(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}