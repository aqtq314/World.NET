using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class Synthesis
    {
        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Synthesis")]
        static extern void Compute([In] double[] f0, int f0_length,
            [In] IntPtr[] spectrogram, [In] IntPtr[] aperiodicity,
            int fft_size, double frame_period, int fs, int y_length, [Out] double[] y);

        public static double[] Compute(double[] f0, double[,] sp, double[,] ap, int fs,
            double framePeriod = Constants.DefaultFramePeriod)
        {
            var fftSize = (sp.GetLength(1) - 1) * 2;

            var yLength = (int)(f0.Length * framePeriod * fs / 1000);
            var y = new double[yLength];
            Util.AsRowPointers(sp, spFramePtrs =>
            Util.AsRowPointers(ap, apFramePtrs =>
                Compute(f0, f0.Length, spFramePtrs, apFramePtrs, fftSize, framePeriod, fs, yLength, y)));

            return y;
        }
    }
}
