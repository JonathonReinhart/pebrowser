using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PEBrowser.Controls
{
    internal partial class PESectionHeadersControl : UserControl, IPEFileViewer
    {
        public PESectionHeadersControl()
        {
            InitializeComponent();

            InitDgv();
        }

        private OpenPEFile m_pe;
        public OpenPEFile PEFile
        {
            get { return m_pe; }
            set
            {
                m_pe = value;
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            PopulateSections();
        }

        private void InitDgv()
        {
            dgvSections.Columns.Add("name", "Name");
            //dgvSections.Columns["name"].Width = 160;

            dgvSections.Columns.Add("vaddr", "Virtual Address");
            dgvSections.Columns["vaddr"].CellTemplate = new HexCell();

            dgvSections.Columns.Add("vsize", "Virtual Size");
            dgvSections.Columns["vsize"].CellTemplate = new HexCell();

            dgvSections.Columns.Add("rsize", "Size of Raw Data");
            dgvSections.Columns["rsize"].CellTemplate = new HexCell();

            dgvSections.Columns.Add("rptr", "Pointer to Raw Data");
            dgvSections.Columns["rptr"].CellTemplate = new HexCell();

            dgvSections.Columns.Add("chars", "Characteristics");
            dgvSections.Columns["chars"].CellTemplate = new HexCell();

            //dgvSections.Columns.Add("pdirs", "Pointing Directories");
        }

        private void PopulateSections()
        {
            dgvSections.Rows.Clear();

            if (PEFile == null) return;

            var oh = PEFile.PE.OptionalHeader;

            foreach (var s in PEFile.PE.ImageSectionHeaders) {
                var va = s.VirtualAddress + PEFile.PE.OptionalHeader.ImageBase;

                object objva;
                if (oh.IsPE32Plus)      // Don't replace with ?:
                    objva = va;
                else
                    objva = (UInt32)va;

                dgvSections.Rows.Add(s.Name, objva, s.VirtualSize,
                    s.SizeOfRawData, s.PointerToRawData, (UInt32)s.Characteristics);
            }

            // Show EOF extra data as a section
            uint extraLength;
            uint extraStart;
            if (PEHelper.DetectExtraData(m_pe, out extraStart, out extraLength)) {
                var rownum = dgvSections.Rows.Add("[EOF Extra Data]", null, null,
                                     extraLength, extraStart);
                MakeItalic(dgvSections.Rows[rownum].Cells[0]);
            }


        }

        private static void MakeItalic(DataGridViewCell cell) {
            cell.Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Italic);
        }
    }
}
