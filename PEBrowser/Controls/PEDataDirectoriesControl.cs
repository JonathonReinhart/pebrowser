using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PELib;

namespace PEBrowser.Controls
{
    internal partial class PEDataDirectoriesControl : UserControl, IPEFileViewer
    {
        public PEDataDirectoriesControl()
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
            PopulateDataDirectories();
        }

        private void InitDgv()
        {
            dgvDirs.Columns.Add("name", "Directory Name");
            dgvDirs.Columns["name"].Width = 160;

            dgvDirs.Columns.Add("address", "Virtual Address");
            dgvDirs.Columns["address"].CellTemplate = new HexCell();

            dgvDirs.Columns.Add("size", "Size");
            dgvDirs.Columns["size"].CellTemplate = new HexCell();
        }


        private void PopulateDataDirectories() {
            dgvDirs.Rows.Clear();

            if (PEFile == null) return;

            var oh = PEFile.PE.OptionalHeader;

            AddDataDir("[0] Export Table", oh.ExportTable);
            AddDataDir("[1] Import Table", oh.ImportTable);
            AddDataDir("[2] Resource Table", oh.ResourceTable);
            AddDataDir("[3] Exception Table", oh.ExceptionTable);
            AddDataDir("[4] Certificate Table", oh.CertificateTable);
            AddDataDir("[5] Relocation Table", oh.BaseRelocationTable);
            AddDataDir("[6] Debug Data", oh.Debug);
            AddDataDir("[7] Architecture-specific data", oh.Architecture);
            AddDataDir("[8] Global Pointer (MIPS GP)", oh.GlobalPtr);
            AddDataDir("[9] TLS Table", oh.TLSTable);
            AddDataDir("[10] Load Configuration Table", oh.LoadConfigTable);
            AddDataDir("[11] Bound Import Table", oh.BoundImport);
            AddDataDir("[12] Import Address Table", oh.IAT);
            AddDataDir("[13] Delay Import Descriptor", oh.DelayImportDescriptor);
            AddDataDir("[14] CLR Runtime Header", oh.CLRRuntimeHeader);
            AddDataDir("[15] Reserved", oh.Reserved);
        }

        private void AddDataDir(string name, DataDirectory dir) {
            if (dir == null) return;

            if (dir.VirtualAddress == 0)
                dgvDirs.Rows.Add(name);
            else
                dgvDirs.Rows.Add(name, dir.VirtualAddress, dir.Size);
        }
    }
}
