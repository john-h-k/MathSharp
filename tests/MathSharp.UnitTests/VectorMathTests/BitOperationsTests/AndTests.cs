using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorMathTests.BitOperationsTests
{
    public class AndTests
    {
        [Fact]
        public void And_AllBitsZeroAndWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(0f);
            var expected = new Vector4(0f);

            vector = Vector.And(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void And_AllBitsSetAndWithSelf_ExpectedValueAllBitsSet()
        {
            Vector128<float> vector = Vector128.Create(-1).AsSingle();
            int m1 = -1;
            float notZero = Unsafe.As<int, float>(ref m1);
            var expected = new Vector4(notZero);

            vector = Vector.And(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void And_MixedBitsSetAndWithSelf_ExpectedSameValue()
        {
            float val1 = 235434f;
            float val2 = -123f;
            float val3 = 0;
            float val4 = float.MaxValue;

            Vector128<float> vector = Vector128.Create(val1, val2, val3, val4);
            var expected = new Vector4(val1, val2, val3, val4);

            vector = Vector.And(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void And_AllBitsSetAndWithNoBitsSet_ExpectedValueNoBitsSet()
        {
            Vector128<float> allBitsSet = Vector128.Create(-1).AsSingle();
            Vector128<float> noBitsSet = Vector128.Create(0f);
            var expected = new Vector4(0f);

            Vector128<float> result = Vector.And(allBitsSet, noBitsSet);

            Assert.True(Helpers.AreEqual(expected, result));
        }

        [Fact]
        public void And_MixedBitsSetAndWithMixedBitsSet_ExpectedValueFromField()
        {
            float val1_1 = 0;
            float val1_2 = float.MinValue;
            float val1_3 = float.PositiveInfinity;
            float val1_4 = 1414123f;

            float val2_1 = float.NaN;
            float val2_2 = -0.00000000023434f;
            float val2_3 = float.NegativeInfinity;
            float val2_4 = 0;

            Vector128<float> vector1 = Vector128.Create(val1_1, val1_2, val1_3, val1_4);
            Vector128<float> vector2 = Vector128.Create(val2_1, val2_2, val2_3, val2_4);
            var expected = GetExpectedValue();

            Vector128<float> result = Vector.And(vector1, vector2);

            Assert.True(Helpers.AreEqual(expected, result));

            Vector4 GetExpectedValue()
            {
                return new Vector4(
                    AndF(val1_1, val2_1),
                    AndF(val1_2, val2_2),
                    AndF(val1_3, val2_3),
                    AndF(val1_4, val2_4)
                );
            }

            float AndF(float a, float b)
            {
                uint and = Unsafe.As<float, uint>(ref a) & Unsafe.As<float, uint>(ref b);
                return Unsafe.As<uint, float>(ref and);
            }
        }
    }
}