using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Xunit;
using static MathSharp.Utils.Helpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.ComparisonTests
{
    public class Equality
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { Vector128.Create(0f), Vector128.Create(0f), new []{ true, true, true, true } },
                new object[] { Vector128.Create(0f), Vector128.Create(1f), new []{ false, false, false, false } },
                new object[] { Vector128.Create(0f, 1f, 0f, 1f), Vector128.Create(0f), new []{ true, false, true, false } },
                new object[] { Vector128.Create(float.NaN), Vector128.Create(0f), new []{ false, false, false, false } },
                new object[] 
                {
                    Vector128.Create(0f, float.MaxValue, float.MinValue, float.PositiveInfinity),
                    Vector128.Create(float.Epsilon, float.MaxValue - 1000000000000000000000000000000000000f, float.MinValue + 1000000000000000000000000000000000000f, float.NegativeInfinity),
                    new []{ false, false, false, false }
                }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public static void Equality_Theory(Vector128<float> left, Vector128<float> right, bool[] expected)
        {
            Vector128<int> result = Vector.Equality(left, right).AsInt32();

            Assert.True(AreAllEqual(expected, result));
        }
    }
}