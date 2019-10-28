using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class IsInfinite
    {
        public static IEnumerable<object[]> Data => DataSets.Single.CreateUnaryDataSet(x => float.IsInfinity(x) ? Utils.Helpers.AllBitsSetSingle : 0f);

        [Theory]
        [MemberData(nameof(Data))]
        public static void IsInfinite_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.IsInfinite(value);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}