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
    internal partial class PEExportsControl : UserControl, IPEFileViewer
    {
        public PEExportsControl() {
            InitializeComponent();
            InitDgvExports();
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
            PopulateExports();
        }


        private void InitDgvExports()
        {

            dgvExports.Columns.Add("entrypoint", "Entry Point");
            dgvExports.Columns["entrypoint"].CellTemplate = new HexCell();

            dgvExports.Columns.Add("ordinal", "Ordinal");

            dgvExports.Columns.Add("name", "Name");
            dgvExports.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void PopulateExports() {
            dgvExports.Rows.Clear();

            if (PEFile == null) return;

            var et = PEFile.PE.ExportTable;
            foreach (var entry in et.ExportEntries) {
                var addr = entry.RVA + PEFile.PE.OptionalHeader.ImageBase;
                object oAddr;
                if (PEFile.PE.OptionalHeader.IsPE32Plus)
                    oAddr = addr;
                else
                    oAddr = (UInt32) addr;

                dgvExports.Rows.Add(oAddr, entry.Ordinal, entry.Name);
            }
        }

    }
}
