using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.VectorOperations
{
    public class Normalize
    {
        public static IEnumerable<object[]> Data(VectorDimensions dimension)
        {
            var objs = new[]
            {
                new object[] {Vector256.Create(0d), default(Vector4d)},
                new object[] {Vector256.Create(1d), default(Vector4d)},
                new object[] {Vector256.Create(-1d), default(Vector4d)},
                new object[] {Vector256.Create(-1d), default(Vector4d)},
                new object[] {Vector256.Create(1d, 2d, 3d, 4d), default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), default(Vector4d)},
                new object[] {Vector256.Create(double.NaN), default(Vector4d)},
                new object[] {Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), default(Vector4d)},
            };

            foreach (object[] set in objs)
            {
                switch (dimension)
                {
                    case VectorDimensions.V2D:
                        {
                            Vector2d v1 = TestHelpers.ByValToSlowVector2d(((Vector256<double>)set[0]));
                            set[1] = Vector2d.Normalize(v1);
                            break;
                        }

                    case VectorDimensions.V3D:
                        {
                            Vector3d v1 = TestHelpers.ByValToSlowVector3d(((Vector256<double>)set[0]));
                            set[1] = Vector3d.Normalize(v1);
                            break;
                        }

                    case VectorDimensions.V4D:
                        {
                            Vector4d v1 = TestHelpers.ByValToSlowVector4d(((Vector256<double>)set[0]));
                            set[1] = Vector4d.Normalize(v1);
                            break;
                        }

                    default:
                        throw new ArgumentException(nameof(dimension));
                }
            }

            return objs;
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V2D)]
        public static void Normalize2D_Theory(Vector256<double> vector, Vector2d expected)
        {
            Vector256<double> result = Vector.Normalize2D(vector);

            Assert.True(TestHelpers.AreApproxEqual(expected, result));
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V3D)]
        public static void Normalize3D_Theory(Vector256<double> vector, Vector3d expected)
        {
            Vector256<double> result = Vector.Normalize3D(vector);

            Assert.True(TestHelpers.AreApproxEqual(expected, result));
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V4D)]
        public static void Normalize4D_Theory(Vector256<double> vector, Vector4d expected)
        {
            Vector256<double> result = Vector.Normalize4D(vector);

            Assert.True(TestHelpers.AreApproxEqual(expected, result, 0.00001d), $"Expected: {expected}, got {result}");
        }
    }
}