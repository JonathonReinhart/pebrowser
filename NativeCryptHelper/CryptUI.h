#pragma once

#include "Stdafx.h"

#ifdef __cplusplus
extern "C" {
#endif

/////////////////////////////////////////////////////////
// The following were reverse-engineered from CryptUI.dll

#define CRYPTUI_PROVIDE_PRIVATE_DATA	(1<<31)		// dwReserved points to a CERT_VIEWSIGNERINFO_PRIVATE
#define CRYPTUI_SPECIFY_ERROR_MESSAGE	(1<<30)		// dwReserved is a system error message number

typedef struct tagCERT_VIEWSIGNERINFO_PRIVATE {
  CRYPT_PROVIDER_DATA  *pProvData;		// Populated by Wintrust!WTHelperProvDataFromStateData()
  DWORD                fpCryptProviderDataTrustedUsage;
  DWORD                idxSigner;
  DWORD                fCounterSigner;
  DWORD                idxCounterSigner;
  DWORD                messageId;
  DWORD                _unknown_1Ch;
} CERT_VIEWSIGNERINFO_PRIVATE;

// End reverse-engineered definitions
/////////////////////////////////////////////////////////


typedef struct tagCRYPTUI_VIEWSIGNERINFO_STRUCT {
  DWORD            dwSize;
  HWND             hwndParent;
  DWORD            dwFlags;
  LPCTSTR          szTitle;
  CMSG_SIGNER_INFO *pSignerInfo;
  HCRYPTMSG        hMsg;
  LPCSTR           pszOID;
  DWORD_PTR        dwReserved;
  DWORD            cStores;
  HCERTSTORE       *rghStores;
  DWORD            cPropSheetPages;
  LPCPROPSHEETPAGE rgPropSheetPages;
} CRYPTUI_VIEWSIGNERINFO_STRUCT, *PCRYPTUI_VIEWSIGNERINFO_STRUCT;



#ifdef UNICODE
#define CryptUIDlgViewSignerInfo CryptUIDlgViewSignerInfoW
#else
#define CryptUIDlgViewSignerInfo CryptUIDlgViewSignerInfoA
#endif

BOOL WINAPI CryptUIDlgViewSignerInfo(
  _In_  CRYPTUI_VIEWSIGNERINFO_STRUCT *pcvsi
);


#ifdef __cplusplus
}  // extern "C"
#endif

