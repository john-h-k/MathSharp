using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Security.Cryptography.X509Certificates;
using OpenTK;
using static MathSharp.Vector;

#nullable disable

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    [RPlotExporter]
    public class SineWaveBenchmark
    {
        private const bool UseSinApprox = true;

        public const int SampleRate = 44100;
        private const float Tau = MathF.PI * 2;
        //The wave's frequency, A4.
        private const float Frequency = 440;

        public float[] AudioBufferNormal;
        public float[] AudioBufferVectorized;

        

        [GlobalSetup]
        public unsafe void Setup()
        {
            //We're going to generate a second of a pure sine.
            AudioBufferNormal = new float[SampleRate];
            AudioBufferVectorized = new float[SampleRate];

            Constants = (Vector128<float>*)Marshal.AllocHGlobal(sizeof(Vector128<float>) * 4);
            Constants[0] = Vector128.Create(0f, 1f, 2f, 3f);
            Constants[1] = Vector128.Create((float)SampleRate);
            Constants[2] = Vector128.Create(4f);
            Constants[3] = Vector128.Create(Tau * Frequency);
        }

        [Benchmark]
        public void SystemMathF()
        {
            var length = AudioBufferNormal.Length;
            for (int i = 0; i < length; i++)
                AudioBufferNormal[i] = MathF.Sin(Tau * Frequency * ((float)i / SampleRate));
        }

        private unsafe Vector128<float>* Constants;

        [Benchmark]
        public unsafe void MathSharp()
        {
            var length = SampleRate;
            var vecLength = SampleRate & ~3;

            var samplePoints = Constants[0];
            var sampleRate = Constants[1];
            var sampleIterator = Constants[2];
            var sine = Constants[3];

            var i = 0;

            //fixed (float* ptr = AudioBufferVectorized) // OLD
            fixed (float* ptr = &AudioBufferVectorized[0]) // NEW
            {
                while (i < vecLength)
                {
                    Vector128<float> vector = Divide(samplePoints, sampleRate);
                    vector = Multiply(vector, sine);

                    vector = Sin(vector);
                    vector.Store4D(&ptr[i]);

                    i += 4;
                    samplePoints = Add(samplePoints, sampleIterator);
                }

                if (i == length) return;

                var onesIterator = Vector128.CreateScalarUnsafe(1.0f);
                while (i < length)
                {
                    Vector128<float> vector = Divide(samplePoints, sampleRate);
                    vector = Multiply(vector, sine);

                    vector = Sin(vector);

                    Unsafe.Write(&ptr[i], vector.ToScalar());

                    i++;
                    samplePoints = Add(samplePoints, onesIterator);
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
