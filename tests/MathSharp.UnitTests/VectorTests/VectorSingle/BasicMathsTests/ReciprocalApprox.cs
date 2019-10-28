using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class ReciprocalApprox
    {
        public const float AllowedDifference = 1.5f * 2e-12f;

        public static IEnumerable<object[]> Data => DataSets.Single.CreateUnaryDataSet(x => 1 / x);

        [Theory]
        [MemberData(nameof(Data))]
        public static void ReciprocalApprox_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.ReciprocalApprox(value);

            Assert.True(TestHelpers.AreApproxEqual(value, expected, AllowedDifference));
        }
    }
}