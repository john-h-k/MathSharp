using System.Collections.Generic;
using OpenTK;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BitOperationsTests
{
    public class OrTests
    {
        public static readonly float AllBitsSet = Unsafe.As<int, float>(ref Unsafe.AsRef(-1));

        public static IEnumerable<object[]> Data
        {
            get
            {
                static float OrF(float a, float b)
                {
                    uint or = Unsafe.As<float, uint>(ref a) | Unsafe.As<float, uint>(ref b);
                    return Unsafe.As<uint, float>(ref or);
                }

                return new[]
                {
                    new object[] { Vector128.Create(0f), Vector128.Create(0f), new Vector4(0f) },
                    new object[] { Vector128.Create(AllBitsSet), Vector128.Create(AllBitsSet), new Vector4(AllBitsSet) },
                    new object[] { Vector128.Create(235434f, -123f, 0, float.MaxValue),  Vector128.Create(235434f, -123f, 0, float.MaxValue), new Vector4(235434f, -123f, 0, float.MaxValue) },
                    new object[]
                    {
                        Vector128.Create(0f,  float.MinValue,   float.PositiveInfinity,  1414123f), Vector128.Create(float.NaN, -0.00000000023434f, float.NegativeInfinity, 0),
                        new Vector4(OrF(0f, float.NaN), OrF(float.MinValue, -0.00000000023434f), OrF(float.PositiveInfinity, float.NegativeInfinity), OrF(1414123f, 0f))
                    }
                };
            }
        }

        [Theory]
        // TODO wtf
#pragma warning disable xUnit1019
        [MemberData(nameof(Data))]
#pragma warning enable xUnit1019
        public void Or_Theory(Vector128<float> left, Vector128<float> right, Vector4 expected)
        {
            Vector128<float> vector = Vector.Or(left, right);

            Assert.True(TestHelpers.AreEqual(expected, vector));
        }
    }
}