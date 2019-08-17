using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BasicMathsTests
{
    public class HorizontalAdd
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                static Vector4d HorizontalAdd(Vector4d left, Vector4d right)
                {
                    return new Vector4d(left.X + left.Y, right.X + right.Y, left.Z + left.W,  right.Z + right.W);
                }

                static Vector4d HorizontalAddD(double left, double right)
                {
                    return HorizontalAdd(new Vector4d(left), new Vector4d(right));
                }

                return new[]
                {
                    new object[]
                    {
                        Vector256.Create(0d), Vector256.Create(0d),
                        HorizontalAddD(0d, 0d)
                    },
                    new object[]
                    {
                        Vector256.Create(1d), Vector256.Create(1d),
                        HorizontalAddD(1d, 1d)
                    },
                    new object[]
                    {
                        Vector256.Create(10d, -10d, double.MinValue, double.MaxValue), Vector256.Create(0.0005d, 0.1d, 2343242d, -123123123123d),
                        HorizontalAdd(new Vector4d(10d, -10d, double.MinValue, double.MaxValue), new Vector4d(0.0005d, 0.1d, 2343242d, -123123123123d))
                    },
                };
            }
        }


        [Theory]
        [MemberData(nameof(Data))]
        public void HorizontalAdd_Theory(Vector256<double> left, Vector256<double> right, Vector4d expected)
        {
            Vector256<double> result = Vector.HorizontalAdd(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}