using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PELib
{

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


        public enum MagicValue : ushort
        {
            PE32 = 0x10b,
            PE32Plus = 0x20b,
        }

        public bool IsPE32Plus
        {
            get { return Magic == MagicValue.PE32Plus; }
        }


        public OptionalHeader(Stream stream)
        {
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
                ExportTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 2)
                ImportTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 3)
                ResourceTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 4)
                ExceptionTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 5)
                CertificateTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 6)
                BaseRelocationTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 7)
                Debug = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 8)
                Architecture = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 9)
                GlobalPtr = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 10)
                TLSTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 11)
                LoadConfigTable = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 12)
                BoundImport = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 13)
                IAT = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 14)
                DelayImportDescriptor = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 15)
                CLRRuntimeHeader = new DataDirectory(stream);
            if (NumberOfRvaAndSizes >= 16)
                Reserved = new DataDirectory(stream);
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


}
