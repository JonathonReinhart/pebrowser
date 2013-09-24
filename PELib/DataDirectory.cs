using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PELib
{
    /// <summary>
    /// IMAGE_DATA_DIRECTORY
    /// </summary>
    public class DataDirectory
    {
        public UInt32 VirtualAddress { get; set; }
        public UInt32 Size { get; set; }

        public DataDirectory(Stream stream)
        {
            var br = new BinaryReader(stream);

            VirtualAddress = br.ReadUInt32();
            Size = br.ReadUInt32();
        }
    }

}
