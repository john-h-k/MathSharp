using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorMathTests.BitOperationsTests
{
    public class XorTests
    {
        public static readonly float AllBitsSet = Unsafe.As<int, float>(ref Unsafe.AsRef(-1));

        public static IEnumerable<object[]> Data
        {
            get
            {
                static float XorF(float a, float b)
                {
                    uint xor = Unsafe.As<float, uint>(ref a) ^ Unsafe.As<float, uint>(ref b);
                    return Unsafe.As<uint, float>(ref xor);
                }

                return new[]
                {
                    new object[] { Vector128.Create(0f), Vector128.Create(0f), new Vector4(0f) },
                    new object[] { Vector128.Create(AllBitsSet), Vector128.Create(AllBitsSet), new Vector4(0f) },
                    new object[] { Vector128.Create(235434f, -123f, 0, float.MaxValue),  Vector128.Create(235434f, -123f, 0, float.MaxValue), new Vector4(0f) },
                    new object[]
                    {
                        Vector128.Create(0f,  float.MinValue,   float.PositiveInfinity,  1414123f), Vector128.Create(float.NaN, -0.00000000023434f, float.NegativeInfinity, 0),
                        new Vector4(XorF(0f, float.NaN), XorF(float.MinValue, -0.00000000023434f), XorF(float.PositiveInfinity, float.NegativeInfinity), XorF(1414123f, 0f))
                    }
                };
            }
        }

        [Theory]
        // TODO wtf
#pragma warning disable xUnit1019
        [MemberData(nameof(Data))]
#pragma warning enable xUnit1019
        public void Xor_Theory(Vector128<float> left, Vector128<float> right, Vector4 expected)
        {
            Vector128<float> vector = Vector.Xor(left, right);

            Assert.True(Helpers.AreEqual(expected, vector));
        }
    }
}