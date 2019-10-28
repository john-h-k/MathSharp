using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BitOperationsTests
{
    public class AndTests
    {
        public static readonly double AllBitsSet = Unsafe.As<int, double>(ref Unsafe.AsRef(-1));

        public static IEnumerable<object[]> Data
        {
            get
            {
                static double AndF(double a, double b)
                {
                    ulong and = Unsafe.As<double, ulong>(ref a) & Unsafe.As<double, ulong>(ref b);
                    return Unsafe.As<ulong, double>(ref and);
                }

                return new[]
                {
                    new object[] { Vector256.Create(0d), Vector256.Create(0d), Vector256.Create(0d) },
                    new object[] { Vector256.Create(AllBitsSet), Vector256.Create(AllBitsSet), Vector256.Create(AllBitsSet) },
                    new object[] { Vector256.Create(235434d, -123d, 0, double.MaxValue),  Vector256.Create(235434d, -123d, 0, double.MaxValue), Vector256.Create(235434d, -123d, 0, double.MaxValue) },
                    new object[]
                    {
                        Vector256.Create(0d,  double.MinValue,   double.PositiveInfinity,  1414123d), Vector256.Create(double.NaN, -0.00000000023434d, double.NegativeInfinity, 0),
                        Vector256.Create(AndF(0d, double.NaN), AndF(double.MinValue, -0.00000000023434d), AndF(double.PositiveInfinity, double.NegativeInfinity), AndF(1414123d, 0d))
                    }
                };
            }
        }

        [Theory]
        // TODO wtf
#pragma warning disable xUnit1019
        [MemberData(nameof(Data))]
#pragma warning enable xUnit1019
        public void And_Theory(Vector256<double> left, Vector256<double> right, Vector256<double> expected)
        {
            Vector256<double> vector = Vector.And(left, right);

            Assert.True(TestHelpers.AreEqual(expected, vector));
        }
    }
}