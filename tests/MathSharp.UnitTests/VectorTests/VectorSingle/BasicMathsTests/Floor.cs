using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Floor
    {
        public static IEnumerable<object[]> Data { get; } = DataSets.Single.CreateRoundingDataSet(MathF.Floor);

        [Theory]
        [MemberData(nameof(Data))]
        public static void Floor_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.Floor(value);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}