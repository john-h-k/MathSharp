using System;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

using static MathSharp.Vector;

#nullable disable

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    [RPlotExporter]
    public class SawToothWaveBenchmark
    {
        private const int SampleRate = 44100;
        //The wave's frequency, A4.
        private const float Frequency = 440;

        private const int Size = 4960;

        public float[] AudioBufferNormal;
        public float[] AudioBufferVectorized;

        [GlobalSetup]
        public unsafe void Setup()
        {
            //We're going to generate a second of a pure sine.
            AudioBufferNormal = new float[Size];
            AudioBufferVectorized = new float[Size];

            Constants = (Vector128<float>*)Marshal.AllocHGlobal(sizeof(Vector128<float>) * 5);
            Constants[0] = Vector128.Create(0f, 1f, 2f, 3f);
            Constants[1] = Vector128.Create((float)SampleRate);
            Constants[2] = Vector128.Create(Frequency);
            Constants[3] = Vector128.Create(2f);
            Constants[4] = Vector128.Create(4f);

            Constants256 = (Vector256<float>*)Marshal.AllocHGlobal(sizeof(Vector256<float>) * 5);
            Constants256[0] = Vector256.Create(0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f);
            Constants256[1] = Vector256.Create((float)SampleRate);
            Constants256[2] = Vector256.Create(Frequency);
            Constants256[3] = Vector256.Create(2f);
            Constants256[4] = Vector256.Create(8f);
        }

        [Benchmark]
        public void NonVectorised()
        { 
            for (int i = 0; i < AudioBufferNormal.Length; i++)
                AudioBufferNormal[i] = ((Frequency * ((float)i / SampleRate)) % 2) - 1;
        }

        [Benchmark]
        public void NonVectorised_FastRemainder()
        {
            for (int i = 0; i < AudioBufferNormal.Length; i++)
            {
                var temp = Frequency * ((float) i / SampleRate);
                AudioBufferNormal[i] = FRemainder(temp, 2) - 1;
            }
        }

        private static float FRemainder(float x, float y)
        {
            float n = RoundDown(x / y);

            var z = x - n * y;

            return z;
        }

        private static float RoundDown(float x)
        {
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNegativeInfinity(Vector128.CreateScalarUnsafe(x)).ToScalar();
            }

            return MathF.Round(x, MidpointRounding.ToNegativeInfinity);
        }

        private unsafe Vector128<float>* Constants;
        private unsafe Vector256<float>* Constants256;

        [Benchmark]
        public unsafe void MathSharp()
        {
            var length = Size;
            var vecLength = Size & ~3;

            var time = Constants[0];
            var sampleRate = Constants[1];
            var frequency = Constants[2];
            var two = Constants[3];
            var sampleIterator = Constants[4];

            var i = 0;

            fixed (float* ptr = AudioBufferVectorized)
            {
                while (i < vecLength)
                {
                    Vector128<float> vector = time;
                    vector = Divide(vector, sampleRate);
                    vector = Multiply(frequency, vector);
                    vector = Remainder(vector, two);
                    vector = Subtract(vector, SingleConstants.One);

                    vector.Store(out ptr[i]);

                    i += 4;
                    time = Add(time, sampleIterator);
                }

                while (i < length)
                {
                    ptr[i] = (Frequency * ((float) i / SampleRate) % 2 - 1);
                    i++;
                }
            }
        }

        [GlobalCleanup]
        public unsafe void Cleanup()
        {
            Marshal.FreeHGlobal((IntPtr)Constants);
        }
    }
}