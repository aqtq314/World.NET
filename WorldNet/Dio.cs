using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class Dio
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DioOption
        {
            public double F0Floor;
            public double F0Ceil;
            public double ChannelsInOctave;
            public double FramePeriod;  // msec
            public int Speed;  // (1, 2, ..., 12)
            public double AllowedRange;  // Threshold used for fixing the F0 contour.
        }

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Dio")]
        static extern void Compute([In] double[] x, int x_length, int fs, [In] ref DioOption option,
            [Out] double[] temporal_positions, [Out] double[] f0);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitializeDioOption")]
        static extern void InitOption([Out] out DioOption option);

        [DllImport(@"WORLD.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetSamplesForDIO")]
        public static extern int GetFrameCount(int fs, int xLength, double framePeriod = Constants.DefaultFramePeriod);

        public static (double[] f0, double[] tpos) Compute(double[] x, int fs,
            double f0Floor = Constants.DefaultF0Floor, double f0Ceil = Constants.DefaultF0Ceil, double channelsInOctave = 2,
            double framePeriod = Constants.DefaultFramePeriod, int speed = 1, double allowedRange = 0.1)
        {
            InitOption(out var option);
            option.F0Floor = f0Floor;
            option.F0Ceil = f0Ceil;
            option.ChannelsInOctave = channelsInOctave;
            option.FramePeriod = framePeriod;
            option.Speed = speed;
            option.AllowedRange = allowedRange;

            var f0Length = GetFrameCount(fs, x.Length, option.FramePeriod);
            var f0 = new double[f0Length];
            var tpos = new double[f0Length];
            Compute(x, x.Length, fs, ref option, tpos, f0);
            return (f0, tpos);
        }
    }
}
