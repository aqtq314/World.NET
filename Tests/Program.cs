using System;
using System.Collections.Generic;
using System.Linq;
using World;

namespace Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var audioPath = @"vaiueo2d.wav";
            var audioOutPath = @"vaiueo2d-out.wav";

            var (x, fs, nbit) = AudioIO.WavRead(audioPath);
            var (f0, t) = Dio.Compute(x, fs);
            f0 = StoneMask.Compute(x, fs, t, f0);
            var fftSize = CheapTrick.GetFftSize(fs);
            var spfull = CheapTrick.Compute(x, fs, t, f0, fftSize: fftSize);
            var apfull = D4C.Compute(x, fs, t, f0, fftSize: fftSize);

            var ndim = 60;
            var sp = Codec.CodeSpectralEnvelope(spfull, fs, ndim);
            var ap = Codec.CodeAperiodicity(apfull, fs);

            Console.WriteLine($"{audioPath}:");
            Console.WriteLine($"    input samples count: {x.Length}");
            Console.WriteLine($"    sampling rate: {fs}");
            Console.WriteLine($"    bit rate: {nbit}");
            Console.WriteLine();
            Console.WriteLine($"    frame count: {f0.Length}");
            Console.WriteLine($"    fft size: {fftSize}");
            Console.WriteLine($"    spectrum width: {spfull.GetLength(1)}");
            Console.WriteLine();
            Console.WriteLine($"    mfcc width: {ndim}");
            Console.WriteLine($"    ap width: {ap.GetLength(1)}");
            Console.WriteLine();

            for (int i = 0; i < f0.Length; i++)
            {
                f0[i] *= 1.6789;
            }

            spfull = Codec.DecodeSpectralEnvelope(sp, fs, fftSize);
            apfull = Codec.DecodeAperiodicity(ap, fs, fftSize);
            var y = Synthesis.Compute(f0, spfull, apfull, fs);

            Console.WriteLine($"--> {audioOutPath}");
            AudioIO.WavWrite(y, fs, nbit, audioOutPath);
        }
    }
}
