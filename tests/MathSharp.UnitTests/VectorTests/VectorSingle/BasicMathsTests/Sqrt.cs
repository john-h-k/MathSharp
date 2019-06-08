using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.Utils.Helpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Sqrt
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(0f), new Vector4(MathF.Sqrt(0f)) },
                new object[] { Vector128.Create(1f), new Vector4(MathF.Sqrt(1f)) },
                new object[] { Vector128.Create(-1f), new Vector4(MathF.Sqrt(-1)) },
                new object[] { Vector128.Create(1f, 4f, 9f, 16f), new Vector4(MathF.Sqrt(1f), MathF.Sqrt(4f), MathF.Sqrt(9f), MathF.Sqrt(16f)) },
                new object[] { Vector128.Create(0.5f, 111f, -0.0000001f, 0.0000001f), new Vector4(MathF.Sqrt(0.5f), MathF.Sqrt(111f), MathF.Sqrt(-0.0000001f), MathF.Sqrt(0.0000001f)) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Sqrt_Theory(Vector128<float> vector, Vector4 expected)
        {
            Vector128<float> result = Vector.Sqrt(vector);

            Assert.True(AreEqual(expected, result));
        }
    }
}