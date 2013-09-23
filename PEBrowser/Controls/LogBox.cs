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

        public bool NewestOnTop { get; set; }

        public void Clear() {
            textBoxLog.Clear();
        }

        public void LogLine(string line) {
            line = String.Format("{0:H:mm:ss}: {1}", DateTime.Now, line) + Environment.NewLine;

            if (NewestOnTop) {
                textBoxLog.Text = line + textBoxLog.Text;    
            }
            else {
                textBoxLog.Text += line;
            }

        }

        public void LogLineFormat(string format, params object[] args) {
            LogLine(String.Format(format, args));
        }
    }
}
