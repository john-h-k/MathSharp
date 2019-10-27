using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Round
    {
        public class Floor
        {
            public static IEnumerable<object[]> Data { get; } = DataSets.CreateRoundingDataSet(MathF.Round);

            [Theory]
            [MemberData(nameof(Data))]
            public static void Round_Theory(Vector128<float> value, Vector128<float> expected)
            {
                value = Vector.Round(value);

                Assert.True(TestHelpers.AreEqual(value, expected));
            }
        }
    }
}