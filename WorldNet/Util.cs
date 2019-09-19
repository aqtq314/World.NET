using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace World
{
    public static class Constants
    {
        public const double DefaultFramePeriod = 5;
        public const double DefaultF0Floor = 71;
        public const double DefaultF0Ceil = 800;
    }

    public static class Util
    {
        public static unsafe void AsRowPointers(double[,] arr, Action<IntPtr[]> cont)
        {
            fixed (double* arrPtr = arr)
            {
                var arrRowPtrs = new IntPtr[arr.GetLength(0)];
                for (int i = 0; i < arr.GetLength(0); i++)
                    arrRowPtrs[i] = (IntPtr)(&arrPtr[i * arr.GetLength(1)]);

                cont(arrRowPtrs);
            }
        }
    }
}
