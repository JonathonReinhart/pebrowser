using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PELib.ExtensionMethods;

namespace PELib
{
    public class ExportTable
    {
        private ExportTable() { }
        
        public string Name { get; private set; }

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

        #region Actual "Export Directory Table" fields

        public UInt32 ExportFlags { get; private set; }
        public UInt32 TimeDateStamp { get; private set; }
        public UInt16 MajorVersion { get; private set; }
        public UInt16 MinorVersion { get; private set; }

        #endregion


        public static ExportTable Read(PeFile pe, Stream stream)
        {
            var result = new ExportTable();

            var br = new BinaryReader(stream);

            result.ExportFlags = br.ReadUInt32();       // 00h DWORD  Characteristics
            result.TimeDateStamp = br.ReadUInt32();     // 04h DWORD  TimeDateStamp
            result.MajorVersion = br.ReadUInt16();      // 08h WORD   MajorVersion
            result.MinorVersion = br.ReadUInt16();      // 0Ah WORD   MinorVersion
            var nameRva = br.ReadUInt32();              // 0Ch DWORD  Name
            var ordinalBase = br.ReadUInt32();          // 10h DWORD  Base                  - The starting ordinal number for exports in this image.
            var numAddrs = br.ReadUInt32();             // 14h DWORD  NumberOfFunctions     - Number of entries in export address table
            var numNamesAndOrds = br.ReadUInt32();      // 18h DWORD  NumberOfNames         - Number of entries in nampe pointer table (and ordinal table)
            var exportAddrTblRva = br.ReadUInt32();     // 1Ch DWORD  AddressOfFunctions    - Address of export address table
            var namePtrRva = br.ReadUInt32();           // 20h DWORD  AddressOfNames        - Address of name pointer table
            var ordTblRva = br.ReadUInt32();            // 24h DWORD  AddressOfNameOrdinals - Address of ordinal table

            result.Name = stream.ReadNullTerminatedString(pe.RvaToFileOffset(nameRva), Encoding.ASCII);



            // "The export address table contains the address of exported entry points and exported data
            // and absolutes. An ordinal number is used as an index into the export address table."
            var exportTable = new UInt32[numAddrs];
            if (numAddrs > 0) {
                stream.Position = pe.RvaToFileOffset(exportAddrTblRva);
                for (int i = 0; i < numAddrs; i++) {
                    exportTable[i] = br.ReadUInt32();
                }
            }

            // "The export name pointer table is an array of addresses (RVAs) into the export name table."
            var nameTable = new string[numNamesAndOrds];
            if (numNamesAndOrds > 0) {
                stream.Position = pe.RvaToFileOffset(namePtrRva);
                for (int i = 0; i < numNamesAndOrds; i++) {
                    var rva = br.ReadUInt32();
                    nameTable[i] = stream.ReadNullTerminatedString(pe.RvaToFileOffset(rva), Encoding.ASCII);
                }
            }

            // "The export ordinal table is an array of 16-bit indexes into the export address table."
            // JR: The actual ordinals are the values in this table, plus the ordinal base.
            var ordinalTable = new UInt16[numNamesAndOrds];
            if (numNamesAndOrds > 0) {
                stream.Position = pe.RvaToFileOffset(ordTblRva);
                for (int i = 0; i < numNamesAndOrds; i++) {
                    ordinalTable[i] = br.ReadUInt16();
                }
            }

            // From these tables, construct our ExportEntry objects
            for (int i = 0; i < numNamesAndOrds; i++) {
                var ord = ordinalTable[i];
                var name = nameTable[i];
                var export = exportTable[ord];

                var entry = new ExportEntry(name, (int)(ord + ordinalBase), export);
                result.m_exports.Add(entry);
            }

            return result;
        }

        private readonly List<ExportEntry> m_exports = new List<ExportEntry>();
        public IEnumerable<ExportEntry> ExportEntries { get { return m_exports; } }
        
    }

    public class ExportEntry
    {
        public string Name { get; private set; }
        public int Ordinal { get; private set; }
        public UInt32 RVA { get; private set; }

        public ExportEntry(string name, int ordinal, UInt32 rva)
        {
            Name = name;
            Ordinal = ordinal;
            RVA = rva;
        }
    }


}
