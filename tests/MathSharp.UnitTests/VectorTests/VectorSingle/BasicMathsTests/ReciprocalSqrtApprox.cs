using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class ReciprocalSqrtApprox
    {
        public const float AllowedDifference = 1.5f * 2e-12f;

        public static IEnumerable<object[]> Data => DataSets.Single.CreateUnaryDataSet(x => 1 / MathF.Sqrt(x));

        [Theory]
        [MemberData(nameof(Data))]
        public static void ReciprocalSqrtApprox_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.ReciprocalSqrtApprox(value);

            Assert.True(TestHelpers.AreApproxEqual(value, expected, AllowedDifference));
        }
    }
}