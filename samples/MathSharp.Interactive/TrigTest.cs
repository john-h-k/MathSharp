using System;
using System.Runtime.Intrinsics;

using static MathSharp.Vector;

namespace MathSharp.Interactive
{
    public class TrigTest
    {
        public static void Test(float f)
            => Test(f, true, true, true, true);

        public static void Test(float f, bool doSin = false, bool doCos = false, bool doTan = false, bool doSinCos = false)
        {
            Console.WriteLine($"For value {f}: ");

            var v = Vector128.Create(f);

            if (doSin)
            {
                Console.WriteLine($"MathF Sin:             {MathF.Sin(f)}");
                Console.WriteLine($"MathSharp Sin:         {Sin(v).ToScalar()}");
                Console.WriteLine($"MathSharp SinApprox:   {SinApprox(v).ToScalar()}");
            }

            if (doCos)
            {
                Console.WriteLine($"MathF Cos:             {MathF.Cos(f)}");
                Console.WriteLine($"MathSharp Cos:         {Cos(v).ToScalar()}");
                Console.WriteLine($"MathSharp CosApprox:   {CosApprox(v).ToScalar()}");
            }

            if (doTan)
            {
                Console.WriteLine($"MathF Tan:             {MathF.Tan(f)}");
                Console.WriteLine($"MathSharp Tan:         {Tan(v).ToScalar()}");
                Console.WriteLine($"MathSharp TanApprox:   {TanApprox(v).ToScalar()}");
            }

            if (doSinCos)
            {
                SinCos(v, out Vector128<float> sin, out Vector128<float> cos);
                SinCosApprox(v, out Vector128<float> sinEst, out Vector128<float> cosEst);

                Console.WriteLine($"MathF SinCos:             {MathF.Sin(f)}, {MathF.Cos(f)}");
                Console.WriteLine($"MathSharp SinCos:         {sin.ToScalar()}, {cos.ToScalar()}");
                Console.WriteLine($"MathSharp SinCosApprox:   {sinEst.ToScalar()}, {cosEst.ToScalar()}");
            }

            Console.WriteLine("\n");
        }

        //public static void Test(double f)
        //    => Test(f, true, true, true, true);

        //public static void Test(double f, bool doSin = false, bool doCos = false, bool doTan = false, bool doSinCos = false)
        //{
        //    Console.WriteLine($"For value {f}: ");

        //    var v = Vector256.Create(f);

        //    if (doSin)
        //    {
        //        Console.WriteLine($"Math Sin:             {Math.Sin(f)}");
        //        Console.WriteLine($"MathSharp Sin:         {Sin(v).ToScalar()}");
        //        Console.WriteLine($"MathSharp SinApprox:   {SinApprox(v).ToScalar()}");
        //    }

        //    if (doCos)
        //    {
        //        Console.WriteLine($"Math Cos:             {Math.Cos(f)}");
        //        Console.WriteLine($"MathSharp Cos:         {Cos(v).ToScalar()}");
        //        Console.WriteLine($"MathSharp CosApprox:   {CosApprox(v).ToScalar()}");
        //    }

        //    if (doTan)
        //    {
        //        Console.WriteLine($"Math Tan:             {Math.Tan(f)}");
        //        Console.WriteLine($"MathSharp Tan:         {Tan(v).ToScalar()}");
        //        Console.WriteLine($"MathSharp TanApprox:   {TanApprox(v).ToScalar()}");
        //    }

        //    if (doSinCos)
        //    {
        //        SinCos(v, out Vector256<double> sin, out Vector256<double> cos);
        //        SinCosApprox(v, out Vector256<double> sinEst, out Vector256<double> cosEst);

        //        Console.WriteLine($"Math SinCos:             {Math.Sin(f)}, {Math.Cos(f)}");
        //        Console.WriteLine($"MathSharp SinCos:         {sin.ToScalar()}, {cos.ToScalar()}");
        //        Console.WriteLine($"MathSharp SinCosApprox:   {sinEst.ToScalar()}, {cosEst.ToScalar()}");
        //    }

        //    Console.WriteLine("\n");
        //}
    }
}