using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class IsZero
    {
        public static IEnumerable<object[]> Data => DataSets.CreateUnaryDataSet(x => x == 0 ? Utils.Helpers.AllBitsSetSingle : 0f);

        [Theory]
        [MemberData(nameof(Data))]
        public static void IsZero_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.IsZero(value);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}