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
            var sp = CheapTrick.Compute(x, fs, t, f0, fftSize: fftSize);
            var ap = D4C.Compute(x, fs, t, f0, fftSize: fftSize);

            var ndim = 60;
            var mgc = Codec.CodeSpectralEnvelope(sp, fs, ndim);
            var bap = Codec.CodeAperiodicity(ap, fs);

            Console.WriteLine($"{audioPath}:");
            Console.WriteLine($"    input samples count: {x.Length}");
            Console.WriteLine($"    sampling rate: {fs}");
            Console.WriteLine($"    bit rate: {nbit}");
            Console.WriteLine();
            Console.WriteLine($"    frame count: {f0.Length}");
            Console.WriteLine($"    fft size: {fftSize}");
            Console.WriteLine($"    sp width: {sp.GetLength(1)}");
            Console.WriteLine();
            Console.WriteLine($"    mgc width: {ndim}");
            Console.WriteLine($"    bap width: {bap.GetLength(1)}");
            Console.WriteLine();

            for (int i = 0; i < f0.Length; i++)
            {
                f0[i] *= 1.6789;
            }

            sp = Codec.DecodeSpectralEnvelope(mgc, fs, fftSize);
            ap = Codec.DecodeAperiodicity(bap, fs, fftSize);
            var y = Synthesis.Compute(f0, sp, ap, fs);

            Console.WriteLine($"--> {audioOutPath}");
            AudioIO.WavWrite(y, fs, nbit, audioOutPath);
        }
    }
}
