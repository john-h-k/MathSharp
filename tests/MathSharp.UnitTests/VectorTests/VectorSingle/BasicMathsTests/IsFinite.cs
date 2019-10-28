using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class IsFinite
    {
        public static IEnumerable<object[]> Data =>
            DataSets.Single.CreateUnaryDataSet(x => float.IsFinite(x) ? Utils.Helpers.AllBitsSetSingle : 0f);

        [Theory]
        [MemberData(nameof(Data))]
        public static void IsFinite_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.IsFinite(value);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}