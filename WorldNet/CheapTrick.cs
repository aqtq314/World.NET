using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class CheapTrick
    {
        [StructLayout(LayoutKind.Sequential)]
        struct CheapTrickOption
        {
            public double Q1;
            public double F0Floor;
            public int FftSize;
        }

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CheapTrick")]
        static extern void Compute([In] double[] x, int x_length, int fs,
            [In] double[] temporal_positions, [In] double[] f0, int f0_length,
            [In] ref CheapTrickOption option, [In, Out] IntPtr[] spectrogram);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitializeCheapTrickOption")]
        static extern void InitOption(int fs, [Out] out CheapTrickOption option);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetFFTSizeForCheapTrick")]
        static extern int GetFftSize(int fs, [In] ref CheapTrickOption option);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetF0FloorForCheapTrick")]
        public static extern double GetF0Floor(int fs, int fftSize);

        public static double[,] Compute(double[] x, int fs, double[] tpos, double[] f0,
            double q1 = -0.15, double f0Floor = Constants.DefaultF0Floor, int? fftSize = null)
        {
            InitOption(fs, out var option);
            option.Q1 = q1;
            option.F0Floor = f0Floor;
            option.FftSize = fftSize ?? GetFftSize(fs, ref option);

            var sp = new double[f0.Length, option.FftSize / 2 + 1];
            Util.AsRowPointers(sp, spFramePtrs =>
                Compute(x, x.Length, fs, tpos, f0, f0.Length, ref option, spFramePtrs));

            return sp;
        }

        public static int GetFftSize(int fs, double f0Floor = Constants.DefaultF0Floor)
        {
            InitOption(fs, out var option);
            option.F0Floor = f0Floor;
            return GetFftSize(fs, ref option);
        }
    }
}
