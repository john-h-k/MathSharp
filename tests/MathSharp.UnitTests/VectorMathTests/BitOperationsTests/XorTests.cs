using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Xunit;
using BitOperations = MathSharp.VectorFloat.BitOperations;

namespace MathSharp.UnitTests.VectorMathTests.BitOperationsTests
{
    public class XorTests
    {
        // TODO-MAYBE Tests not against self?

        [Fact]
        public void Xor_AllBitsZeroXorWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(0f);
            var expected = new Vector4(0);

            vector = BitOperations.Xor(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void Xor_AllBitsSetXorWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(-1).AsSingle();
            var expected = new Vector4(0);

            vector = BitOperations.Xor(vector, vector);

            Assert.True(Helpers.AreEqual(expected, vector));
        }

        [Fact]
        public void Xor_MixedBitsSetXorWithSelf_ExpectedValueZero()
        {
            Vector128<float> vector = Vector128.Create(235434f, -123f, 0, float.MaxValue);
            var expected = new Vector4(0);

            vector = BitOperations.Xor(vector, vector);

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

            Vector128<float> result = BitOperations.Xor(allBitsSet, noBitsSet);

            Assert.True(Helpers.AreEqual(expected, result));
        }

        [Fact]
        public void Xor_MixedBitsSetXorWithMixedBitsSet_ExpectedValueFromField()
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

            Vector128<float> result = BitOperations.Xor(vector1, vector2);

            Assert.True(Helpers.AreEqual(expected, result));

            Vector4 GetExpectedValue()
            {
                return new Vector4(
                    XorF(val1_1, val2_1),
                    XorF(val1_2, val2_2),
                    XorF(val1_3, val2_3),
                    XorF(val1_4, val2_4)
                );
            }

            float XorF(float a, float b)
            {
                uint xor = Unsafe.As<float, uint>(ref a) ^ Unsafe.As<float, uint>(ref b);
                return Unsafe.As<uint, float>(ref xor);
            }
        }
    }
}