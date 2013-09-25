using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PELib;

namespace PEBrowser.Forms
{
    internal partial class MainForm : Form
    {
        private OpenPEFile m_pe;
        private readonly IEnumerable<IPEFileViewer> m_peFileViewers;

        public MainForm()
        {
            InitializeComponent();

            m_peFileViewers = new List<IPEFileViewer> {
                peHeaderControl,
                peDataDirectoriesControl,
                peSectionHeadersControl,
                peImportsControl,
                peExportsControl,
            };

            LogLine("Started up.");
        }

        #region Logging
        public void LogLine(string line) {
            logBox.LogLine(line);
        }
        
        public void LogLineFormat(string format, params object[] args) {
            logBox.LogLineFormat(format, args);
        }
        #endregion



        #region Menu Bar

        #region File Menu

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e) {
            ShowOpenFileDialog();
        }

        private void reloadFileToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void closeFileToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            CloseFile();
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        #endregion


        #endregion

        #region Tool Strip
        private void tsbOpen_Click(object sender, EventArgs e)
        {
            ShowOpenFileDialog();
        }

        private void tsbReload_Click(object sender, EventArgs e)
        {

        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        #endregion


        #region Opening Files

        private void ShowOpenFileDialog() {
            var dialog = new OpenFileDialog {
                Multiselect = false, 
                CheckFileExists = true
            };

            var filter = new FileDialogFilterBuilder { AllTypesName = "PE Files"};
            filter.AddExtension("Executable Files", "exe");
            filter.AddExtension("Library Files", "dll");
            filter.AddExtension("Drivers", "sys");
            filter.AddExtension("MS Styles", "msstyles");
            filter.AddExtension("Control Panel Applets", "cpl");
            filter.AddExtension("ActiveX Library", "ocx");
            filter.AddExtension("Codec Files", "acm", "ax");
            filter.AddExtension("Borland Library", "dpl", "bpl");
            filter.AddExtension("Screen Savers", "scr");
            filter.AddExtension("Multilingual User Interface Pack", "mui");
            dialog.Filter = filter.Filter;

            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) return;
            
            OpenFile(dialog.FileName);
        }

        private void OpenFile(string path) {
            CloseFile();

            LogLineFormat("Open File: {0}", path);

            m_pe = OpenPEFile.Open(path);
      
            OnFileOpened();
        }

        private void OnFileOpened() {
            LogLineFormat("File size: 0x{0:X} ({0})", m_pe.FileSize);


            var correct = "";
            var hdrCksum = m_pe.PE.OptionalHeader.CheckSum;
            if (hdrCksum != 0)
                correct = (hdrCksum == m_pe.PE.CalculatedCheckSum) ? "Correct" : "INCORRECT";
            LogLineFormat("Header's Checksum: 0x{0:X08}   Actual Checksum: 0x{1:X08}  {2}",
                hdrCksum, m_pe.PE.CalculatedCheckSum, correct);

            uint extraLength;
            uint extraStart;
            if (PEHelper.DetectExtraData(m_pe, out extraStart, out extraLength)) {
                LogLineFormat("EOF Extra Data: @ 0x{0:X}  (0x{1:X} bytes)", extraStart, extraLength);
            }

            UpdateGuiState();
        }

        private void CloseFile() {
            if (m_pe == null) return;

            m_pe.Dispose();
            m_pe = null;

            OnFileClosed();
        }

        private void OnFileClosed() {
            LogLine("File closed.");

            UpdateGuiState();
        }

        

        private void UpdateGuiState() {
            mnuFileClose.Enabled = m_pe != null;
            tsbClose.Enabled = m_pe != null;

            foreach (var viewer in m_peFileViewers) {
                viewer.PEFile = m_pe;
            }
        }

        #endregion



        
    }



}
