using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class ReciprocalSqrt
    {
        public static IEnumerable<object[]> Data => DataSets.CreateUnaryDataSet(x => 1 / MathF.Sqrt(x));

        [Theory]
        [MemberData(nameof(Data))]
        public static void ReciprocalSqrt_Theory(Vector128<float> value, Vector128<float> expected)
        {
            value = Vector.ReciprocalSqrt(value);

            Assert.True(TestHelpers.AreEqual(value, expected));
        }
    }
}