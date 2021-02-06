using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

using SysVector2 = System.Numerics.Vector2;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.VectorOperations
{
    public class CrossProduct
    {
        public static IEnumerable<object[]> Data(VectorDimensions dimension)
        {
            var objs = new[]
            {
                new object[] {Vector128.Create(0f), Vector128.Create(0f), Vector128.Create(0f), default(Vector4)},
                new object[] {Vector128.Create(1f), Vector128.Create(1f), Vector128.Create(1f), default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(-1f), Vector128.Create(-1f), default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(1f, 4f, 9f, 16f), Vector128.Create(100f, 200f, 300f, 400f), default(Vector4)},
                new object[] {Vector128.Create(1f, 2f, 3f, 4f), Vector128.Create(4f, -5f, 6f, 9f), Vector128.Create(0f), default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.PositiveInfinity), Vector128.Create(0f), default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.NegativeInfinity), Vector128.Create(0f), default(Vector4)},
                new object[] {Vector128.Create(float.NaN), Vector128.Create(float.NegativeInfinity), Vector128.Create(float.PositiveInfinity), default(Vector4)},
                new object[] {Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), Vector128.Create(10f), default(Vector4)},
            };

            foreach (object[] set in objs)
            {
                switch (dimension)
                {
                    case VectorDimensions.V2D:
                        {
                            var v1 = (Vector128<float>)set[0];
                            var v2 = (Vector128<float>)set[1];
                            set[3] = ByValToSlowVector2(SoftwareFallbacks.CrossProduct2D_Software(v1, v2));
                            break;
                        }

                    case VectorDimensions.V3D:
                        {
                            var v1 = (Vector128<float>)set[0];
                            var v2 = (Vector128<float>)set[1];
                            Vector128<float> res = SoftwareFallbacks.CrossProduct3D_Software(v1, v2);
                            set[3] = ByValToSlowVector3(res);
                            Vector3 correct = Vector3.Cross(ByValToSlowVector3(v1), ByValToSlowVector3(v2));
                            Assert.True(AreEqual(Vector3.Cross(ByValToSlowVector3(v1), ByValToSlowVector3(v2)), res), $"Expected {correct}, got {res}");
                            break;
                        }

                    case VectorDimensions.V4D:
                        {
                            var v1 = (Vector128<float>)set[0];
                            var v2 = (Vector128<float>)set[1];
                            var v3 = (Vector128<float>)set[2];
                            set[3] = ByValToSlowVector4(SoftwareFallbacks.CrossProduct4D_Software(v1, v2, v3));
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
        public static void CrossProduct2D_Theory(Vector128<float> left, Vector128<float> right, Vector128<float> discard, SysVector2 expected)
        {
            Vector128<float> result = Vector.CrossProduct2D(left, right);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V3D)]
        public static void CrossProduct3D_Theory(Vector128<float> left, Vector128<float> right, Vector128<float> discard, Vector3 expected)
        {
            Vector128<float> result = Vector.CrossProduct3D(left, right);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

#pragma warning restore xUnit1026

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V4D)]
        public static void CrossProduct4D_Theory(Vector128<float> left, Vector128<float> right, Vector128<float> third, Vector4 expected)
        {
            Vector128<float> result = Vector.CrossProduct4D(left, right, third);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}