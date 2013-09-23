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
    internal partial class PEHeaderControl : UserControl
    {
        private OpenPEFile m_pe;

        public PEHeaderControl()
        {
            InitializeComponent();

            InitFileHeader();
        }

        private void InitFileHeader() {
            dgvFileHeader.ColumnAdded += (s, a) => { a.Column.SortMode = DataGridViewColumnSortMode.NotSortable; };

            //dgvFileHeader.Columns.Add("field", "Field");
            //dgvFileHeader.Columns.Add("value", "Value");
            dgvFileHeader.Columns["value"].CellTemplate = new HexCell();
            //dgvFileHeader.Columns.Add("description", "Description");
            
        }

        public void FileOpened(OpenPEFile file) {
            m_pe = file;

            PopulateFileHeader();
        }

        private void PopulateFileHeader() {
            var fh = m_pe.PE.FileHeader;

            if (Enum.IsDefined(typeof (PeFile.IMAGE_FILE_MACHINE), fh.Machine))
                dgvFileHeader.Rows.Add("Machine", fh.Machine, fh.Machine);
            else
                dgvFileHeader.Rows.Add("Machine", fh.Machine);
            dgvFileHeader.Rows.Add("Number of Sections", fh.NumberOfSections);
            dgvFileHeader.Rows.Add("Time Date Stamp", fh.TimeDateStamp, m_pe.PE.TimeStamp);

            
        }
    }


    internal class HexCell : DataGridViewTextBoxCell
    {
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            //long val;
            //try {
            //    val = (int) value;
            //}
            //catch (InvalidCastException) {
            //    return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
            //}

            return String.Format("0x{0:X}", value);
        }
    }
}
