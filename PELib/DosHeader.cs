using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PELib
{

    /// <summary>// DOS .EXE header</summary>
    public struct IMAGE_DOS_HEADER
    {
        // ReSharper disable InconsistentNaming

        /// <summary>Magic number</summary>
        public UInt16 e_magic;

        /// <summary>Bytes on last page of file</summary>
        public UInt16 e_cblp;

        /// <summary>Pages in file</summary>
        public UInt16 e_cp;

        /// <summary>Relocations</summary>
        public UInt16 e_crlc;

        /// <summary>Size of header in paragraphs</summary>
        public UInt16 e_cparhdr;

        /// <summary>Minimum extra paragraphs needed</summary>
        public UInt16 e_minalloc;

        /// <summary>Maximum extra paragraphs needed</summary>
        public UInt16 e_maxalloc;

        /// <summary>Initial (relative) SS value</summary>
        public UInt16 e_ss;

        /// <summary>Initial SP value</summary>
        public UInt16 e_sp;

        /// <summary>Checksum</summary>
        public UInt16 e_csum;

        /// <summary>Initial IP value</summary>
        public UInt16 e_ip;

        /// <summary>Initial (relative) CS value</summary>
        public UInt16 e_cs;

        /// <summary>File address of relocation table</summary>
        public UInt16 e_lfarlc;

        /// <summary>Overlay number</summary>
        public UInt16 e_ovno;

        /// <summary>Reserved words</summary>
        public UInt16 e_res_0;

        /// <summary>Reserved words</summary>
        public UInt16 e_res_1;

        /// <summary>Reserved words</summary>
        public UInt16 e_res_2;

        /// <summary>Reserved words</summary>
        public UInt16 e_res_3;

        /// <summary>OEM identifier (for e_oeminfo)</summary>
        public UInt16 e_oemid;

        /// <summary>OEM information; e_oemid specific</summary>
        public UInt16 e_oeminfo;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_0;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_1;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_2;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_3;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_4;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_5;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_6;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_7;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_8;

        /// <summary>Reserved words</summary>
        public UInt16 e_res2_9;

        /// <summary>File address of new exe header</summary>
        public UInt32 e_lfanew;

        // ReSharper restore InconsistentNaming
    }
}
