using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;

namespace MathSharp.UnitTests.VectorTests.VectorSingle
{
    public class DataSets
    {
        public class Single
        {
            public static float[] SpecialValues =
            {
                0f, -0f, float.NaN, float.NegativeInfinity, float.PositiveInfinity, float.MinValue, float.MaxValue,
                float.Epsilon
            };

            public static float[] NormalValues =
            {
                1f, -1f, 3.14159265f, 2.71828183f, 1.41421356f, 11111111111111111111.23423f, 0.000000000042342f, 128932183128342.1f
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
                foreach (var value in SpecialValues)
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

        public class Double
        {
            public static double[] SpecialValues =
            {
                0d, -0d, double.NaN, double.NegativeInfinity, double.PositiveInfinity, double.MinValue, double.MaxValue,
                double.Epsilon
            };

            public static double[] NormalValues =
            {
                1d, -1d, 3.14159265d, 2.71828183d, 1.41421356d
            };

            public static double[] RoundingValues =
            {
                0d, 1d, 5d, 100000d, 1000000000d,
                -0d, -1d, -5d, -100000d, -1000000000d,
                0.0001d, 1.0001d, 5.0001d, 100000.0001d, 1000000000.0001d,
                -0.0001d, -1.0001d, -5.0001d, -100000.0001d, -1000000000.0001d,
                0.5d, 1.5d, 5.5d, 100000.5d, 1000000000.5d,
                -0.5d, -1.5d, -5.5d, -100000.5d, -1000000000.5d,
                -0.50001d, -1.50001d, -5.50001d, -100000.50001d, -1000000000.50001d,
                -0.49999d, -1.49999d, -5.49999d, -100000.49999d, -1000000000.49999d,
                -0.99999d, -1.99999d, -5.99999d, -100000.99999d, -1000000000.99999d
            };

            public static IEnumerable<object[]> CreateRoundingDataSet(Func<double, double> correctTransformation)
            {
                foreach (var value in SpecialValues)
                {
                    yield return CreateUnarySingleDataSet(value);
                }

                foreach (var value in RoundingValues)
                {
                    yield return CreateUnarySingleDataSet(value);
                }

                object[] CreateUnarySingleDataSet(double f) => new object[] { Vector256.Create(f), Vector256.Create(correctTransformation(f)) };
            }

            public static IEnumerable<object[]> CreateUnaryDataSet(Func<double, double> correctTransformation)
            {
                foreach (var value in SpecialValues)
                {
                    yield return CreateUnarySingleDataSet(value);
                }

                foreach (var value in NormalValues)
                {
                    yield return CreateUnarySingleDataSet(value);
                }

                object[] CreateUnarySingleDataSet(double f) => new object[] { Vector256.Create(f), Vector256.Create(correctTransformation(f)) };
            }

            public static IEnumerable<object[]> CreateBinaryDataSet(Func<double, double, double> correctTransformation)
            {
                foreach (var value1 in SpecialValues)
                {
                    foreach (var value2 in NormalValues)
                    {
                        yield return CreateBinarySingleDataSet(value1, value2);
                        yield return CreateBinarySingleDataSet(value2, value2);
                    }
                }



                object[] CreateBinarySingleDataSet(double f1, double f2) => new object[] { Vector256.Create(f1), Vector256.Create(f2), Vector256.Create(correctTransformation(f1, f2)) };
            }
        }
    }
}