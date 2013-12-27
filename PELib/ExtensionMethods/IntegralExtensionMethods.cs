using System;

namespace PELib.ExtensionMethods
{
    public static class IntegralExtensionMethods
    {
        public static bool IsPow2(this long x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        /// <summary>Returns true if x is &gt;= lower and &lt; upper.</summary>
        /// <param name="x">Value to compare</param>
        /// <param name="lower">Inclusive lower range limit</param>
        /// <param name="upper">Exclusive upper range limit</param>
        /// <returns></returns>
        public static bool InRange(this long x, long lower, long upper)
        {
            return (x >= lower) && (x < upper);
        }

        public static long Align(this long val, long align)
        {
            if (!align.IsPow2())
                throw new ArgumentException("align must be a power of 2.");

            // 0x1000 - 1 = 0x0FFF

            if ((val & (align - 1)) == 0) return val;     // already aligned

            return (val & ~(align - 1)) + align;
        }
    }
}