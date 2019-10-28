using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class InBounds
    {
        public static IEnumerable<object[]> Data => DataSets.Single.CreateBinaryDataSet((val, bound) => val <= bound && val >= -bound ? Utils.Helpers.AllBitsSetSingle : 0f);

        [Theory]
        [MemberData(nameof(Data))]
        public static void InBounds_Theory(Vector128<float> value, Vector128<float> bounds, Vector128<float> expected)
        {
            value = Vector.InBounds(value, bounds);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}