using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.BitOperationsTests
{
    public class NotTests
    {
        public static readonly float AllBitsSet = Unsafe.As<int, float>(ref Unsafe.AsRef(-1));

        public static IEnumerable<object[]> Data
        {
            get
            {
                static float NotF(float a)
                {
                    uint not = ~Unsafe.As<float, uint>(ref a);
                    return Unsafe.As<uint, float>(ref not);
                }

                return new[]
                {
                    new object[] { Vector128.Create(0f), new Vector4(AllBitsSet) },
                    new object[] { Vector128.Create(AllBitsSet), new Vector4(0f) },
                    new object[] { Vector128.Create(235434f, -123f, 0, float.MaxValue),  new Vector4(NotF(235434f), NotF(-123f), NotF(0f), NotF(float.MaxValue)) }
                };
            }
        }

        [Theory]
        // TODO wtf
#pragma warning disable xUnit1019
        [MemberData(nameof(Data))]
#pragma warning enable xUnit1019
        public void Not_Theory(Vector128<float> value, Vector4 expected)
        {
            Vector128<float> vector = Vector.Not(value);

            Assert.True(Helpers.AreEqual(expected, vector));
        }
    }
}