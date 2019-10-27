using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;

namespace MathSharp.UnitTests.VectorTests.VectorSingle
{
    public class DataSets
    {
        public enum FpType { Single, Double }

        public static float[] SpecialValues =
        {
            0f, -0f, float.NaN, float.NegativeInfinity, float.PositiveInfinity, float.MinValue, float.MaxValue,
            float.Epsilon
        };

        public static float[] NormalValues =
        {
            1f, -1f, 3.14159265f, 2.71828183f, 1.41421356f
        };

        public static float[] RoundingValues =
        {
            0f, 1f, 5f, 100000f, 1000000000f,
            -0f, -1f, -5f, -100000f, -1000000000f,
            0.0001f, 1.0001f, 5.0001f, 100000.0001f, 1000000000.0001f,
            -0.0001f, -1.0001f, -5.0001f, -100000.0001f, -1000000000.0001f,
            0.5f, 1.5f, 5.5f, 100000.5f, 1000000000.5f,
            -0.5f, -1.5f, -5.5f, -100000.5f, -1000000000.5f,
            -0.50001f, -1.50001f, -5.50001f, -100000.50001f, -1000000000.50001f,
            -0.49999f, -1.49999f, -5.49999f, -100000.49999f, -1000000000.49999f,
            -0.99999f, -1.99999f, -5.99999f, -100000.99999f, -1000000000.99999f
        };

        public static IEnumerable<object[]> CreateRoundingDataSet(Func<float, float> correctTransformation)
        {
            foreach(var value in SpecialValues)
            {
                yield return CreateUnarySingleDataSet(value);
            }

            foreach (var value in RoundingValues)
            {
                yield return CreateUnarySingleDataSet(value);
            }

            object[] CreateUnarySingleDataSet(float f) => new object[] { Vector128.Create(f), Vector128.Create(correctTransformation(f)) };
        }
        
        public static IEnumerable<object[]> CreateUnaryDataSet(Func<float, float> correctTransformation)
        {
            foreach (var value in SpecialValues)
            {
                yield return CreateUnarySingleDataSet(value);
            }

            foreach (var value in NormalValues)
            {
                yield return CreateUnarySingleDataSet(value);
            }

            object[] CreateUnarySingleDataSet(float f) => new object[] { Vector128.Create(f), Vector128.Create(correctTransformation(f)) };
        }

        public static IEnumerable<object[]> CreateBinaryDataSet(Func<float, float, float> correctTransformation)
        {
            foreach (var value1 in SpecialValues)
            {
                foreach (var value2 in NormalValues)
                {
                    yield return CreateBinarySingleDataSet(value1, value2);
                    yield return CreateBinarySingleDataSet(value2, value2);
                }
            }

            

            object[] CreateBinarySingleDataSet(float f1, float f2) => new object[] { Vector128.Create(f1), Vector128.Create(f2), Vector128.Create(correctTransformation(f1, f2)) };
        }
    }
}