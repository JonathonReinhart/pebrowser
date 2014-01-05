using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPI.DirectoryServices;

namespace PELib.ExtensionMethods
{
    internal static class DNExtensionMethods
    {
        // http://www.codeproject.com/Articles/9788/An-RFC-2253-Compliant-Distinguished-Name-Parser
        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa366101.aspx
        // http://www.ietf.org/rfc/rfc2253.txt

        public static string GetRDNValue(this DN dn, string rdnName, string defaultValue) {
            var match = 
                from RDN rdn in dn.RDNs
                from RDNComponent comp in rdn.Components
                where comp.ComponentType == rdnName
                select comp.ComponentValue;
            return match.DefaultIfEmpty(defaultValue).FirstOrDefault();
        }

        public static string GetRDNValue(this DN dn, string rdnName) {
            return dn.GetRDNValue(rdnName, String.Empty);
        }
    }
}
