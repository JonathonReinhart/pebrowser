using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEBrowser.Forms;
using PELib;

namespace PEBrowser
{
    internal class PEHelper
    {
        public static bool DetectExtraData(OpenPEFile pefile, out uint extraStart, out uint extraLength) {
            extraStart = 0;
            extraLength = 0;

            // Iterate over the section headers, and determine what they say the end of the file is
            uint maxRawEnd = 0;
            foreach (var section in pefile.PE.ImageSectionHeaders) {
                var end = section.PointerToRawData + section.SizeOfRawData;
                if (end > maxRawEnd)
                    maxRawEnd = end;
            }

            if (pefile.FileSize > maxRawEnd) {
                extraStart = maxRawEnd;
                extraLength = (uint)pefile.FileSize - maxRawEnd;
                return true;
            }

            return false;
        }
    }
}
