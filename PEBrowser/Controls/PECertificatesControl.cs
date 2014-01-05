using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows.Forms;
using PELib;

namespace PEBrowser.Controls
{
    internal partial class PECertificatesControl : UserControl, IPEFileViewer
    {
        public PECertificatesControl()
        {
            InitializeComponent();
            InitDgvSignatures();
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

        private void UpdateUI() {
            PopulateSignatures();
        }


        private void InitDgvSignatures() {
            dgvSignatures.Columns.Add("type", "Type of Signature");
            dgvSignatures.Columns["type"].Width = 160;

            dgvSignatures.Columns.Add("name", "Name of Signer");
            dgvSignatures.Columns["name"].Width = 160;

            dgvSignatures.Columns.Add("email", "Email Address");
            dgvSignatures.Columns["name"].Width = 160;

            dgvSignatures.Columns.Add("timestamp", "Timestamp");
            dgvSignatures.Columns["timestamp"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvSignatures.SelectionChanged += (sender, args) => dgvSignaturesSelectionChanged();
        }


        private void PopulateSignatures() {
            dgvSignatures.Rows.Clear();

            if (PEFile == null) return;

            var certs = PEFile.PE.CertificateTable;
            if (certs == null) return;

            foreach (var cert in certs.Entries) {
                int r = dgvSignatures.Rows.Add(cert.Type, cert.NameOfSigner, cert.EmailAddress, cert.Timestamp);
                dgvSignatures.Rows[r].Tag = cert;
            }

            dgvSignaturesSelectionChanged();
        }

        private CertificateTableEntry SelectedEntry {
            get {
                if (dgvSignatures.SelectedRows.Count == 0)
                    return null;
                return dgvSignatures.SelectedRows[0].Tag as CertificateTableEntry;
            }
        }

        private void dgvSignaturesSelectionChanged() {
            btnViewSig.Enabled = SelectedEntry != null;
        }

        private void btnViewSig_Click(object sender, EventArgs e) {
            var cert = SelectedEntry;

            var pkcs7cert = cert as PkcsSignedDataCertificateTableEntry;
            if (pkcs7cert != null) {
                View(pkcs7cert);
                return;
            }

            MessageBox.Show("Not implemented");
        }

        private void View(PkcsSignedDataCertificateTableEntry cert)
        {
            cert.Cms.ShowSignerInfoDialog(this.Handle);
        }
    }
}
