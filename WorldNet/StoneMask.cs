using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class StoneMask
    {
        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "StoneMask")]
        static extern void Compute([In] double[] x, int x_length, int fs,
            [In] double[] temporal_positions, [In] double[] f0, int f0_length, [Out] double[] refined_f0);

        public static double[] Compute(double[] x, int fs, double[] tpos, double[] f0)
        {
            var outF0 = new double[f0.Length];
            Compute(x, x.Length, fs, tpos, f0, f0.Length, outF0);
            return outF0;
        }
    }
}
