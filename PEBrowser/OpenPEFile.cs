using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PELib;

namespace PEBrowser
{
    internal class OpenPEFile : IDisposable
    {
        public string Path { get; private set; }
        public PeFile PE { get; private set; }
        public Stream Stream { get; private set; }

        public long FileSize { get { return Stream.Length; } }

        private OpenPEFile() { }

        public static OpenPEFile Open(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var pe = new OpenPEFile()
            {
                Path = path,
                Stream = stream,
                PE = new PeFile(stream)
            };
            return pe;
        }

        public void Dispose()
        {
            Stream.Dispose();
        }

    }
}
