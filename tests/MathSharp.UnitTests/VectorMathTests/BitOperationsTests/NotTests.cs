using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static MathSharp.Helpers;
using static MathSharp.VectorF;
using Microsoft.VisualBasic.CompilerServices;
using Xunit;

namespace MathSharp.UnitTests.VectorMathTests.BitOperationsTests
{
    public class NotTests
    {
        [Fact]
        public void Not_AllBitsZeroNot_ExpectedValueAllBitsSet()
        {
            Vector128<float> vector = Vector128.Create(0f);
            int m1 = -1;
            float notZero = Unsafe.As<int, float>(ref m1);
            var expected = new Vector4(notZero);

            vector = VectorF.Not(vector);

            Assert.True(AreEqual(expected, vector));
        }

        [Fact]
        public void Not_AllBitsSetNot_ExpectedValueAllBitsZero()
        {
            Vector128<float> vector = Vector128.Create(-1).AsSingle();
            var expected = new Vector4(0f);

            vector = VectorF.Not(vector);

            Assert.True(AreEqual(expected, vector));
        }

        [Fact]
        public void Not_MixedBitsSetNot_ExpectedValueFromField()
        {
            var val1 = 235434f;
            var val2 = -123f;
            var val3 = 0f;
            var val4 = float.MaxValue;
            Vector128<float> vector = Vector128.Create(val1, val2, val3, val4);
            var expected = GetExpectedValue();

            vector = VectorF.Not(vector);

            Assert.True(AreEqual(expected, vector));

            Vector4 GetExpectedValue()
            {
                return new Vector4(
                    NotF(val1),
                    NotF(val2),
                    NotF(val3),
                    NotF(val4)
                );
            }

            static float NotF(float value)
            {
                int not = ~Unsafe.As<float, int>(ref value);
                return Unsafe.As<int, float>(ref not);
            }
        }
    }
}