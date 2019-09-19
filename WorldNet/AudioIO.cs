using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class AudioIO
    {
        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAudioLength([In] string filename);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wavread")]
        static extern void WavRead([In] string filename,
            [Out] out int fs, [Out] out int nbit, [Out] double[] x);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "wavwrite")]
        static extern void WavWrite([In] double[] x, int x_length, int fs, int nbit,
            [In] string filename);

        public static (double[] x, int fs, int nbit) WavRead(string filename)
        {
            var audioLength = GetAudioLength(filename);
            var x = new double[audioLength];
            WavRead(filename, out var fs, out var nbit, x);
            return (x, fs, nbit);
        }

        public static void WavWrite(double[] x, int fs, int nbit, string filename)
        {
            WavWrite(x, x.Length, fs, nbit, filename);
        }
    }
}
