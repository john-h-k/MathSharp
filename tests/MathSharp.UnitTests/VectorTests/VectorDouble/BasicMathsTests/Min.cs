using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BasicMathsTests
{
    public class Min
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector256.Create(0d), Vector256.Create(1d), new Vector4d(Math.Min(0d, 1d)) },
                new object[] { Vector256.Create(1d), Vector256.Create(-1d), new Vector4d(Math.Min(1d, -1d)) },
                new object[] { Vector256.Create(0.99999999999999999999999999999999999999999d), Vector256.Create(1d), new Vector4d(Math.Min(0.99999999999999999999999999999999999999999d, 1d)) },
                new object[] { Vector256.Create(double.NegativeInfinity), Vector256.Create(double.PositiveInfinity), new Vector4d(Math.Min(double.NegativeInfinity, double.PositiveInfinity)) },
                new object[] { Vector256.Create(double.NaN), Vector256.Create(0d), new Vector4d(0d) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Min_Theory(Vector256<double> left, Vector256<double> right, Vector4d expected)
        {
            Vector256<double> result = Vector.Min(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}