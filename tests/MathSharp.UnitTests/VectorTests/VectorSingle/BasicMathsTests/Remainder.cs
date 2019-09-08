using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class Remainder
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                new object[] { 10.4f, 2f},
                new object[] {-10.4f, 2f}
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Remainder_Theory(float left, float right)
        {
            var result = Vector.Remainder(Vector128.Create(left), right);
            Vector.Store(result, out Vector4 l);
            var r = new Vector4(left % right);

            Assert.Equal(l, r);
        }
    }
}
