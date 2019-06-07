using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorMathTests.BitOperationsTests
{
    public class XorTests
    {
        // TODO-MAYBE Tests not against self?

        public static readonly float AllBitsSet = Unsafe.As<int, float>(ref Unsafe.AsRef(-1));

        public static IEnumerable<object[]> XorData
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

        //[Theory]
        //[MemberData(nameof(XorData))]
        //public void Xor_Theory(Vector128<float> left, Vector128<float> right, Vector4 expected)
        //{
        //    Vector128<float> vector = Vector.Xor(left, right);

        //    Assert.True(Helpers.AreEqual(expected, vector));
        //}

        [Fact]
        public void Xor_AllBitsZeroXorWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(0f);
            var expected = new Vector4(0);

            vector = Vector.Xor(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void Xor_AllBitsSetXorWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(-1).AsSingle();
            var expected = new Vector4(0);

            vector = Vector.Xor(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void Xor_MixedBitsSetXorWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(235434f, -123f, 0, float.MaxValue);
            var expected = new Vector4(0);

            vector = Vector.Xor(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void Xor_AllBitsSetXorWithNoBitsSet_ExpectedValueAllBitsSet()
        {
            Vector128<float> allBitsSet = Vector128.Create(-1).AsSingle();
            Vector128<float> noBitsSet = Vector128.Create(0f);
            int m1 = -1;
            float notZero = Unsafe.As<int, float>(ref m1);
            var expected = new Vector4(notZero);

            Vector128<float> result = Vector.Xor(allBitsSet, noBitsSet);

            Assert.True(Helpers.AreEqual(expected, result));
        }

        //[Fact]
        //public void Xor_MixedBitsSetXorWithMixedBitsSet_ExpectedValueFromField()
        //{



        //    Vector128<float> vector1 = Vector128.Create(val1_1, val1_2, val1_3, val1_4);
        //    Vector128<float> vector2 = Vector128.Create(val2_1, val2_2, val2_3, val2_4);
        //    var expected = GetExpectedValue();

        //    Vector128<float> result = Vector.Xor(vector1, vector2);

        //    Assert.True(Helpers.AreEqual(expected, result));

        //    Vector4 GetExpectedValue()
        //    {
        //        return new Vector4(
        //            XorF(val1_1, val2_1),
        //            XorF(val1_2, val2_2),
        //            XorF(val1_3, val2_3),
        //            XorF(val1_4, val2_4)
        //        );
        //    }

        //    float XorF(float a, float b)
        //    {
        //        uint xor = Unsafe.As<float, uint>(ref a) ^ Unsafe.As<float, uint>(ref b);
        //        return Unsafe.As<uint, float>(ref xor);
        //    }
        //}
    }
}