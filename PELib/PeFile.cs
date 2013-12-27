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
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (stream.Position != 0)
                throw new ArgumentException("Stream position must be zero.");
            
            var reader = new BinaryReader(stream, Encoding.ASCII);

            // Verify COFF Header
            DosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);
            if (DosHeader.e_magic != IMAGE_DOS_SIGNATURE)    // 'MZ'
                throw new PeException("Invalid MZ signature");

            // Verify PE Header
            stream.Position = DosHeader.e_lfanew;

            UInt32 ntHeadersSignature = reader.ReadUInt32();
            if (ntHeadersSignature != IMAGE_NT_SIGNATURE)
                throw new PeException("Invalid PE header signature");

            // Calculate the checksum
            using (new StreamKeeper(stream)) {
                stream.Position = 0;
                CalcChecksum(stream);    
            }

            
            // Process the subsequent headers
            FileHeader = new FileHeader(stream);

            OptionalHeader = new OptionalHeader(stream);

            for (int i = 0; i < FileHeader.NumberOfSections; i++ )
                m_sectionHeaders.Add(new SectionHeader(stream));

            // At this point we no longer care about the position of the stream.
            // Everything is located somewhere referenced by something else.



            ReadImportTable(stream);
            ReadExportTable(stream);

        }

        private void CalcChecksum(Stream stream) {
            var buf = new byte[stream.Length];
            stream.Read(buf, 0, buf.Length);

            uint HeaderSum;
            uint CheckSum;
            var result = Imagehlp.CheckSumMappedFile(buf, (uint)buf.Length, out HeaderSum, out CheckSum);
            if (result == IntPtr.Zero)
                throw new Exception(String.Format("CheckSumMappedFile() failed {0}", Marshal.GetLastWin32Error()));

            CalculatedCheckSum = CheckSum;
        }

        public UInt32 CalculatedCheckSum { get; private set; }

        public uint RvaToFileOffset(uint rva) {
            // Iterate over the section headers to find the section that has this RVA.
            var sec = ImageSectionHeaders.SingleOrDefault(sh => (rva >= sh.VirtualAddress) && (rva < sh.VirtualAddress + sh.VirtualSize));
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





        #endregion Properties

        #region Section Headers

        private readonly List<SectionHeader> m_sectionHeaders = new List<SectionHeader>();
        public IEnumerable<SectionHeader> ImageSectionHeaders { get { return m_sectionHeaders; } }


        #endregion


        #region Import Table

        public ImportTable ImportTable { get; private set;}

        private void ReadImportTable(Stream stream)
        {
            if (DataDirectory.IsNullOrEmpty(OptionalHeader.ImportTable)) return;

            var fo = RvaToFileOffset(OptionalHeader.ImportTable.VirtualAddress);
            stream.Position = fo;

            ImportTable = ImportTable.Read(this, stream);
        }

        #endregion


        #region Export Table

        public ExportTable ExportTable { get; private set; }

        private void ReadExportTable(Stream stream)
        {
            if (DataDirectory.IsNullOrEmpty(OptionalHeader.ExportTable)) return;

            var fo = RvaToFileOffset(OptionalHeader.ExportTable.VirtualAddress);
            stream.Position = fo;

            ExportTable = ExportTable.Read(this, stream);

        }

        #endregion

        // ReSharper disable InconsistentNaming
        private const UInt16 IMAGE_DOS_SIGNATURE = 0x5A4D;
        private const UInt32 IMAGE_NT_SIGNATURE = 0x00004550;
        // ReSharper restore InconsistentNaming

    }


    public class PeException : Exception
    {
        public PeException() : base() { }
        public PeException(string message) : base(message) { }
        public PeException(string message, Exception innerException) : base(message, innerException) { }
    }


}