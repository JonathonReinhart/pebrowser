using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PELib;

namespace PEBrowser.Forms
{
    public partial class MainForm : Form
    {
        private OpenPEFile m_pe;

        public MainForm()
        {
            InitializeComponent();

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

            var filter = new FileDialogFilterBuilder();
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

            peHeaderControl.FileOpened(m_pe);

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
        }

        #endregion



        
    }



}
