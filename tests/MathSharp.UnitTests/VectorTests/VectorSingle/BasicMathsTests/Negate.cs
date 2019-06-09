using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.Utils.Helpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Negate
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(1f), new Vector4(-1f) },
                new object[] { Vector128.Create(-1f), new Vector4(1f) },
                new object[] { Vector128.Create(-1f, float.NegativeInfinity, -1f, -0.00000000000000000001f), new Vector4(1f, float.PositiveInfinity, 1f, 0.00000000000000000001f) },
                new object[] { Vector128.Create(float.NegativeInfinity), new Vector4(float.PositiveInfinity) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Negate_Theory(Vector128<float> vector, Vector4 expected)
        {
            Vector128<float> result = Vector.Negate4D(vector);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Fact]
        public void Negate_NegateZero_Passes()
        {
            Vector128<float> result = Vector.Negate4D(Vector128.Create(0f));

            Assert.True(result.AsInt32().Equals(Vector128.Create(int.MinValue)));
        }
    }
}