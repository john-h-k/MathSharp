using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using OpenTK.Graphics.OpenGL;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class CopySign
    {
        public static IEnumerable<object[]> Data => DataSets.Single.CreateBinaryDataSet(MathF.CopySign);

        [Theory]
        [MemberData(nameof(Data))]
        public static void CopySign_Theory(Vector128<float> a, Vector128<float> b, Vector128<float> expected)
        {
            a = Vector.CopySign(a, b);

            Assert.True(PerElemCheck(a, expected, (f, exp) => f.Equals(exp)));
        }
    }
}