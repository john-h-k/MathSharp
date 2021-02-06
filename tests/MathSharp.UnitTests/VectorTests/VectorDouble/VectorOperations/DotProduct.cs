using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.VectorOperations
{
    public class DotProduct
    {
        public static IEnumerable<object[]> Data(VectorDimensions dimension)
        {
            var objs = new[]
            {
                new object[] {Vector256.Create(0d), Vector256.Create(0d), default(Vector4d)},
                new object[] {Vector256.Create(1d), Vector256.Create(1d), default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(-1d), default(Vector4d)},
                new object[] {Vector256.Create(-1d), Vector256.Create(1d, 4d, 9d, 16d), default(Vector4d)},
                new object[] {Vector256.Create(1d, 2d, 3d, 4d), Vector256.Create(4d, -5d, 6d, 9d), default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.PositiveInfinity), default(Vector4d)},
                new object[] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.NegativeInfinity), default(Vector4d)},
                new object[] {Vector256.Create(double.NaN), Vector256.Create(double.NegativeInfinity), default(Vector4d)},
                new object[] {Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), default(Vector4d)},
            };

            foreach (object[] set in objs)
            {
                switch (dimension)
                {
                    case VectorDimensions.V2D:
                        {
                            Vector2d v1 = TestHelpers.ByValToSlowVector2d(((Vector256<double>)set[0]));
                            Vector2d v2 = TestHelpers.ByValToSlowVector2d(((Vector256<double>)set[1]));
                            double dot = Vector2d.Dot(v1, v2);
                            set[2] = new Vector2d(dot);
                            break;
                        }

                    case VectorDimensions.V3D:
                        {
                            Vector3d v1 = TestHelpers.ByValToSlowVector3d(((Vector256<double>)set[0]));
                            Vector3d v2 = TestHelpers.ByValToSlowVector3d(((Vector256<double>)set[1]));
                            double dot = Vector3d.Dot(v1, v2);
                            set[2] = new Vector3d(dot);
                            break;
                        }

                    case VectorDimensions.V4D:
                        {
                            Vector4d v1 = TestHelpers.ByValToSlowVector4d(((Vector256<double>)set[0]));
                            Vector4d v2 = TestHelpers.ByValToSlowVector4d(((Vector256<double>)set[1]));
                            double dot = Vector4d.Dot(v1, v2);
                            set[2] = new Vector4d(dot);
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
        public static void Dot2D_Theory(Vector256<double> left, Vector256<double> right, Vector2d expected)
        {
            Vector256<double> result = Vector.Dot2D(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V3D)]
        public static void Dot3D_Theory(Vector256<double> left, Vector256<double> right, Vector3d expected)
        {
            Vector256<double> result = Vector.Dot3D(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V4D)]
        public static void Dot4D_Theory(Vector256<double> left, Vector256<double> right, Vector4d expected)
        {
            Vector256<double> result = Vector.Dot4D(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}