using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Truncate
    {
        public static IEnumerable<object[]> Data { get; } = DataSets.CreateRoundingDataSet(MathF.Truncate);

        [Theory]
        [MemberData(nameof(Data))]
        public static void Truncate_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.Truncate(value);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}