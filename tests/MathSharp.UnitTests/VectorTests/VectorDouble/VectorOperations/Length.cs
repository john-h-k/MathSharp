using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.VectorOperations
{
    public class Length
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
                            double dot = v1.Length;
                            set[1] = new Vector2d(dot);
                            break;
                        }

                    case VectorDimensions.V3D:
                        {
                            Vector3d v1 = TestHelpers.ByValToSlowVector3d(((Vector256<double>)set[0]));
                            double dot = v1.Length;
                            set[1] = new Vector3d(dot);
                            break;
                        }

                    case VectorDimensions.V4D:
                        {
                            Vector4d v1 = TestHelpers.ByValToSlowVector4d(((Vector256<double>)set[0]));
                            double dot = v1.Length;
                            set[1] = new Vector4d(dot);
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
        public static void Length2D_Theory(Vector256<double> vector, Vector2d expected)
        {
            Vector256<double> result = Vector.Length2D(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V3D)]
        public static void Length3D_Theory(Vector256<double> vector, Vector3d expected)
        {
            Vector256<double> result = Vector.Length3D(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V4D)]
        public static void Length4D_Theory(Vector256<double> vector, Vector4d expected)
        {
            Vector256<double> result = Vector.Length4D(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}