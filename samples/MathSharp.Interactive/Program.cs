using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using static MathSharp.Vector;

namespace MathSharp.Interactive
{
    internal class Program
    {
        private static void Main()
        {
            var vector = Vector128.Create(0u, 1u, 2u, 3u).AsSingle();
            var one = Vector128.Create(1u);

            for (uint i = 0; i < uint.MaxValue; i += 4)
            {
                var sin = Sin(vector);
                var sinFma = SinBenchmark.SinFma(vector);

                var comp = CompareEqual(sin, sinFma);

                if (comp.AnyFalse())
                {
                    Console.WriteLine($"With value {vector}: Sin = {sin}, SinFma = {sinFma}");
                }

                vector = Add(vector.AsUInt32(), one).AsSingle();
            }

            Console.WriteLine("Done");
        }
    }

    public class SinBenchmark
    {
        public static object[] Data => new object[] { Vector128.Create(1f) };

        private static readonly Vector128<float> SinCoefficient0 = Vector128.Create(-0.16666667f, +0.0083333310f, -0.00019840874f, +2.7525562e-06f);
        private static readonly Vector128<float> SinCoefficient1 = Vector128.Create(-2.3889859e-08f, -0.16665852f, +0.0083139502f, -0.00018524670f);

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public Vector128<float> SinNormal(Vector128<float> vector)
        {
            Vector128<float> vec = Mod2Pi(vector);

            Vector128<float> sign = ExtractSign(vec);
            Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

            Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

            Vector128<float> neg = Subtract(tmp, vec);

            Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

            Vector128<float> select0 = SelectWhereTrue(vec, comp);
            Vector128<float> select1 = SelectWhereFalse(neg, comp);

            vec = Or(select0, select1);

            Vector128<float> vectorSquared = Multiply(vec, vec);

            // Polynomial approx
            Vector128<float> sc1 = SinCoefficient1;
            Vector128<float> constants = PermuteWithX(sc1);
            Vector128<float> result = Multiply(constants, vectorSquared);

            Vector128<float> sc0 = SinCoefficient0;
            constants = PermuteWithW(sc0);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);

            constants = PermuteWithZ(sc0);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);

            constants = PermuteWithY(sc0);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);

            constants = PermuteWithX(sc0);
            result = Add(result, constants);
            result = Multiply(result, vectorSquared);
            result = Add(result, SingleConstants.One);
            result = Multiply(result, vec);

            return result;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public Vector128<float> SinRearranged(Vector128<float> vector)
        {
            Vector128<float> vec = Mod2Pi(vector);

            Vector128<float> sign = ExtractSign(vec);
            Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

            Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

            Vector128<float> neg = Subtract(tmp, vec);

            Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

            Vector128<float> select0 = SelectWhereTrue(vec, comp);
            Vector128<float> select1 = SelectWhereFalse(neg, comp);

            vec = Or(select0, select1);

            Vector128<float> vectorSquared = Multiply(vec, vec);

            // Polynomial approx
            Vector128<float> sc0 = SinCoefficient0;
            Vector128<float> sc1 = SinCoefficient1;

            Vector128<float> constants = PermuteWithX(sc1);
            Vector128<float> result = Multiply(constants, vectorSquared);
            result = Add(result, PermuteWithW(sc0));

            constants = PermuteWithZ(sc0);
            result = Multiply(result, vectorSquared);
            result = Add(result, constants);

            constants = PermuteWithY(sc0);
            result = Multiply(result, vectorSquared);
            result = Add(result, constants);

            constants = PermuteWithX(sc0);
            result = Multiply(result, vectorSquared);
            result = Add(result, constants);

            result = Multiply(result, vectorSquared);
            result = Add(result, SingleConstants.One);

            result = Multiply(result, vec);

            return result;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public static Vector128<float> SinFma(Vector128<float> vector)
        {
            Vector128<float> vec = Mod2Pi(vector);

            Vector128<float> sign = ExtractSign(vec);
            Vector128<float> tmp = Or(SingleConstants.Pi, sign); // Pi with the sign from vector

            Vector128<float> abs = AndNot(sign, vec); // Gets the absolute of vector

            Vector128<float> neg = Subtract(tmp, vec);

            Vector128<float> comp = CompareLessThanOrEqual(abs, SingleConstants.PiDiv2);

            Vector128<float> select0 = SelectWhereTrue(vec, comp);
            Vector128<float> select1 = SelectWhereFalse(neg, comp);

            vec = Or(select0, select1);

            Vector128<float> vectorSquared = Multiply(vec, vec);

            // Polynomial approx
            Vector128<float> sc0 = SinCoefficient0;
            Vector128<float> sc1 = SinCoefficient1;

            Vector128<float> constants = PermuteWithX(sc1);

            Vector128<float> result = FusedMultiplyAdd(constants, vectorSquared, PermuteWithW(sc0));

            constants = PermuteWithZ(sc0);
            result = FusedMultiplyAdd(result, vectorSquared, constants);

            constants = PermuteWithY(sc0);
            result = FusedMultiplyAdd(result, vectorSquared, constants);

            constants = PermuteWithX(sc0);
            result = FusedMultiplyAdd(result, vectorSquared, constants);

            result = FusedMultiplyAdd(result, vectorSquared, SingleConstants.One);

            result = Multiply(result, vec);

            return result;
        }
    }
}