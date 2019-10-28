using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class ExtractSign
    {
        public static IEnumerable<object[]> Data => DataSets.Single.CreateUnaryDataSet(x => IsNegative(x) ? -0f : 0f);

        [Theory]
        [MemberData(nameof(Data))]
        public static void ExtractSign_Theory(Vector128<float> a, Vector128<float> expected)
        {
            a = Vector.ExtractSign(a);

            Assert.True(PerElemCheck(a, expected, (f1, f2) => IsNegative(f1) == IsNegative(f2)));
        }
    }
}