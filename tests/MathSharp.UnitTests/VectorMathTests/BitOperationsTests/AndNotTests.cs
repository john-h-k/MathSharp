using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Xunit;
using BitOperations = MathSharp.VectorF.BitOperations;

namespace MathSharp.UnitTests.VectorMathTests.BitOperationsTests
{
    public class AndNotTests
    {
        [Fact]
        public void AndNot_AllBitsZeroAndNotWithSelf_ExpectedValueAllBitsZero()
        {
            Vector128<float> vector = Vector128.Create(0f);
            var expected = new Vector4(0f);

            vector = BitOperations.AndNot(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void AndNot_AllBitsSetAndNotWithSelf_ExpectedValueAllBitsZero()
        {
            Vector128<float> vector = Vector128.Create(-1).AsSingle();
            var expected = new Vector4(0f);

            vector = BitOperations.AndNot(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void AndNot_MixedBitsSetAndNotWithSelf_ExpectedValueAllBitsZero()
        {
            Vector128<float> vector = Vector128.Create(235434f, -123f, 0, float.MaxValue);
            var expected = new Vector4(0f);

            vector = BitOperations.AndNot(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void AndNot_AllBitsSetAndNotWithAllBitsZero_ExpectedValueAllBitsZero()
        {
            Vector128<float> allBitsSet = Vector128.Create(-1).AsSingle();
            Vector128<float> noBitsSet = Vector128.Create(0f);
            var expected = new Vector4(0f);

            Vector128<float> result = BitOperations.AndNot(allBitsSet, noBitsSet);

            Assert.True(Helpers.AreEqual(expected, result));

        }

        [Fact] public void AndNot_AllBitsZeroAndNotWithAllBitsZero_ExpectedValueAllBitsZero()
        {
            Vector128<float> allBitsSet = Vector128.Create(-1).AsSingle();
            Vector128<float> noBitsSet = Vector128.Create(0f);
            var expected = new Vector4(0f);

            Vector128<float> result = BitOperations.AndNot(allBitsSet, noBitsSet);

            Assert.True(Helpers.AreEqual(expected, result));

        }

        [Fact]
        public void AndNot_AllBitsSetAndNotWithAllBitsSet_ExpectedValueAllBitsSet()
        {
            Vector128<float> allBitsSet = Vector128.Create(-1).AsSingle();
            Vector128<float> noBitsSet = Vector128.Create(0f);
            var expected = new Vector4(0f);

            Vector128<float> result = BitOperations.AndNot(allBitsSet, noBitsSet);

            Assert.True(Helpers.AreEqual(expected, result));
        }

        [Fact]
        public void AndNot_MixedBitsSetAndNotWithMixedBitsSet_ExpectedValueFromField()
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

            Vector128<float> result = BitOperations.AndNot(vector1, vector2);

            Assert.True(Helpers.AreEqual(expected, result));

            Vector4 GetExpectedValue()
            {
                return new Vector4(
                    AndNotF(val1_1, val2_1),
                    AndNotF(val1_2, val2_2),
                    AndNotF(val1_3, val2_3),
                    AndNotF(val1_4, val2_4)
                );
            }

            float AndNotF(float a, float b)
            {
                uint andNot = ~Unsafe.As<float, uint>(ref a) & Unsafe.As<float, uint>(ref b);
                return Unsafe.As<uint, float>(ref andNot);
            }
        }
    }
}