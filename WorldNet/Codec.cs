using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class Codec
    {
        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNumberOfAperiodicities(int fs);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void CodeAperiodicity([In] IntPtr[] aperiodicity, int f0_length,
            int fs, int fft_size, [In, Out] IntPtr[] coded_aperiodicity);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void DecodeAperiodicity([In] IntPtr[] coded_aperiodicity,
            int f0_length, int fs, int fft_size, [In, Out] IntPtr[] aperiodicity);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void CodeSpectralEnvelope([In] IntPtr[] spectrogram, int f0_length,
            int fs, int fft_size, int number_of_dimensions, [In, Out] IntPtr[] coded_spectral_envelope);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void DecodeSpectralEnvelope([In] IntPtr[] coded_spectral_envelope,
            int f0_length, int fs, int fft_size, int number_of_dimensions, [In, Out] IntPtr[] spectrogram);

        public static double[,] CodeAperiodicity(double[,] ap, int fs)
        {
            var fftSize = (ap.GetLength(1) - 1) * 2;
            var numCodedAp = GetNumberOfAperiodicities(fs);

            var codedAp = new double[ap.GetLength(0), numCodedAp];
            Util.AsRowPointers(ap, apFramePtrs =>
            Util.AsRowPointers(codedAp, codedApFramePtrs =>
                CodeAperiodicity(apFramePtrs, ap.GetLength(0), fs, fftSize, codedApFramePtrs)));

            return codedAp;
        }

        public static double[,] DecodeAperiodicity(double[,] codedAp, int fs, int fftSize)
        {
            var ap = new double[codedAp.GetLength(0), fftSize / 2 + 1];
            Util.AsRowPointers(codedAp, codedApFramePtrs =>
            Util.AsRowPointers(ap, apFramePtrs =>
                DecodeAperiodicity(codedApFramePtrs, codedAp.GetLength(0), fs, fftSize, apFramePtrs)));

            return ap;
        }

        public static double[,] CodeSpectralEnvelope(double[,] sp, int fs, int numDimensions)
        {
            var fftSize = (sp.GetLength(1) - 1) * 2;

            var codedSp = new double[sp.GetLength(0), numDimensions];
            Util.AsRowPointers(sp, spFramePtrs =>
            Util.AsRowPointers(codedSp, codedSpFramePtrs =>
                CodeSpectralEnvelope(spFramePtrs, sp.GetLength(0), fs, fftSize, numDimensions, codedSpFramePtrs)));

            return codedSp;
        }

        public static double[,] DecodeSpectralEnvelope(double[,] codedSp, int fs, int fftSize)
        {
            var sp = new double[codedSp.GetLength(0), fftSize / 2 + 1];
            Util.AsRowPointers(codedSp, codedSpFramePtrs =>
            Util.AsRowPointers(sp, spFramePtrs =>
                DecodeSpectralEnvelope(codedSpFramePtrs, codedSp.GetLength(0), fs, fftSize, codedSp.GetLength(1), spFramePtrs)));

            return sp;
        }
    }
}
