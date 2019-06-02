using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Xunit;
using BitOperations = MathSharp.VectorF.BitOperations;

namespace MathSharp.UnitTests.VectorMathTests.BitOperationsTests
{
    public class OrTests
    {
        [Fact]
        public void Or_AllBitsZeroOrWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(0f);
            var expected = new Vector4(0);

            vector = BitOperations.Or(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }



        [Fact]
        public void Or_AllBitsSetOrWithSelf_ExpectedValueAllBitsSet()
        {
            Vector128<float> vector = Vector128.Create(-1).AsSingle();
            int m1 = -1;
            float notZero = Unsafe.As<int, float>(ref m1);
            var expected = new Vector4(notZero);

            vector = BitOperations.Or(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void Or_MixedBitsSetOrWithSelf_ExpectedValueNoChange()
        {
            var val1 = 235434f;
            var val2 = -123f;
            var val3 = 0f;
            var val4 = float.MaxValue;
            Vector128<float> vector = Vector128.Create(val1, val2, val3, val4);
            var expected = new Vector4(val1, val2, val3, val4);

            vector = BitOperations.Or(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void Or_AllBitsSetWithNoBitsSet_ExpectedValueAllBitsSet()
        {
            Vector128<float> allBitsSet = Vector128.Create(-1).AsSingle();
            Vector128<float> noBitsSet = Vector128.Create(0f);
            int m1 = -1;
            float notZero = Unsafe.As<int, float>(ref m1);
            var expected = new Vector4(notZero);

            Vector128<float> result = BitOperations.Or(allBitsSet, noBitsSet);

            Assert.True(Helpers.AreEqual(expected, result));
        }

        [Fact]
        public void Or_MixedBitsSetWithMixedBitsSet_ExpectedValueFromField()
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

            Vector128<float> result = BitOperations.Or(vector1, vector2);

            Assert.True(Helpers.AreEqual(expected, result));

            Vector4 GetExpectedValue()
            {
                return new Vector4(
                    OrF(val1_1, val2_1),
                    OrF(val1_2, val2_2),
                    OrF(val1_3, val2_3),
                    OrF(val1_4, val2_4)
                );
            }

            float OrF(float a, float b)
            {
                uint or = Unsafe.As<float, uint>(ref a) | Unsafe.As<float, uint>(ref b);
                return Unsafe.As<uint, float>(ref or);
            }
        }
    }
}