using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PELib
{
    public static class Imagehlp
    {

        // http://doxygen.reactos.org/d3/d54/lib_2rtl_2image_8c_ac7d640524aeb6e88e72f05cfef7eefcc.html#ac7d640524aeb6e88e72f05cfef7eefcc
        public static ushort ChkSum(ulong sum, byte[] src) {
            
            for (int i=0; i<src.Length; i+=2) {
                // Treat buffer as array of 16-bit words
                ushort src_i = (ushort)((src[i + 1] << 8) | src[i]);

                // Sum up the current word
                sum += src_i;

                // Sum up everything above the low word as a carry
                sum = (sum & 0xFFFF) + (sum >> 16);
            }

            // Aply carry one more time and clamp to ushort
            return (ushort)((sum + (sum >> 16)) & 0xFFFF);
        }

        [DllImport("Imagehlp.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern IntPtr CheckSumMappedFile(byte[] BaseAddress, UInt32 FileLength, out UInt32 HeaderSum, out UInt32 CheckSum);
    }
}
