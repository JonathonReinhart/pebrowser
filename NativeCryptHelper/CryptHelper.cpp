// This is the main DLL file.

#include "stdafx.h"

#include "CryptHelper.h"

// Link with the Crypt32.lib file.
#pragma comment (lib, "Crypt32")
#pragma comment (lib, "Cryptui")



static BOOL _ShowSignerInfo(HWND hWnd, HCRYPTMSG hMsg) {
	BOOL res;
	DWORD cb;
	CMSG_SIGNER_INFO* signerinfo;
	
	// Get the Signer Info

	if (!CryptMsgGetParam(
		hMsg,                           // hCryptMsg [in]
		CMSG_SIGNER_INFO_PARAM,         // dwParamType [in]
		0,                              // dwIndex [in]
		NULL,                           // pvData [out]
		&cb                             // pcbData [in, out]
		))	return FALSE;

	if (! (signerinfo = (CMSG_SIGNER_INFO*)LocalAlloc(LPTR, cb)) )
		return FALSE;

	if (!CryptMsgGetParam(
		hMsg,                           // hCryptMsg [in]
		CMSG_SIGNER_INFO_PARAM,         // dwParamType [in]
		0,                              // dwIndex [in]
		signerinfo,                      // pvData [out]
		&cb                             // pcbData [in, out]
		))	return FALSE;


	// Initialize struct parameter
	CRYPTUI_VIEWSIGNERINFO_STRUCT vsi;
	memset(&vsi, 0, sizeof(vsi));

	vsi.dwSize = sizeof(vsi);
	vsi.hwndParent = hWnd;
	vsi.dwFlags = 0;	// SHDocvw.dll passes 0x14
	vsi.szTitle = NULL;
	vsi.pSignerInfo = signerinfo;
	vsi.hMsg = hMsg;
	vsi.pszOID = "1.3.6.1.5.5.7.3.3";	// XCN_OID_PKIX_KP_CODE_SIGNING


	// Show the dialog!
	res = CryptUIDlgViewSignerInfo(&vsi);

	// Free resources
	LocalFree(signerinfo);
	return res;
}



namespace CryptHelper {

	
void CryptUI::ShowSignerInfo(array<Byte>^ data) {
	CryptUI::ShowSignerInfo(data, IntPtr::Zero);
}

void CryptUI::ShowSignerInfo(array<Byte>^ data, IntPtr hWnd) {
	pin_ptr<Byte> pData = &data[0];			// http://stackoverflow.com/questions/17689154
	HWND _hWnd = (HWND)hWnd.ToPointer();	// http://stackoverflow.com/a/14334609/119527

	CERT_BLOB blob;
	blob.cbData = data->Length;
	blob.pbData = pData;


	DWORD MsgAndCertEncodingType;
	DWORD ContentType;
	DWORD FormatType;
	HCERTSTORE hCertStore;
	HCRYPTMSG hMsg;

	BOOL res;

	res = CryptQueryObject(
		CERT_QUERY_OBJECT_BLOB,         // dwObjectType [in]
		&blob,                          // pvObject [in]
		CERT_QUERY_CONTENT_FLAG_ALL,    // dwExpectedContentTypeFlags [in]
		CERT_QUERY_FORMAT_FLAG_BINARY,  // dwExpectedFormatTypeFlags [in]
		0,                              // dwFlags [in]

		&MsgAndCertEncodingType,        // pdwMsgAndCertEncodingType [out]
		&ContentType,                   // pdwContentType [out]
		&FormatType,                    // pdwFormatType [out]
		&hCertStore,                    // phCertStore [out]
		&hMsg,                          // phMsg [out]
		NULL                            // ppvContext [out]
		);


	_ShowSignerInfo( (HWND)hWnd.ToPointer(), hMsg);


	if (hCertStore)		CertCloseStore(hCertStore, 0);
	if (hMsg)			CryptMsgClose(hMsg);
}

}