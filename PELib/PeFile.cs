using System;
using System.Collections.Generic;
using System.IO;
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
        #region File Header Structures

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

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DATA_DIRECTORY
        {
            public UInt32 VirtualAddress;
            public UInt32 Size;
        }



        #endregion File Header Structures


        #region Public Methods

        public static PeFile FromFile(string filePath) {
            // Read in the DLL or EXE
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                return new PeFile(stream);
            }
        }

        public PeFile(Stream stream) {
            var zeroOffset = stream.Position;

            var reader = new BinaryReader(stream);
            DosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);

            // Add 4 bytes to the offset
            stream.Seek(zeroOffset + DosHeader.e_lfanew, SeekOrigin.Begin);

            UInt32 ntHeadersSignature = reader.ReadUInt32();
            FileHeader = new FileHeader(stream);

            OptionalHeader = new OptionalHeader(stream);

            for (int i = 0; i < FileHeader.NumberOfSections; i++ )
                m_sectionHeaders.Add(new SectionHeader(stream));
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
        /// Gets if the file header is 32 bit or not
        /// </summary>
        public bool Is32BitHeader {
            get {
                const ushort IMAGE_FILE_32BIT_MACHINE = 0x0100;
                return (IMAGE_FILE_32BIT_MACHINE & FileHeader.Characteristics) == IMAGE_FILE_32BIT_MACHINE;
            }
        }

        /// <summary>
        /// The DOS header
        /// </summary>
        public IMAGE_DOS_HEADER DosHeader { get; private set; }

        public FileHeader FileHeader { get; private set; }

        public OptionalHeader OptionalHeader { get; private set; }

        private readonly List<SectionHeader> m_sectionHeaders = new List<SectionHeader>();
        public IEnumerable<SectionHeader> ImageSectionHeaders { get { return m_sectionHeaders; } }

        /// <summary>
        /// Gets the timestamp from the file header
        /// </summary>
        public DateTime TimeStamp {
            get {
                // Timestamp is a date offset from 1970
                var returnValue = new DateTime(1970, 1, 1, 0, 0, 0);

                // Add in the number of seconds since 1970/1/1
                returnValue = returnValue.AddSeconds(FileHeader.TimeDateStamp);
                // Adjust to local timezone
                returnValue += TimeZone.CurrentTimeZone.GetUtcOffset(returnValue);

                return returnValue;
            }
        }


        #endregion Properties
    }

    public class FileHeader
    {
        public IMAGE_FILE_MACHINE Machine { get; set; }
        public UInt16 NumberOfSections { get; set; }
        public UInt32 TimeDateStamp { get; set; }
        public UInt32 PointerToSymbolTable { get; set; }
        public UInt32 NumberOfSymbols { get; set; }
        public UInt16 SizeOfOptionalHeader { get; set; }
        public UInt16 Characteristics { get; set; }

        public FileHeader(Stream stream) {
            var br = new BinaryReader(stream);
            
            Machine = (IMAGE_FILE_MACHINE)br.ReadUInt16();
            NumberOfSections = br.ReadUInt16();
            TimeDateStamp = br.ReadUInt32();
            PointerToSymbolTable = br.ReadUInt32();
            NumberOfSymbols = br.ReadUInt32();
            SizeOfOptionalHeader = br.ReadUInt16();
            Characteristics = br.ReadUInt16();
        }
    }



    public class OptionalHeader
    {
        public MagicValue Magic { get; set; }
        public Byte MajorLinkerVersion { get; set; }
        public Byte MinorLinkerVersion { get; set; }
        public UInt32 SizeOfCode { get; set; }
        public UInt32 SizeOfInitializedData { get; set; }
        public UInt32 SizeOfUninitializedData { get; set; }
        public UInt32 AddressOfEntryPoint { get; set; }
        public UInt32 BaseOfCode { get; set; }
        public UInt32 BaseOfData { get; set; }
        public UInt64 ImageBase { get; set; }
        public UInt32 SectionAlignment { get; set; }
        public UInt32 FileAlignment { get; set; }
        public UInt16 MajorOperatingSystemVersion { get; set; }
        public UInt16 MinorOperatingSystemVersion { get; set; }
        public UInt16 MajorImageVersion { get; set; }
        public UInt16 MinorImageVersion { get; set; }
        public UInt16 MajorSubsystemVersion { get; set; }
        public UInt16 MinorSubsystemVersion { get; set; }
        public UInt32 Win32VersionValue { get; set; }
        public UInt32 SizeOfImage { get; set; }
        public UInt32 SizeOfHeaders { get; set; }
        public UInt32 CheckSum { get; set; }
        public IMAGE_SUBSYSTEM Subsystem { get; set; }
        public UInt16 DllCharacteristics { get; set; }
        public UInt64 SizeOfStackReserve { get; set; }
        public UInt64 SizeOfStackCommit { get; set; }
        public UInt64 SizeOfHeapReserve { get; set; }
        public UInt64 SizeOfHeapCommit { get; set; }
        public UInt32 LoaderFlags { get; set; }
        public UInt32 NumberOfRvaAndSizes { get; set; }

        public DataDirectory ExportTable { get; set; }
        public DataDirectory ImportTable { get; set; }
        public DataDirectory ResourceTable { get; set; }
        public DataDirectory ExceptionTable { get; set; }
        public DataDirectory CertificateTable { get; set; }
        public DataDirectory BaseRelocationTable { get; set; }
        public DataDirectory Debug { get; set; }
        public DataDirectory Architecture { get; set; }
        public DataDirectory GlobalPtr { get; set; }
        public DataDirectory TLSTable { get; set; }
        public DataDirectory LoadConfigTable { get; set; }
        public DataDirectory BoundImport { get; set; }
        public DataDirectory IAT { get; set; }
        public DataDirectory DelayImportDescriptor { get; set; }
        public DataDirectory CLRRuntimeHeader { get; set; }
        public DataDirectory Reserved { get; set; }


        public enum MagicValue : ushort {
            PE32 = 0x10b,
            PE32Plus = 0x20b,
        }

        public bool IsPE32Plus {
            get { return Magic == MagicValue.PE32Plus; }
        }


        public OptionalHeader(Stream stream) {
            var br = new BinaryReader(stream);

            Magic = (MagicValue)br.ReadUInt16();

            switch (Magic) {
                case MagicValue.PE32:
                case MagicValue.PE32Plus:
                    break;
                default:
                    throw new Exception(String.Format("Invalid PE32 Optional Header magic value (0x{0:X})", Magic));
            }

            // TODO: Make sure no read goes past SizeOfOptionalHeader (pain-in-the-ass bucket)

            MajorLinkerVersion = br.ReadByte();
            MinorLinkerVersion = br.ReadByte();
            SizeOfCode = br.ReadUInt32();
            SizeOfInitializedData = br.ReadUInt32();
            SizeOfUninitializedData = br.ReadUInt32();
            AddressOfEntryPoint = br.ReadUInt32();
            BaseOfCode = br.ReadUInt32();
            if (!IsPE32Plus)
                BaseOfData = br.ReadUInt32();
            ImageBase = IsPE32Plus ? br.ReadUInt64() : br.ReadUInt32();
            SectionAlignment = br.ReadUInt32();
            FileAlignment = br.ReadUInt32();
            MajorOperatingSystemVersion = br.ReadUInt16();
            MinorOperatingSystemVersion = br.ReadUInt16();
            MajorImageVersion = br.ReadUInt16();
            MinorImageVersion = br.ReadUInt16();
            MajorSubsystemVersion = br.ReadUInt16();
            MinorSubsystemVersion = br.ReadUInt16();
            Win32VersionValue = br.ReadUInt32();
            SizeOfImage = br.ReadUInt32();
            SizeOfHeaders = br.ReadUInt32();
            CheckSum = br.ReadUInt32();
            Subsystem = (IMAGE_SUBSYSTEM)br.ReadUInt16();
            DllCharacteristics = br.ReadUInt16();
            SizeOfStackReserve = IsPE32Plus ? br.ReadUInt64() : br.ReadUInt32();
            SizeOfStackCommit = IsPE32Plus ? br.ReadUInt64() : br.ReadUInt32();
            SizeOfHeapReserve = IsPE32Plus ? br.ReadUInt64() : br.ReadUInt32();
            SizeOfHeapCommit = IsPE32Plus ? br.ReadUInt64() : br.ReadUInt32();
            LoaderFlags = br.ReadUInt32();
            NumberOfRvaAndSizes = br.ReadUInt32();


            if (NumberOfRvaAndSizes >= 1)
                ExportTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 2)
                ImportTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 3)
                ResourceTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 4)
                ExceptionTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 5)
                CertificateTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 6)
                BaseRelocationTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 7)
                Debug  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 8)
                Architecture  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 9)
                GlobalPtr  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 10)
                TLSTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 11)
                LoadConfigTable  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 12)
                BoundImport  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 13)
                IAT  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 14)
                DelayImportDescriptor  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 15)
                CLRRuntimeHeader  = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 16)
                Reserved  = new DataDirectory(stream);
        }
    }

    /// <summary>
    /// IMAGE_DATA_DIRECTORY
    /// </summary>
    public class DataDirectory
    {
        public UInt32 VirtualAddress { get; set; }
        public UInt32 Size { get; set; }

        public DataDirectory(Stream stream) {
            var br = new BinaryReader(stream);

            VirtualAddress = br.ReadUInt32();
            Size = br.ReadUInt32();
        }
    }

    public class SectionHeader
    {
        public string Name { get; set; }
        public UInt32 VirtualSize { get; set; }
        public UInt32 VirtualAddress { get; set; }
        public UInt32 SizeOfRawData { get; set; }
        public UInt32 PointerToRawData { get; set; }
        public UInt32 PointerToRelocations { get; set; }
        public UInt32 PointerToLinenumbers { get; set; }
        public UInt16 NumberOfRelocations { get; set; }
        public UInt16 NumberOfLinenumbers { get; set; }
        public DataSectionFlags Characteristics { get; set; }


        public SectionHeader(Stream stream) {
            var br = new BinaryReader(stream);

            Name = Encoding.ASCII.GetString(br.ReadBytes(8)).TrimEnd('\0');
            VirtualSize = br.ReadUInt32();
            VirtualAddress = br.ReadUInt32();
            SizeOfRawData = br.ReadUInt32();
            PointerToRawData = br.ReadUInt32();
            PointerToRelocations = br.ReadUInt32();
            PointerToLinenumbers = br.ReadUInt32();
            NumberOfRelocations = br.ReadUInt16();
            NumberOfLinenumbers = br.ReadUInt16();
            Characteristics = (DataSectionFlags)br.ReadUInt32();
        }
    }



    public enum IMAGE_SUBSYSTEM : ushort
    {
        /// <summary>An unknown subsystem</summary>
        IMAGE_SUBSYSTEM_UNKNOWN = 0,

        /// <summary>Device drivers and native Windows processes</summary>
        IMAGE_SUBSYSTEM_NATIVE = 1,

        /// <summary>The Windows graphical user interface (GUI) subsystem</summary>
        IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,

        /// <summary>The Windows character subsystem</summary>
        IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,

        /// <summary>The Posix character subsystem</summary>
        IMAGE_SUBSYSTEM_POSIX_CUI = 7,

        /// <summary>Windows CE</summary>
        IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,

        /// <summary>An Extensible Firmware Interface (EFI) application</summary>
        IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,

        /// <summary>An EFI driver with boot services</summary>
        IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,

        /// <summary>An EFI driver with run‐time services</summary>
        IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,

        /// <summary>An EFI ROM image</summary>
        IMAGE_SUBSYSTEM_EFI_ROM = 13,

        /// <summary>XBox</summary>
        IMAGE_SUBSYSTEM_XBOX = 14,
    }

    [Flags]
    public enum DataSectionFlags : uint
    {
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeReg = 0x00000000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeDsect = 0x00000001,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeNoLoad = 0x00000002,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeGroup = 0x00000004,

        /// <summary>
        /// The section should not be padded to the next boundary. This flag is obsolete and is replaced by IMAGE_SCN_ALIGN_1BYTES. This is valid only for object files.
        /// </summary>
        TypeNoPadded = 0x00000008,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeCopy = 0x00000010,

        /// <summary>
        /// The section contains executable code.
        /// </summary>
        ContentCode = 0x00000020,

        /// <summary>
        /// The section contains initialized data.
        /// </summary>
        ContentInitializedData = 0x00000040,

        /// <summary>
        /// The section contains uninitialized data.
        /// </summary>
        ContentUninitializedData = 0x00000080,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        LinkOther = 0x00000100,

        /// <summary>
        /// The section contains comments or other information. The .drectve section has this type. This is valid for object files only.
        /// </summary>
        LinkInfo = 0x00000200,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        TypeOver = 0x00000400,

        /// <summary>
        /// The section will not become part of the image. This is valid only for object files.
        /// </summary>
        LinkRemove = 0x00000800,

        /// <summary>
        /// The section contains COMDAT data. For more information, see section 5.5.6, COMDAT Sections (Object Only). This is valid only for object files.
        /// </summary>
        LinkComDat = 0x00001000,

        /// <summary>
        /// Reset speculative exceptions handling bits in the TLB entries for this section.
        /// </summary>
        NoDeferSpecExceptions = 0x00004000,

        /// <summary>
        /// The section contains data referenced through the global pointer (GP).
        /// </summary>
        RelativeGP = 0x00008000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemPurgeable = 0x00020000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Memory16Bit = 0x00020000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryLocked = 0x00040000,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MemoryPreload = 0x00080000,

        /// <summary>
        /// Align data on a 1-byte boundary. Valid only for object files.
        /// </summary>
        Align1Bytes = 0x00100000,

        /// <summary>
        /// Align data on a 2-byte boundary. Valid only for object files.
        /// </summary>
        Align2Bytes = 0x00200000,

        /// <summary>
        /// Align data on a 4-byte boundary. Valid only for object files.
        /// </summary>
        Align4Bytes = 0x00300000,

        /// <summary>
        /// Align data on an 8-byte boundary. Valid only for object files.
        /// </summary>
        Align8Bytes = 0x00400000,

        /// <summary>
        /// Align data on a 16-byte boundary. Valid only for object files.
        /// </summary>
        Align16Bytes = 0x00500000,

        /// <summary>
        /// Align data on a 32-byte boundary. Valid only for object files.
        /// </summary>
        Align32Bytes = 0x00600000,

        /// <summary>
        /// Align data on a 64-byte boundary. Valid only for object files.
        /// </summary>
        Align64Bytes = 0x00700000,

        /// <summary>
        /// Align data on a 128-byte boundary. Valid only for object files.
        /// </summary>
        Align128Bytes = 0x00800000,

        /// <summary>
        /// Align data on a 256-byte boundary. Valid only for object files.
        /// </summary>
        Align256Bytes = 0x00900000,

        /// <summary>
        /// Align data on a 512-byte boundary. Valid only for object files.
        /// </summary>
        Align512Bytes = 0x00A00000,

        /// <summary>
        /// Align data on a 1024-byte boundary. Valid only for object files.
        /// </summary>
        Align1024Bytes = 0x00B00000,

        /// <summary>
        /// Align data on a 2048-byte boundary. Valid only for object files.
        /// </summary>
        Align2048Bytes = 0x00C00000,

        /// <summary>
        /// Align data on a 4096-byte boundary. Valid only for object files.
        /// </summary>
        Align4096Bytes = 0x00D00000,

        /// <summary>
        /// Align data on an 8192-byte boundary. Valid only for object files.
        /// </summary>
        Align8192Bytes = 0x00E00000,

        /// <summary>
        /// The section contains extended relocations.
        /// </summary>
        LinkExtendedRelocationOverflow = 0x01000000,

        /// <summary>
        /// The section can be discarded as needed.
        /// </summary>
        MemoryDiscardable = 0x02000000,

        /// <summary>
        /// The section cannot be cached.
        /// </summary>
        MemoryNotCached = 0x04000000,

        /// <summary>
        /// The section is not pageable.
        /// </summary>
        MemoryNotPaged = 0x08000000,

        /// <summary>
        /// The section can be shared in memory.
        /// </summary>
        MemoryShared = 0x10000000,

        /// <summary>
        /// The section can be executed as code.
        /// </summary>
        MemoryExecute = 0x20000000,

        /// <summary>
        /// The section can be read.
        /// </summary>
        MemoryRead = 0x40000000,

        /// <summary>
        /// The section can be written to.
        /// </summary>
        MemoryWrite = 0x80000000
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