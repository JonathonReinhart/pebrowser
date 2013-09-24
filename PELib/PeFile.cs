using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PELib
{
    // http://code.cheesydesign.com/?p=572

    /// <summary>
    /// Reads in the header information of the Portable Executable format.
    /// Provides information such as the date the assembly was compiled.
    /// </summary>
    public class PeFile
    {

        #region Public Methods

        public static PeFile FromFile(string filePath) {
            // Read in the DLL or EXE
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                return new PeFile(stream);
            }
        }

        public PeFile(Stream stream) {
            var zeroOffset = stream.Position;

            var reader = new BinaryReader(stream, Encoding.ASCII);
            DosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);

            // Add 4 bytes to the offset
            stream.Seek(zeroOffset + DosHeader.e_lfanew, SeekOrigin.Begin);

            UInt32 ntHeadersSignature = reader.ReadUInt32();
            FileHeader = new FileHeader(stream);

            OptionalHeader = new OptionalHeader(stream);

            for (int i = 0; i < FileHeader.NumberOfSections; i++ )
                m_sectionHeaders.Add(new SectionHeader(stream));


            if (OptionalHeader.ImportTable != null) {
                var idir_fo  = RvaToFileOffset(OptionalHeader.ImportTable.VirtualAddress);
                stream.Seek(zeroOffset + idir_fo, SeekOrigin.Begin);

                while (true) {
                    var dir = new ImportDirectoryTable(stream);
                    if (dir.IsNull) break;

                    var pos = stream.Position;

                    var name = stream.ReadNullTerminatedString(zeroOffset + dir.NameRva, Encoding.ASCII);


                    stream.Position = zeroOffset + dir.ImportLookupTableRva;
                    var table = new ImportLookupTable(stream, OptionalHeader.IsPE32Plus);




                    stream.Position = pos;
                }

            }

        }

        uint RvaToFileOffset(uint rva) {
            // Iterate over the section headers to find the section that has this RVA.
            var sec = ImageSectionHeaders.SingleOrDefault(sh => (rva > sh.VirtualAddress) && (rva < sh.VirtualAddress + sh.VirtualSize));
            if (sec == null)
                throw new Exception("Section with containing VA not found.");

            var off = rva - sec.VirtualAddress;
            return sec.PointerToRawData + off;
        }



        /// <summary>
        /// Gets the header of the .NET assembly that called this function
        /// </summary>
        /// <returns></returns>
        public static PeFile GetCallingAssemblyHeader() {
            // Get the path to the calling assembly, which is the path to the
            // DLL or EXE that we want the time of
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;

            // Get and return the timestamp
            return FromFile(filePath);
        }

        /// <summary>
        /// Gets the header of the .NET assembly that called this function
        /// </summary>
        /// <returns></returns>
        public static PeFile GetAssemblyHeader() {
            // Get the path to the calling assembly, which is the path to the
            // DLL or EXE that we want the time of
            string filePath = System.Reflection.Assembly.GetAssembly(typeof (PeFile)).Location;

            // Get and return the timestamp
            return FromFile(filePath);
        }

        /// <summary>
        /// Reads in a block from a file and converts it to the struct
        /// type specified by the template parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T FromBinaryReader<T>(BinaryReader reader) {
            // Read in a byte array
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof (T)));

            // Pin the managed memory while, copy it out the data, then unpin it
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof (T));
            handle.Free();

            return theStructure;
        }

        #endregion Public Methods

        #region Properties

        /// <summary>
        /// The DOS header
        /// </summary>
        public IMAGE_DOS_HEADER DosHeader { get; private set; }

        public FileHeader FileHeader { get; private set; }

        public OptionalHeader OptionalHeader { get; private set; }

        private readonly List<SectionHeader> m_sectionHeaders = new List<SectionHeader>();
        public IEnumerable<SectionHeader> ImageSectionHeaders { get { return m_sectionHeaders; } }

        #endregion Properties
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

        public ImportDirectoryTable(Stream stream) {
            var br = new BinaryReader(stream);

            ImportLookupTableRva = br.ReadUInt32();
            TimeDateStamp = br.ReadUInt32();
            ForwarderChain = br.ReadUInt32();
            NameRva = br.ReadUInt32();
            IatRva = br.ReadUInt32();
        }

        public bool IsNull {
            get {
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


        public ImportLookupTable(Stream stream, bool pe32Plus) {
            var br = new BinaryReader(stream, Encoding.ASCII);

            UInt64 ordinalMask = pe32Plus ? (1u << 63) : (1u << 31);

            while (true) {
                UInt64 val = pe32Plus ? br.ReadUInt64() : br.ReadUInt32();
                if (val == 0) break;

                bool useOrd = (val & ordinalMask) != 0;
                
                if (useOrd) {
                    UInt16 ord = (UInt16) (val & 0xFFFF);
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

    public abstract class ImportLookupTableEntry {}
    public class NameImportLookupTableEntry : ImportLookupTableEntry
    {
        public UInt16 Hint { get; private set; }
        public string Name { get; private set; }
        public NameImportLookupTableEntry(UInt16 hint, string name) {
            Hint = hint;
            Name = name;
        }
    }

    public class OrdinalImportLookupTableEntry : ImportLookupTableEntry
    {
        public UInt16 Ordinal { get; private set; }
        public OrdinalImportLookupTableEntry(UInt16 ordinal) {
            Ordinal = ordinal;
        }
    }

    /*
    public class ImportLookupTableEntry
    {
        private UInt32 m_value;

        public bool UseOrdinal { get; private set; }

        public ImportLookupTableEntry(bool useOrdinal) {
            UseOrdinal = useOrdinal;
        }
    }
    */


 



}