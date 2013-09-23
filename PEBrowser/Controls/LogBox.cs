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
    public partial class LogBox : UserControl
    {
        public LogBox()
        {
            InitializeComponent();
        }

        public void Clear() {
            textBoxLog.Clear();
        }

        public void LogLine(string line) {
            line = String.Format("{0:H:mm:ss}: {1}", DateTime.Now, line) + Environment.NewLine;
            textBoxLog.Text = line + textBoxLog.Text;
        }

        public void LogLineFormat(string format, params object[] args) {
            LogLine(String.Format(format, args));
        }
    }
}
