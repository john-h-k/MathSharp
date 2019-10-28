using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BasicMathsTests
{
    public class Clamp
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector256.Create(0d), Vector256.Create(0d), Vector256.Create(0d), new Vector4d(Math.Clamp(0d, 0d, 0d)) },
                new object[] { Vector256.Create(1d), Vector256.Create(0d), Vector256.Create(1d), new Vector4d(Math.Clamp(1d, 0d, 1d)) },
                new object[] { Vector256.Create(1d), Vector256.Create(10d), Vector256.Create(110d), new Vector4d(Math.Clamp(1d, 10d, 110d)) },
                new object[] { Vector256.Create(double.NegativeInfinity), Vector256.Create(0d), Vector256.Create(1d), new Vector4d(Math.Clamp(double.NegativeInfinity, 0d, 1d)) },
                new object[] { Vector256.Create(double.PositiveInfinity), Vector256.Create(0d), Vector256.Create(100d), new Vector4d(Math.Clamp(double.PositiveInfinity, 0d, 100d)) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Clamp_Theory(Vector256<double> vector, Vector256<double> low, Vector256<double> high, Vector4d expected)
        {
            Vector256<double> result = Vector.Clamp(vector, low, high);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}