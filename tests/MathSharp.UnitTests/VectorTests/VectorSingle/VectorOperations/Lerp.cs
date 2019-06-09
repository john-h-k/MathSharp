using System;
using System.Collections.Generic;
using OpenTK;
using System.Runtime.Intrinsics;
using Xunit;
using static MathSharp.UnitTests.TestHelpers;

namespace MathSharp.UnitTests.VectorTests.VectorSingle.VectorOperations
{
    public class Lerp
    {
        public static IEnumerable<object[]> Data()
        {
            var objs = new[]
            {
                new object[] {Vector128.Create(0f), Vector128.Create(0f), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(0f), Vector128.Create(0f), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(0f), Vector128.Create(0f), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(0f), Vector128.Create(0f), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(1f), Vector128.Create(1f), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(1f), Vector128.Create(1f), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(1f), Vector128.Create(1f), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(1f), Vector128.Create(1f), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(-1f), Vector128.Create(-1f), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(-1f), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(-1f), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(-1f), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(-1f), Vector128.Create(1f, 4f, 9f, 16f), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(1f, 4f, 9f, 16f), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(1f, 4f, 9f, 16f), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(-1f), Vector128.Create(1f, 4f, 9f, 16f), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(1f, 2f, 3f, 4f), Vector128.Create(4f, -5f, 6f, 9f), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(1f, 2f, 3f, 4f), Vector128.Create(4f, -5f, 6f, 9f), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(1f, 2f, 3f, 4f), Vector128.Create(4f, -5f, 6f, 9f), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(1f, 2f, 3f, 4f), Vector128.Create(4f, -5f, 6f, 9f), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.PositiveInfinity), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.PositiveInfinity), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.PositiveInfinity), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.PositiveInfinity), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.NegativeInfinity), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.NegativeInfinity), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.NegativeInfinity), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(float.PositiveInfinity), Vector128.Create(float.NegativeInfinity), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(float.NaN), Vector128.Create(float.NegativeInfinity), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(float.NaN), Vector128.Create(float.NegativeInfinity), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(float.NaN), Vector128.Create(float.NegativeInfinity), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(float.NaN), Vector128.Create(float.NegativeInfinity), 1.0f, default(Vector4)},

                new object[] {Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), 0.1f, default(Vector4)},
                new object[] {Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), 0.5f, default(Vector4)},
                new object[] {Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), 0.0f, default(Vector4)},
                new object[] {Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), Vector128.Create(float.MaxValue, float.MinValue, float.NaN, 0), 1.0f, default(Vector4)},

            };

            foreach (object[] set in objs)
            {
                Vector4 v1 = ByValToSlowVector4((Vector128<float>)set[0]);
                Vector4 v2 = ByValToSlowVector4((Vector128<float>)set[1]);
                var weight = (float)set[2];

                set[3] = Vector4.Lerp(v1, v2, weight);
            }

            return objs;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public static void Lerp_Theory(Vector128<float> left, Vector128<float> right, float weight, Vector4 expected)
        {
            Vector128<float> result = Vector.Lerp(left, right, weight);

            Assert.True(AreEqual(expected, result), $"Expected {expected}, got {result}");
        }
    }
}