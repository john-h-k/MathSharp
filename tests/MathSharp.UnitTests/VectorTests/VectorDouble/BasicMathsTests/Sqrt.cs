using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BasicMathsTests
{
    public class Sqrt
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector256.Create(0d), new Vector4d(Math.Sqrt(0d)) },
                new object[] { Vector256.Create(1d), new Vector4d(Math.Sqrt(1d)) },
                new object[] { Vector256.Create(-1d), new Vector4d(Math.Sqrt(-1)) },
                new object[] { Vector256.Create(1d, 4d, 9d, 16d), new Vector4d(Math.Sqrt(1d), Math.Sqrt(4d), Math.Sqrt(9d), Math.Sqrt(16d)) },
                new object[] { Vector256.Create(0.5d, 111d, -0.0000001d, 0.0000001d), new Vector4d(Math.Sqrt(0.5d), Math.Sqrt(111d), Math.Sqrt(-0.0000001d), Math.Sqrt(0.0000001d)) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Sqrt_Theory(Vector256<double> vector, Vector4d expected)
        {
            Vector256<double> result = Vector.Sqrt(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}