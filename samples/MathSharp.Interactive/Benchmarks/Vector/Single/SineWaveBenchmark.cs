using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.Intrinsics;
using static MathSharp.Vector;

#nullable disable

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    public class SineWaveBenchmark
    {
        private const int SampleRate = 44100;
        private const float Tau = MathF.PI * 2;
        //The wave's frequency, A4.
        private const float Frequency = 440;

        private float[] _audioBufferNormal;
        private float[] _audioBufferVectorized;

        [GlobalSetup]
        public void Setup()
        {
            //We're going to generate a second of a pure sine.
            _audioBufferNormal = new float[SampleRate];
            _audioBufferVectorized = new float[SampleRate];
        }

        [Benchmark]
        public void Normal()
        {
            var length = _audioBufferNormal.Length;
            for (int i = 0; i < length; i++)
                _audioBufferNormal[i] = MathF.Sin(Tau * Frequency * ((float)i / SampleRate));
        }

        [Benchmark]
        public unsafe void MathSharp()
        {
            var length = _audioBufferVectorized.Length;
            var vecLength = _audioBufferVectorized.Length & ~3;

            var samplePoints = Vector128.Create((float)0, 1, 2, 3);
            var sampleRate = Vector128.Create((float)SampleRate);
            var sampleIterator = Vector128.Create((float)4);
            var sine = Vector128.Create(Tau * Frequency);
            var i = 0;

            fixed (float* ptr = _audioBufferVectorized)
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
            }
            while (i < length)
            {
                _audioBufferVectorized[i] = MathF.Sin(Tau * Frequency * ((float)i / SampleRate));
            }
        }
        [GlobalCleanup]
        public void Cleanup()
        { }
    }
}
