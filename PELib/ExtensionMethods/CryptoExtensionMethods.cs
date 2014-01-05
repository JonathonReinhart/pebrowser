using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace PELib.ExtensionMethods
{
    public static class CryptoExtensionMethods
    {
        public static DateTime? GetSigningTime(this SignerInfo signerInfo) {
            if (signerInfo.CounterSignerInfos.Count == 0)
                return null;
            var csi = signerInfo.CounterSignerInfos[0];

            var st = csi.SignedAttributes
                .Cast<CryptographicAttributeObject>()
                .Select(x => x.Values[0])
                .OfType<Pkcs9SigningTime>()
                .FirstOrDefault();

            if (st == null)
                return null;

            return st.SigningTime;
        }
    }
}
