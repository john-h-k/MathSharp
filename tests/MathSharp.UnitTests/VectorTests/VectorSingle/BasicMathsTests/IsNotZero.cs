using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class IsNotZero
    {
        public static IEnumerable<object[]> Data => DataSets.CreateUnaryDataSet(x => x != 0 ? Utils.Helpers.AllBitsSetSingle : 0f);

        [Theory]
        [MemberData(nameof(Data))]
        public static void IsNotZero_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.IsNotZero(value);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}