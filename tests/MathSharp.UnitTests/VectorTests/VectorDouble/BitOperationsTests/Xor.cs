using System.Collections.Generic;
using OpenTK;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.BitOperationsTests
{
    public class XorTests
    {
        public static readonly double AllBitsSet = Unsafe.As<int, double>(ref Unsafe.AsRef(-1));

        public static IEnumerable<object[]> Data
        {
            get
            {
                static double XorF(double a, double b)
                {
                    ulong xor = Unsafe.As<double, ulong>(ref a) ^ Unsafe.As<double, ulong>(ref b);
                    return Unsafe.As<ulong, double>(ref xor);
                }

                return new[]
                {
                    new object[] { Vector256.Create(0d), Vector256.Create(0d), new Vector4d(0d) },
                    new object[] { Vector256.Create(AllBitsSet), Vector256.Create(AllBitsSet), new Vector4d(0d) },
                    new object[] { Vector256.Create(235434d, -123d, 0, double.MaxValue),  Vector256.Create(235434d, -123d, 0, double.MaxValue), new Vector4d(0d) },
                    new object[]
                    {
                        Vector256.Create(0d,  double.MinValue,   double.PositiveInfinity,  1414123d), Vector256.Create(double.NaN, -0.00000000023434d, double.NegativeInfinity, 0),
                        new Vector4d(XorF(0d, double.NaN), XorF(double.MinValue, -0.00000000023434d), XorF(double.PositiveInfinity, double.NegativeInfinity), XorF(1414123d, 0d))
                    }
                };
            }
        }

        [Theory]
        // TODO wtf
#pragma warning disable xUnit1019
        [MemberData(nameof(Data))]
#pragma warning enable xUnit1019
        public void Xor_Theory(Vector256<double> left, Vector256<double> right, Vector4d expected)
        {
            Vector256<double> vector = Vector.Xor(left, right);

            Assert.True(TestHelpers.AreEqual(expected, vector));
        }
    }
}