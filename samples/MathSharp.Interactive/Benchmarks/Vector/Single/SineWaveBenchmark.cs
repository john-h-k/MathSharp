using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using OpenTK;
using static MathSharp.Vector;

#nullable disable

namespace MathSharp.Interactive.Benchmarks.Vector.Single
{
    [RPlotExporter]
    public class SineWaveBenchmark
    {
        private const bool UseSinApprox = true;

        private const int SampleRate = 44100;
        private const float Tau = MathF.PI * 2;
        //The wave's frequency, A4.
        private const float Frequency = 440;

        private float[] _audioBufferNormal;
        private float[] _audioBufferVectorized;

        [GlobalSetup]
        public unsafe void Setup()
        {
            //We're going to generate a second of a pure sine.
            _audioBufferNormal = new float[SampleRate];
            _audioBufferVectorized = new float[SampleRate];

            Constants = (Vector128<float>*)Marshal.AllocHGlobal(sizeof(Vector128<float>) * 4);
            Constants[0] = Vector128.Create(0f, 1f, 2f, 3f);
            Constants[1] = Vector128.Create((float)SampleRate);
            Constants[2] = Vector128.Create(4f);
            Constants[3] = Vector128.Create(Tau * Frequency);
        }

        [Benchmark]
        public void SystemMathF()
        {
            var length = _audioBufferNormal.Length;
            for (int i = 0; i < length; i++)
                _audioBufferNormal[i] = MathF.Sin(Tau * Frequency * ((float)i / SampleRate));
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
                i++;
            }
        }

        [GlobalCleanup]
        public unsafe void Cleanup()
        {
            Marshal.FreeHGlobal((IntPtr)Constants);
        }
    }
}
