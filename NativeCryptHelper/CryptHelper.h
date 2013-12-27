// NativeCryptHelper.h

#pragma once

using namespace System;

namespace CryptHelper {

	public ref class CryptUI
	{
	public:
		static void ShowSignerInfo(array<Byte>^ data, IntPtr hWnd);
		static void ShowSignerInfo(array<Byte>^ data);
	};

}
