#pragma once

#include "Stdafx.h"

#ifdef __cplusplus
extern "C" {
#endif

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

