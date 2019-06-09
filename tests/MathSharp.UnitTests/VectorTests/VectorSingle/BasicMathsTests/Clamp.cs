using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.Utils.Helpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Clamp
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(0f), Vector128.Create(0f), Vector128.Create(0f), new Vector4(Math.Clamp(0f, 0f, 0f)) },
                new object[] { Vector128.Create(1f), Vector128.Create(0f), Vector128.Create(1f), new Vector4(Math.Clamp(1f, 0f, 1f)) },
                new object[] { Vector128.Create(1f), Vector128.Create(10f), Vector128.Create(110f), new Vector4(Math.Clamp(1f, 10f, 110f)) },
                new object[] { Vector128.Create(float.NegativeInfinity), Vector128.Create(0f), Vector128.Create(1f), new Vector4(Math.Clamp(float.NegativeInfinity, 0f, 1f)) },
                new object[] { Vector128.Create(float.PositiveInfinity), Vector128.Create(0f), Vector128.Create(100f), new Vector4(Math.Clamp(float.PositiveInfinity, 0f, 100f)) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Clamp_Theory(Vector128<float> vector, Vector128<float> low, Vector128<float> high, Vector4 expected)
        {
            Vector128<float> result = Vector.Clamp(vector, low, high);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}