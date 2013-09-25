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
    internal partial class PEImportsControl : UserControl, IPEFileViewer
    {
        public PEImportsControl()
        {
            InitializeComponent();
            InitDgvLibraries();
            InitDgvFunctions();

            dgvLibraries.SelectionChanged += (sender, args) => dgvLibrariesSelectionChanged();
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
            PopulateLibraries();
        }



        private void InitDgvLibraries()
        {
            dgvLibraries.Columns.Add("name", "Name");
            //dgvLibraries.Columns["name"].Width = 160;
            dgvLibraries.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void PopulateLibraries()
        {
            dgvLibraries.Rows.Clear();

            if (PEFile == null) return;

            var imp = PEFile.PE.ImportTable;

            foreach (var impDir in imp.ImportDirectories) {
                int r = dgvLibraries.Rows.Add(impDir.Name);
                dgvLibraries.Rows[r].Tag = impDir;
            }

            dgvLibrariesSelectionChanged();
        }

        private void InitDgvFunctions()
        {
            dgvFunctions.Columns.Add("hint", "Hint");
            dgvFunctions.Columns["hint"].CellTemplate = new HexCell();
            dgvFunctions.Columns["hint"].Width = 100;

            dgvFunctions.Columns.Add("name", "Name");
            dgvFunctions.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }


        private void dgvLibrariesSelectionChanged() {
            ImportDirectory impDir = null;

            if (dgvLibraries.SelectedRows.Count > 0) {
                var row = dgvLibraries.SelectedRows[0];
                impDir = (ImportDirectory)row.Tag;
            }

            PopulateFunctions(impDir);
        }


        private void PopulateFunctions(ImportDirectory impDir) {
            dgvFunctions.Rows.Clear();

            if (impDir == null) return;

            foreach (var entry in impDir.ImportLookupTable.Entries) {
                var oe = entry as OrdinalImportLookupTableEntry;
                if (oe != null) {
                    dgvFunctions.Rows.Add(null, oe.Ordinal);
                    continue;
                }

                var ne = entry as NameImportLookupTableEntry;
                if (ne != null) {
                    dgvFunctions.Rows.Add(ne.Hint, ne.Name);
                    continue;
                }
            }

        }


    }
}
