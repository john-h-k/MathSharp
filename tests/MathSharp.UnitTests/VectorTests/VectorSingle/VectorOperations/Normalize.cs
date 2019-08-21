using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.VectorOperations
{
    public class Normalize
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
                new object[] {Vector128.Create(15324.32354253f, 0.00000000213112f, 3222222222222222222222f, 4.2342342222222222f), default(Vector4)},
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
                            Vector2 v1 = ByValToSlowVector2((Vector128<float>)set[0]);
                            set[1] = Vector2.Normalize(v1);
                            break;
                        }

                    case VectorDimensions.V3D:
                        {
                            Vector3 v1 = ByValToSlowVector3((Vector128<float>)set[0]);
                            set[1] = Vector3.Normalize(v1);
                            break;
                        }

                    case VectorDimensions.V4D:
                        {
                            Vector4 v1 = ByValToSlowVector4((Vector128<float>)set[0]);
                            set[1] = Vector4.Normalize(v1);
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
        public static void Normalize2D_Theory(Vector128<float> vector, Vector2 expected)
        {
            Vector128<float> result = Vector.Normalize2D(vector);

            Assert.True(AreApproxEqual(expected, result));
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V3D)]
        public static void Normalize3D_Theory(Vector128<float> vector, Vector3 expected)
        {
            Vector128<float> result = Vector.Normalize3D(vector);

            Assert.True(AreApproxEqual(expected, result));
        }

        [Theory]
        [MemberData(nameof(Data), VectorDimensions.V4D)]
        public static void Normalize4D_Theory(Vector128<float> vector, Vector4 expected)
        {
            Vector128<float> result = Vector.Normalize4D(vector);

            Assert.True(AreApproxEqual(expected, result, 0.00001f), $"Expected: {expected}, got {result}");
        }
    }
}