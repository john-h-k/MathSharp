using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.VectorOperations
{
    public class Lerp
    {
        public static IEnumerable<object[]> Data()
        {
            var objs = new[]
            {
                new object[] {Vector256.Create(0d), Vector256.Create(0d), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(0d), Vector256.Create(0d), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(0d), Vector256.Create(0d), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(0d), Vector256.Create(0d), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(1d), Vector256.Create(1d), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(1d), Vector256.Create(1d), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(1d), Vector256.Create(1d), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(1d), Vector256.Create(1d), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(-1d), Vector256.Create(-1d), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(-1d), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(-1d), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(-1d), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(-1d), Vector256.Create(1d, 4d, 9d, 16d), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(1d, 4d, 9d, 16d), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(1d, 4d, 9d, 16d), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(1d, 4d, 9d, 16d), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(1d, 2d, 3d, 4d), Vector256.Create(4d, -5d, 6d, 9d), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(1d, 2d, 3d, 4d), Vector256.Create(4d, -5d, 6d, 9d), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(1d, 2d, 3d, 4d), Vector256.Create(4d, -5d, 6d, 9d), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(1d, 2d, 3d, 4d), Vector256.Create(4d, -5d, 6d, 9d), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.PositiveInfinity), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.PositiveInfinity), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.PositiveInfinity), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.PositiveInfinity), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.NegativeInfinity), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.NegativeInfinity), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.NegativeInfinity), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.NegativeInfinity), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(double.NaN), Vector256.Create(double.NegativeInfinity), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(double.NaN), Vector256.Create(double.NegativeInfinity), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(double.NaN), Vector256.Create(double.NegativeInfinity), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(double.NaN), Vector256.Create(double.NegativeInfinity), 1.0d, default(Vector4d)},

                new object[] {Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), 0.1d, default(Vector4d)},
                new object[] {Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), 0.5d, default(Vector4d)},
                new object[] {Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), 0.0d, default(Vector4d)},
                new object[] {Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), 1.0d, default(Vector4d)},

            };

            foreach (object[] set in objs)
            {
                Vector4d v1 = TestHelpers.ByValToSlowVector4d((Vector256<double>)set[0]);
                Vector4d v2 = TestHelpers.ByValToSlowVector4d((Vector256<double>)set[1]);
                var weight = (double)set[2];

                set[3] = Vector4d.Lerp(v1, v2, weight);
            }

            return objs;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public static void Lerp_Theory(Vector256<double> left, Vector256<double> right, double weight, Vector4d expected)
        {
            Vector256<double> result = Vector.Lerp(left, right, weight);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}