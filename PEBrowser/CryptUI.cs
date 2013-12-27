using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace PEBrowser
{
    internal static class CryptUI
    {

        public static void ShowSignerInfoDialog(this SignedCms cms) {
            ShowSignerInfoDialog(cms, IntPtr.Zero);
        }
        public static void ShowSignerInfoDialog(this SignedCms cms, IntPtr hWnd)
        {
            var vsi = new CRYPTUI_VIEWSIGNERINFO_STRUCT
            {
                dwSize = (uint)Marshal.SizeOf(typeof(CRYPTUI_VIEWSIGNERINFO_STRUCT)),
                pSignerInfo = (SafeHandle)cms.SignerInfos[0].GetPrivateField("m_pbCmsgSignerInfo"),
                hMsg = (SafeHandle)cms.GetPrivateField("m_safeCryptMsgHandle"),
                pszOID = "1.3.6.1.5.5.7.3.3",  // Code signing
                hwndParent = hWnd,
            };

            CryptUIDlgViewSignerInfo(ref vsi);
        }

        [DllImport("Cryptui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CryptUIDlgViewSignerInfo(ref CRYPTUI_VIEWSIGNERINFO_STRUCT pcvsi);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CRYPTUI_VIEWSIGNERINFO_STRUCT
    {
        public UInt32 dwSize;
        public IntPtr hwndParent;
        public UInt32 dwFlags;
        public string szTitle;
        public SafeHandle pSignerInfo;
        public SafeHandle hMsg;

        [MarshalAs(UnmanagedType.LPStr)]
        public string pszOID;
        public UInt32 dwReserved;
        public UInt32 cStores;
        public IntPtr rghStores;
        public UInt32 cPropSheetPages;
        public IntPtr rgPropSheetPages;
    }
}
