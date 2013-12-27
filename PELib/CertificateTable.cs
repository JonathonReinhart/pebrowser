using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using PELib.ExtensionMethods;

namespace PELib
{
    public class CertificateTable
    {
        public static CertificateTable Read(PeFile peFile, Stream stream, uint size) {
            var result = new CertificateTable();
            
            var startPos = stream.Position;

            var br = new BinaryReader(stream);

            while (stream.Position < startPos + size) {
                var entryOff = stream.Position;

                var dwLength = br.ReadUInt32();     // length of bCertificate
                var wRevision = br.ReadUInt16();    // cert version number
                var wCertificateType = br.ReadUInt16();

                


                // Subsequent entries are accessed by advancing that entry’s
                // dwLength bytes, rounded up to an 8-byte multiple, from the
                // start of the current attribute certificate entry.
                stream.Position = entryOff + dwLength;
                stream.SeekAlign(8, startPos);
            }

            return result;
        }

    }

    public class CertificateTableEntry
    {
        
    }
}
