﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Text;
using CPI.DirectoryServices;
using PELib.ExtensionMethods;


namespace PELib
{
    public class CertificateTable
    {
        private readonly List<CertificateTableEntry> m_entries = new List<CertificateTableEntry>(); 
        public IEnumerable<CertificateTableEntry> Entries { get { return m_entries; } }

        public static CertificateTable Read(PeFile peFile, Stream stream, uint size) {
            var result = new CertificateTable();
            
            var startPos = stream.Position;

            var br = new BinaryReader(stream);

            while (stream.Position < startPos + size) {
                var entryOff = stream.Position;

                var dwLength = br.ReadUInt32();     // length of bCertificate
                var wRevision = br.ReadUInt16();    // cert version number
                

                switch (wRevision) {
                    case WIN_CERT_REVISION_1_0:
                        Debug.WriteLine("WIN_CERT_REVISION_1_0 not supported");
                        break;

                    case WIN_CERT_REVISION_2_0:
                        var cert = CertificateTableEntry.Read(stream, (int)dwLength);
                        cert.Offset = entryOff;
                        result.m_entries.Add(cert);
                        break;

                    default:
                        Debug.WriteLine("Unknown WIN_CERT_REVISION", wRevision);
                        break;
                }

                // Subsequent entries are accessed by advancing that entry’s
                // dwLength bytes, rounded up to an 8-byte multiple, from the
                // start of the current attribute certificate entry.
                stream.Position = entryOff + dwLength;
                stream.SeekAlign(8, startPos);
            }

            return result;
        }


        #region Constants

        private const UInt16 WIN_CERT_REVISION_1_0 = 0x0100;
        private const UInt16 WIN_CERT_REVISION_2_0 = 0x0200;

        #endregion

    }




    public abstract class CertificateTableEntry
    {
        protected const string DefaultString = "Unavailable";

        public long Offset { get; internal set; }
        
        public abstract string Type { get; }
        public virtual string NameOfSigner { get { return DefaultString; } }
        public virtual string EmailAddress { get { return DefaultString; } }
        public virtual string Timestamp { get { return DefaultString; } }


        public static CertificateTableEntry Read(Stream stream, int length) {
            var br = new BinaryReader(stream);
            var wCertificateType = br.ReadUInt16();

            length -= (4 + 2 + 2);

            switch (wCertificateType)
            {
                case WIN_CERT_TYPE_X509:
                    return X509CertificateTableEntry.Read(stream, length);

                case WIN_CERT_TYPE_PKCS_SIGNED_DATA:
                    return PkcsSignedDataCertificateTableEntry.Read(stream, length);

                default:
                    Debug.WriteLine("Unknown/unsupported WIN_CERT_REVISION {0}", wCertificateType);
                    return null;
            }
        }

        #region Constants

        private const UInt16 WIN_CERT_TYPE_X509 = 0x0001;
        private const UInt16 WIN_CERT_TYPE_PKCS_SIGNED_DATA = 0x0002;
        private const UInt16 WIN_CERT_TYPE_RESERVED_1 = 0x0003;
        private const UInt16 WIN_CERT_TYPE_TS_STACK_SIGNED = 0x0004;

        #endregion
    }

    public sealed class X509CertificateTableEntry : CertificateTableEntry
    {
        public new static X509CertificateTableEntry Read(Stream stream, int length) {
            return new X509CertificateTableEntry();
        }

        public override string Type {
            get { return "X.509 Certificate"; }
        }
    }



    public sealed class PkcsSignedDataCertificateTableEntry : CertificateTableEntry
    {
        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa366101.aspx
        private const string DNCommonName = "CN";
        private const string DNEmail = "E";
        
        private readonly DN m_dnSubj;
        
        public SignedCms Cms { get; private set; }


        private PkcsSignedDataCertificateTableEntry(SignedCms cms) {
            Cms = cms;
            m_dnSubj = new DN(cms.SignerInfos[0].Certificate.Subject);
        }

        public new static PkcsSignedDataCertificateTableEntry Read(Stream stream, int length) {
            var br = new BinaryReader(stream);
            var b = br.ReadBytes(length);

            var cms = new SignedCms();
            cms.Decode(b);

            return new PkcsSignedDataCertificateTableEntry(cms);
        }

        public override string Type {
            get { return "PKCS#7 (Authenticode)"; }
        }

        public override string NameOfSigner
        {
            get { return m_dnSubj.GetRDNValue(DNCommonName, DefaultString); }
        }

        public override string EmailAddress
        {
            get {
                var si = Cms.SignerInfos[0];

                const string subjectAltName = "2.5.29.17";
                var ex = si.Certificate.Extensions[subjectAltName];
                if (ex != null) {
                    const int offset = 2 + 2;
                    return Encoding.UTF8.GetString(ex.RawData, offset, ex.RawData.Length - offset);
                }

                return m_dnSubj.GetRDNValue(DNEmail, DefaultString);
            }
        }

        public override string Timestamp
        {
            get {
                var st = Cms.SignerInfos[0].GetSigningTime();
                return st.HasValue ? st.Value.ToString() : DefaultString;
            }
        }
    }
}
