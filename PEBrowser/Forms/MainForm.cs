using System.Windows.Forms;

namespace PEBrowser.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            LogLine("Started up!");
            LogLine("Still going");
            LogLineFormat("We've done {0} messages now!", 3);
        }

        #region Logging
        public void LogLine(string line) {
            logBox.LogLine(line);
        }
        
        public void LogLineFormat(string format, params object[] args) {
            logBox.LogLineFormat(format, args);
        }
        #endregion
    }
}
