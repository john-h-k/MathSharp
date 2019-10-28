using System.Collections.Generic;
using OpenTK;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BitOperationsTests
{
    public class NotTests
    {
        public static readonly double AllBitsSet = Unsafe.As<long, double>(ref Unsafe.AsRef(-1L));

        public static IEnumerable<object[]> Data
        {
            get
            {
                static double NotF(double a)
                {
                    ulong not = ~Unsafe.As<double, ulong>(ref a);
                    return Unsafe.As<ulong, double>(ref not);
                }

                return new[]
                {
                    new object[] { Vector256.Create(0d), new Vector4d(AllBitsSet) },
                    new object[] { Vector256.Create(AllBitsSet), new Vector4d(0d) },
                    new object[] { Vector256.Create(235434d, -123d, 0, double.MaxValue),  new Vector4d(NotF(235434d), NotF(-123d), NotF(0d), NotF(double.MaxValue)) }
                };
            }
        }

        [Theory]
        // TODO wtf
#pragma warning disable xUnit1019
        [MemberData(nameof(Data))]
#pragma warning enable xUnit1019
        public void Not_Theory(Vector256<double> value, Vector4d expected)
        {
            Vector256<double> vector = Vector.Not(value);

            Assert.True(TestHelpers.AreEqual(expected, vector));
        }
    }
}