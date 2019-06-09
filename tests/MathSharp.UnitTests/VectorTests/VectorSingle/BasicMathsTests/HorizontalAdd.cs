using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BasicMathsTests
{
    public class HorizontalAdd
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                static Vector4 HorizontalAdd(Vector4 left, Vector4 right)
                {
                    return new Vector4(left.X + left.Y, left.Z + left.W, right.X + right.Y, right.Z + right.W);
                }

                static Vector4 HorizontalAddF(float left, float right)
                {
                    return HorizontalAdd(new Vector4(left), new Vector4(right));
                }

                return new[]
                {
                    new object[]
                    {
                        Vector128.Create(0f), Vector128.Create(0f),
                        HorizontalAddF(0f, 0f)
                    },
                    new object[]
                    {
                        Vector128.Create(1f), Vector128.Create(1f),
                        HorizontalAddF(1f, 1f)
                    },
                    new object[]
                    {
                        Vector128.Create(10f, -10f, float.MinValue, float.MaxValue), Vector128.Create(0.0005f, 0.1f, 2343242f, -123123123123f),
                        HorizontalAdd(new Vector4(10f, -10f, float.MinValue, float.MaxValue), new Vector4(0.0005f, 0.1f, 2343242f, -123123123123f))
                    },
                };
            }
        }


        [Theory]
        [MemberData(nameof(Data))]
        public void HorizontalAdd_Theory(Vector128<float> left, Vector128<float> right, Vector4 expected)
        {
            Vector128<float> result = Vector.HorizontalAdd(left, right);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}