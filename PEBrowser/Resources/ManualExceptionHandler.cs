using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PEBrowser.Resources
{
    internal static class ManualExceptionHandler
    {
        public static void HandleException(string during, Exception exception) {
            var msg = String.Format("Unhandled exception during {0}:\n\n{1}", during, exception);
            MessageBox.Show(msg, "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}
