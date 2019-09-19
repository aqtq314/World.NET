using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class Harvest
    {
        [StructLayout(LayoutKind.Sequential)]
        struct HarvestOption
        {
            public double F0Floor;
            public double F0Ceil;
            public double FramePeriod;
        }

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Harvest")]
        static extern void Compute([In] double[] x, int x_length, int fs, [In] ref HarvestOption option,
            [Out] double[] temporal_positions, [Out] double[] f0);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitializeHarvestOption")]
        static extern void InitOption([Out] out HarvestOption option);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetSamplesForHarvest")]
        public static extern int GetFrameCount(int fs, int xLength, double framePeriod = Constants.DefaultFramePeriod);

        public static (double[] f0, double[] tpos) Compute(double[] x, int fs,
            double f0Floor = Constants.DefaultF0Floor, double f0Ceil = Constants.DefaultF0Ceil,
            double framePeriod = Constants.DefaultFramePeriod)
        {
            InitOption(out var option);
            option.F0Floor = f0Floor;
            option.F0Ceil = f0Ceil;
            option.FramePeriod = framePeriod;

            var f0Length = GetFrameCount(fs, x.Length, option.FramePeriod);
            var f0 = new double[f0Length];
            var tpos = new double[f0Length];
            Compute(x, x.Length, fs, ref option, tpos, f0);
            return (f0, tpos);
        }
    }
}
