using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEBrowser
{
    interface IPEFileViewer
    {
        OpenPEFile PEFile { get; set; }
    }
}
