using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class D4C
    {
        [StructLayout(LayoutKind.Sequential)]
        struct D4COption
        {
            public double Threshold;
        }

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "D4C")]
        static extern void Compute([In] double[] x, int x_length, int fs,
            [In] double[] temporal_positions, [In] double[] f0, int f0_length,
            int fft_size, [In] ref D4COption option, [In, Out] IntPtr[] aperiodicity);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitializeD4COption")]
        static extern void InitOption([Out] out D4COption option);

        public static double[,] Compute(double[] x, int fs, double[] tpos, double[] f0,
            double threshold = 0.85, int? fftSize = null)
        {
            var fftSizeVal = fftSize ?? CheapTrick.GetFftSize(fs);
            InitOption(out var option);
            option.Threshold = threshold;

            var ap = new double[f0.Length, fftSizeVal / 2 + 1];
            Util.AsRowPointers(ap, apFramePtrs =>
                Compute(x, x.Length, fs, tpos, f0, f0.Length, fftSizeVal, ref option, apFramePtrs));

            return ap;
        }
    }
}
