using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using MathSharp.Utils;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorDouble.VectorOperations
{
    public class CrossProduct
    {
        public static IEnumerable<object[]> Data(VectorDimensions dimension)
        {
            var objs = new[]
            {
                new object[4] {Vector256.Create(0d), Vector256.Create(0d), Vector256.Create(0d), default(Vector4d)},
                new object[4] {Vector256.Create(1d), Vector256.Create(1d), Vector256.Create(1d), default(Vector4d)},
                new object[4] {Vector256.Create(-1d), Vector256.Create(-1d), Vector256.Create(-1d), default(Vector4d)},
                new object[4] {Vector256.Create(-1d), Vector256.Create(1d, 4d, 9d, 16d), Vector256.Create(100d, 200d, 300d, 400d), default(Vector4d)},
                new object[4] {Vector256.Create(1d, 2d, 3d, 4d), Vector256.Create(4d, -5d, 6d, 9d), Vector256.Create(0d), default(Vector4d)},
                new object[4] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.PositiveInfinity), Vector256.Create(0d), default(Vector4d)},
                new object[4] {Vector256.Create(double.PositiveInfinity), Vector256.Create(double.NegativeInfinity), Vector256.Create(0d), default(Vector4d)},
                new object[4] {Vector256.Create(double.NaN), Vector256.Create(double.NegativeInfinity), Vector256.Create(double.PositiveInfinity), default(Vector4d)},
                new object[4] {Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), Vector256.Create(double.MaxValue, double.MinValue, double.NaN, 0), Vector256.Create(10d), default(Vector4d)},
            };

            foreach (object[] set in objs)
            {
                switch (dimension)
                {
                    case VectorDimensions.V2D:
                        {
                            var v1 = (Vector256<double>)set[0];
                            var v2 = (Vector256<double>)set[1];
                            set[3] = TestHelpers.ByValToSlowVector2d(SoftwareFallbacks.CrossProduct2D_Software(v1, v2));
                            break;
                        }

                    case VectorDimensions.V3D:
                        {
                            var v1 = (Vector256<double>)set[0];
                            var v2 = (Vector256<double>)set[1];
                            set[3] = TestHelpers.ByValToSlowVector3d(SoftwareFallbacks.CrossProduct3D_Software(v1, v2));
                            break;
                        }

                    case VectorDimensions.V4D:
                        {
                            var v1 = (Vector256<double>)set[0];
                            var v2 = (Vector256<double>)set[1];
                            var v3 = (Vector256<double>)set[2];
                            set[3] = TestHelpers.ByValToSlowVector4d(SoftwareFallbacks.CrossProduct4D_Software(v1, v2, v3));
                            break;
                        }

                    default:
                        throw new ArgumentException(nameof(dimension));
                }
            }

            return objs;
        }

#pragma warning disable xUnit1026

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V2D)]
        public static void CrossProduct2D_Theory(Vector256<double> left, Vector256<double> right, Vector256<double> discard, Vector2d expected)
        {
            Vector256<double> result = Vector.CrossProduct2D(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V3D)]
        public static void CrossProduct3D_Theory(Vector256<double> left, Vector256<double> right, Vector256<double> discard, Vector3d expected)
        {
            Vector256<double> result = Vector.CrossProduct3D(left, right);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

#pragma warning enable xUnit1026

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V4D)]
        public static void CrossProduct4D_Theory(Vector256<double> left, Vector256<double> right, Vector256<double> third, Vector4d expected)
        {
            Vector256<double> result = Vector.CrossProduct4D(left, right, third);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}