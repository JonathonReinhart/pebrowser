using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PELib
{
    public class FileHeader
    {
        public IMAGE_FILE_MACHINE Machine { get; set; }
        public UInt16 NumberOfSections { get; set; }
        public UInt32 TimeDateStamp { get; set; }
        public UInt32 PointerToSymbolTable { get; set; }
        public UInt32 NumberOfSymbols { get; set; }
        public UInt16 SizeOfOptionalHeader { get; set; }
        public UInt16 Characteristics { get; set; }

        public FileHeader(Stream stream)
        {
            var br = new BinaryReader(stream);

            Machine = (IMAGE_FILE_MACHINE)br.ReadUInt16();
            NumberOfSections = br.ReadUInt16();
            TimeDateStamp = br.ReadUInt32();
            PointerToSymbolTable = br.ReadUInt32();
            NumberOfSymbols = br.ReadUInt32();
            SizeOfOptionalHeader = br.ReadUInt16();
            Characteristics = br.ReadUInt16();
        }

        /// <summary>
        /// Gets the timestamp as a DateTime struct.
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                // Timestamp is a date offset from 1970
                var returnValue = new DateTime(1970, 1, 1, 0, 0, 0);

                // Add in the number of seconds since 1970/1/1
                returnValue = returnValue.AddSeconds(TimeDateStamp);
                // Adjust to local timezone
                returnValue += TimeZone.CurrentTimeZone.GetUtcOffset(returnValue);

                return returnValue;
            }
        }

    }





    public enum IMAGE_FILE_MACHINE : ushort
    {
        /// <summary>The contents of this field are assumed to be applicable to any machine type</summary>
        IMAGE_FILE_MACHINE_UNKNOWN = 0x0,

        /// <summary>Matsushita AM33</summary>
        IMAGE_FILE_MACHINE_AM33 = 0x1d3,

        /// <summary>x64</summary>
        IMAGE_FILE_MACHINE_AMD64 = 0x8664,

        /// <summary>ARM little endian</summary>
        IMAGE_FILE_MACHINE_ARM = 0x1c0,

        /// <summary>ARMv7 (or higher) Thumb mode only</summary>
        IMAGE_FILE_MACHINE_ARMV7 = 0x1c4,

        /// <summary>EFI byte code</summary>
        IMAGE_FILE_MACHINE_EBC = 0xebc,

        /// <summary>Intel 386 or later processors and compatible processors</summary>
        IMAGE_FILE_MACHINE_I386 = 0x14c,

        /// <summary>Intel Itanium processor family</summary>
        IMAGE_FILE_MACHINE_IA64 = 0x200,

        /// <summary>Mitsubishi M32R little endian</summary>
        IMAGE_FILE_MACHINE_M32R = 0x9041,

        /// <summary>MIPS16</summary>
        IMAGE_FILE_MACHINE_MIPS16 = 0x266,

        /// <summary>MIPS with FPU</summary>
        IMAGE_FILE_MACHINE_MIPSFPU = 0x366,

        /// <summary>MIPS16 with FPU</summary>
        IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,

        /// <summary>Power PC little endian</summary>
        IMAGE_FILE_MACHINE_POWERPC = 0x1f0,

        /// <summary>Power PC with floating point support</summary>
        IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,

        /// <summary>MIPS little endian</summary>
        IMAGE_FILE_MACHINE_R4000 = 0x166,

        /// <summary>Hitachi SH3</summary>
        IMAGE_FILE_MACHINE_SH3 = 0x1a2,

        /// <summary>Hitachi SH3 DSP</summary>
        IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,

        /// <summary>Hitachi SH4</summary>
        IMAGE_FILE_MACHINE_SH4 = 0x1a6,

        /// <summary>Hitachi SH5</summary>
        IMAGE_FILE_MACHINE_SH5 = 0x1a8,

        /// <summary>ARM or Thumb (“interworking”)</summary>
        IMAGE_FILE_MACHINE_THUMB = 0x1c2,

        /// <summary>MIPS little‐endian WCE v2</summary>
        IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,

    }


}
