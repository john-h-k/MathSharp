using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.VectorOperations
{
    public class LengthSquared
    {
        public static IEnumerable<object[]> Data(VectorDimensions dimension)
        {
            var objs = new[]
            {
                new object[] {Vector128.Create(0f), default(Vector4)},
                new object[] {Vector128.Create(1f), default(Vector4)},
                new object[] {Vector128.Create(-1f), default(Vector4)},
                new object[] {Vector128.Create(-1f), default(Vector4)},
                new object[] {Vector128.Create(1f, 2f, 3f, 4f), default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), default(Vector4)},
                new object[] {Vector128.Create(float.NaN), default(Vector4)},
                new object[] {Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), default(Vector4)},
            };

            foreach (object[] set in objs)
            {
                switch (dimension)
                {
                    case VectorDimensions.V2D:
                        {
                            Vector2 v1 = TestHelpers.ByValToSlowVector2(((Vector128<float>)set[0]));
                            float dot = v1.LengthSquared;
                            set[1] = new Vector2(dot);
                            break;
                        }

                    case VectorDimensions.V3D:
                        {
                            Vector3 v1 = TestHelpers.ByValToSlowVector3(((Vector128<float>)set[0]));
                            float dot = v1.LengthSquared;
                            set[1] = new Vector3(dot);
                            break;
                        }

                    case VectorDimensions.V4D:
                        {
                            Vector4 v1 = TestHelpers.ByValToSlowVector4(((Vector128<float>)set[0]));
                            float dot = v1.LengthSquared;
                            set[1] = new Vector4(dot);
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
        public static void Length2D_Theory(Vector128<float> vector, Vector2 expected)
        {
            Vector128<float> result = Vector.LengthSquared2D(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V3D)]
        public static void Length3D_Theory(Vector128<float> vector, Vector3 expected)
        {
            Vector128<float> result = Vector.LengthSquared3D(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V4D)]
        public static void Length4D_Theory(Vector128<float> vector, Vector4 expected)
        {
            Vector128<float> result = Vector.LengthSquared4D(vector);

            Assert.True(TestHelpers.AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}