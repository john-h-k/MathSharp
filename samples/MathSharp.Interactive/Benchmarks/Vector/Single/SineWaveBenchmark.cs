using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.Intrinsics;
using static MathSharp.Vector;

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    public class SineWaveBenchmark
    {
        private const int SampleRate = 44100;
        private const float Tau = MathF.PI * 2;
        //The wave's frequency, A4.
        private const float Frequency = 440;

        private float[] audioBufferNormal;
        private float[] audioBufferVectorized;

        [GlobalSetup]
        public void Setup()
        {
            //We're going to generate a second of a pure sine.
            audioBufferNormal = new float[SampleRate];
            audioBufferVectorized = new float[SampleRate];
        }

        [Benchmark]
        public void Normal()
        {
            var length = audioBufferNormal.Length;
            for (int i = 0; i < length; i++)
                audioBufferNormal[i] = MathF.Sin(Tau * Frequency * ((float)i / SampleRate));
        }

        [Benchmark]
        public unsafe void MathSharp()
        {
            var length = audioBufferVectorized.Length;
            var vecLength = audioBufferVectorized.Length & ~3;

            var samplePoints = Vector128.Create((float)0, 1, 2, 3);
            var sampleRate = Vector128.Create((float)SampleRate);
            var sampleIterator = Vector128.Create((float)4);
            var sine = Vector128.Create(Tau * Frequency);
            var i = 0;

            fixed (float* ptr = audioBufferVectorized)
            {
                while (i < vecLength)
                {
                    HwVector4S vector = Divide(samplePoints, sampleRate);
                    vector = Multiply(vector, sine);
                    vector = Sin(vector);

                    vector.Store4D(&ptr[i]);

                    i += 4;
                    samplePoints = Add(samplePoints, sampleIterator);
                }
            }
            while (i < length)
            {
                audioBufferVectorized[i] = MathF.Sin(Tau * Frequency * ((float)i / SampleRate));
            }
        }
        [GlobalCleanup]
        public void Cleanup()
        { }
    }
}
