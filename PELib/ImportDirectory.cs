﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PELib
{
    public class ImportTable
    {
        private ImportTable() { }

        private readonly List<ImportDirectory> m_importDirectories = new List<ImportDirectory>();
        public IEnumerable<ImportDirectory> ImportDirectories { get { return m_importDirectories; } }

        public static ImportTable Read(Stream stream, bool isPE32Plus)
        {
            var tbl = new ImportTable();

            while (true) {
                var imp = ImportDirectory.Read(stream, isPE32Plus);
                if (imp == null) break;

                tbl.m_importDirectories.Add(imp);
            }

            return tbl;
        }
    }


    public class ImportDirectory
    {
        public string Name { get; private set; }
        public ImportLookupTable ImportLookupTable { get; private set; }

        private ImportDirectory() { }

        public static ImportDirectory Read(Stream stream, bool isPe32Plus)
        {
            var dir = new ImportDirectoryTable(stream);
            if (dir.IsNull) return null;

            var pos = stream.Position;

            try {
                var result = new ImportDirectory();

                result.Name = stream.ReadNullTerminatedString(dir.NameRva, Encoding.ASCII);

                stream.Position = dir.ImportLookupTableRva;
                result.ImportLookupTable = new ImportLookupTable(stream, isPe32Plus);

                return result;
            }
            finally {
                stream.Position = pos;
            }
        }
    }




    public class ImportDirectoryTable
    {
        /// <summary>The RVA of the import lookup table. This table contains a name or ordinal for each import.</summary>
        public UInt32 ImportLookupTableRva { get; private set; }

        /// <summary> The stamp that is set to zero until the image is bound. After the image is bound,
        /// this field is set to the time/data stamp of the DLL.</summary>
        public UInt32 TimeDateStamp { get; private set; }

        /// <summary>The index of the first forwarder reference.</summary>
        public UInt32 ForwarderChain { get; private set; }

        /// <summary>The address of an ASCII string that contains the name of
        /// the DLL. This address is relative to the image base.</summary>
        public UInt32 NameRva { get; private set; }

        /// <summary> The RVA of the import address table. The contents of this table are identical to
        /// the contents of the import lookup table until the image is bound.</summary>
        public UInt32 IatRva { get; private set; }

        public ImportDirectoryTable(Stream stream)
        {
            var br = new BinaryReader(stream);

            ImportLookupTableRva = br.ReadUInt32();
            TimeDateStamp = br.ReadUInt32();
            ForwarderChain = br.ReadUInt32();
            NameRva = br.ReadUInt32();
            IatRva = br.ReadUInt32();
        }

        public bool IsNull
        {
            get
            {
                return (ImportLookupTableRva == 0)
                       && (TimeDateStamp == 0)
                       && (ForwarderChain == 0)
                       && (NameRva == 0)
                       && (IatRva == 0);
            }
        }
    }


    public class ImportLookupTable
    {
        private readonly List<ImportLookupTableEntry> m_entries = new List<ImportLookupTableEntry>();
        public IEnumerable<ImportLookupTableEntry> Entries { get { return m_entries; } }


        public ImportLookupTable(Stream stream, bool pe32Plus)
        {
            var br = new BinaryReader(stream, Encoding.ASCII);

            UInt64 ordinalMask = pe32Plus ? (1u << 63) : (1u << 31);

            while (true) {
                UInt64 val = pe32Plus ? br.ReadUInt64() : br.ReadUInt32();
                if (val == 0) break;

                bool useOrd = (val & ordinalMask) != 0;

                if (useOrd) {
                    UInt16 ord = (UInt16)(val & 0xFFFF);
                    m_entries.Add(new OrdinalImportLookupTableEntry(ord));
                }
                else {
                    var pos = stream.Position;

                    UInt32 hintNameRva = (UInt32)(val & 0x7FFFFFFF);
                    stream.Seek(hintNameRva, SeekOrigin.Begin);

                    var hint = br.ReadUInt16();
                    var name = br.ReadNullTerminatedString();
                    m_entries.Add(new NameImportLookupTableEntry(hint, name));

                    stream.Position = pos;
                }
            }
        }
    }

    public abstract class ImportLookupTableEntry { }
    public class NameImportLookupTableEntry : ImportLookupTableEntry
    {
        public UInt16 Hint { get; private set; }
        public string Name { get; private set; }
        public NameImportLookupTableEntry(UInt16 hint, string name)
        {
            Hint = hint;
            Name = name;
        }
    }

    public class OrdinalImportLookupTableEntry : ImportLookupTableEntry
    {
        public UInt16 Ordinal { get; private set; }
        public OrdinalImportLookupTableEntry(UInt16 ordinal)
        {
            Ordinal = ordinal;
        }
    }
}