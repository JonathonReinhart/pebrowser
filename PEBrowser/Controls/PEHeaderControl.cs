using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PELib;

namespace PEBrowser.Controls
{
    internal partial class PEHeaderControl : UserControl, IPEFileViewer
    {
        

        private const int RowHeight = 20;

        public PEHeaderControl()
        {
            InitializeComponent();

            InitDgv(dgvFileHeader);
            InitDgv(dgvOptionalHeader);
            InitDgv(dgvOptionalHeader2);
        }

        private OpenPEFile m_pe;
        public OpenPEFile PEFile {
            get { return m_pe; }
            set {
                m_pe = value;
                UpdateUI();
            }
        }


        public void UpdateUI()
        {
            PopulateFileHeader();
            PopulateOptionalHeader();
        }

        private void InitDgv(DataGridView dgv) {
            dgv.ColumnAdded += (s, a) => { a.Column.SortMode = DataGridViewColumnSortMode.NotSortable; };

            dgv.Columns.Add("field", "Field Name");
            dgv.Columns["field"].Width = 140;

            dgv.Columns.Add("value", "Data Value");
            dgv.Columns["value"].CellTemplate = new HexCell();
            
            
            dgv.Columns.Add("description", "Description");
            dgv.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.RowTemplate.Height = RowHeight;
        }



        private void PopulateFileHeader() {
            dgvFileHeader.Rows.Clear();

            if (PEFile == null) return;

            var fh = PEFile.PE.FileHeader;

            if (Enum.IsDefined(typeof (IMAGE_FILE_MACHINE), fh.Machine))
                dgvFileHeader.Rows.Add("Machine", (UInt16)fh.Machine, fh.Machine);
            else
                dgvFileHeader.Rows.Add("Machine", (UInt16)fh.Machine);
            dgvFileHeader.Rows.Add("Number of Sections", fh.NumberOfSections);
            dgvFileHeader.Rows.Add("Time Date Stamp", fh.TimeDateStamp, fh.TimeStamp);
            dgvFileHeader.Rows.Add("Pointer to Symbol Table", fh.PointerToSymbolTable);
            dgvFileHeader.Rows.Add("Number of Symbols", fh.NumberOfSymbols);
            dgvFileHeader.Rows.Add("Size of Optional Header", fh.SizeOfOptionalHeader);
            dgvFileHeader.Rows.Add("Characteristics", fh.Characteristics);
        }


        private void PopulateOptionalHeader() {
            dgvOptionalHeader.Rows.Clear();
            dgvOptionalHeader2.Rows.Clear();

            if (PEFile == null) return;

            var oh = PEFile.PE.OptionalHeader;

            dgvOptionalHeader.Rows.Add("Magic", (UInt16)oh.Magic, oh.Magic);
            dgvOptionalHeader.Rows.Add("Linker Version", oh.MinorLinkerVersion << 8 | oh.MajorLinkerVersion,
                                       String.Format("{0}.{1}", oh.MajorLinkerVersion, oh.MinorLinkerVersion));
            dgvOptionalHeader.Rows.Add("Size of Code", oh.SizeOfCode);
            dgvOptionalHeader.Rows.Add("Size of Initialized Data", oh.SizeOfInitializedData);
            dgvOptionalHeader.Rows.Add("Size of Uninitialized Data", oh.SizeOfUninitializedData);
            dgvOptionalHeader.Rows.Add("Address of Entry Point", oh.AddressOfEntryPoint);
            dgvOptionalHeader.Rows.Add("Base of Code", oh.BaseOfCode);

            if (!oh.IsPE32Plus)
                dgvOptionalHeader.Rows.Add("Base Of Data", oh.BaseOfData);

            if (oh.IsPE32Plus)
                dgvOptionalHeader.Rows.Add("Image Base", oh.ImageBase);
            else
                dgvOptionalHeader.Rows.Add("Image Base", (UInt32)oh.ImageBase);

            dgvOptionalHeader.Rows.Add("Section Alignment", oh.SectionAlignment);
            dgvOptionalHeader.Rows.Add("File Alignment", oh.FileAlignment);
            


            dgvOptionalHeader2.Rows.Add("Operating System Version", oh.MinorOperatingSystemVersion << 8 | oh.MajorOperatingSystemVersion,
                String.Format("{0}.{1}", oh.MajorOperatingSystemVersion, oh.MinorOperatingSystemVersion));

            dgvOptionalHeader2.Rows.Add("Image Version", oh.MinorImageVersion << 8 | oh.MajorImageVersion,
                String.Format("{0}.{1}", oh.MajorImageVersion, oh.MinorImageVersion));

            dgvOptionalHeader2.Rows.Add("Subsystem Version", oh.MinorSubsystemVersion << 8 | oh.MajorSubsystemVersion,
                String.Format("{0}.{1}", oh.MajorSubsystemVersion, oh.MinorSubsystemVersion));


            dgvOptionalHeader2.Rows.Add("Win32 Version Value", oh.Win32VersionValue, "Reserved");
            dgvOptionalHeader2.Rows.Add("Size of Image", oh.SizeOfImage, oh.SizeOfImage + " bytes");
            dgvOptionalHeader2.Rows.Add("Size of Headers", oh.SizeOfHeaders);
            dgvOptionalHeader2.Rows.Add("Checksum", oh.CheckSum);
            dgvOptionalHeader2.Rows.Add("Subsystem", (UInt16)oh.Subsystem, oh.Subsystem);
            dgvOptionalHeader2.Rows.Add("DLL Characteristics", oh.DllCharacteristics);

            object stackres, stackcom, heapres, heapcom;
            if (oh.IsPE32Plus) {
                stackres = oh.SizeOfStackReserve;
                stackcom = oh.SizeOfStackCommit;
                heapres = oh.SizeOfHeapReserve;
                heapcom = oh.SizeOfHeapCommit;
            }
            else {
                stackres = (UInt32)oh.SizeOfStackReserve;
                stackcom = (UInt32)oh.SizeOfStackCommit;
                heapres = (UInt32)oh.SizeOfHeapReserve;
                heapcom = (UInt32)oh.SizeOfHeapCommit;
            }
            dgvOptionalHeader2.Rows.Add("Size of Stack Reserve", stackres);
            dgvOptionalHeader2.Rows.Add("Size of Stack Commit", stackcom);
            dgvOptionalHeader2.Rows.Add("Size of Heap Reserve", heapres);
            dgvOptionalHeader2.Rows.Add("Size of Heap Commit", heapcom);

            dgvOptionalHeader2.Rows.Add("Loader Flags", oh.LoaderFlags, "Obsolete");
            dgvOptionalHeader2.Rows.Add("Number of Data Directories", oh.NumberOfRvaAndSizes);
        }
    }


    internal class HexCell : DataGridViewTextBoxCell
    {
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            // This still doesn't work. I'd like to know if an object can be cast to an integer.
            // But since it's boxed, there's no easy way to tell.
            //if (!value.IsUnsignedInteger())
            //    return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);

            if (value is Byte)
                return String.Format("0x{0:X02}", value);
            if (value is UInt16)
                return String.Format("0x{0:X04}", value);
            if (value is UInt32)
                return String.Format("0x{0:X08}", value);
            if (value is UInt64)
                return String.Format("0x{0:X016}", value);

            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }
    }
}
