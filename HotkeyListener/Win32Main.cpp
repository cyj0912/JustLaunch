#include "Common.h"
#include "HotkeyProcessor.h"
#include "Network.h"
#include "NetCommand.h"
#include <Windows.h>
#include <iostream>
#include <fstream>
#include <vector>
using namespace std;

LRESULT CALLBACK KeyHookProc(int code, WPARAM wParam, LPARAM lParam)
{
	if (code < 0)
		return CallNextHookEx(NULL, code, wParam, lParam);
	bool CallNext = true;
	bool DownOrUp = false;
	if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
		DownOrUp = true;
	KBDLLHOOKSTRUCT *pKBHookStruct = (KBDLLHOOKSTRUCT*)lParam;
	CallNext = !gHkProcessor->ProcessKeyEvent(pKBHookStruct->vkCode, DownOrUp);
	return CallNext ? CallNextHookEx(NULL, code, wParam, lParam) : 1;
}

//int main(void)
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
#ifdef __STDC_WANT_SECURE_LIB__ // For the Microsoft CRT
	FILE* LogFile;
	freopen_s(&LogFile, "HL.log", "w", stdout);
#else
	freopen("HL.log", "w", stdout);
#endif
	HHOOK hKeyHook;
	hKeyHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyHookProc, GetModuleHandle(NULL), 0);
	if (!hKeyHook)
		return -1;
	UnhookWindowsHookEx(hKeyHook);
	HotkeyProcessor HkProc(255);
	gHkProcessor = &HkProc;
	BYTE SysKeyboardState[255];
	bool KeyboardState[255];
	GetKeyboardState(SysKeyboardState);
	for (int i = 0; i <= 254; i++)
	{
		KeyboardState[i] = ((SysKeyboardState[i] & 0x80) == 1);
	}
	HkProc.Setup(KeyboardState, 255);
	//Hotkey TestHotkey1;
	//TestHotkey1.Block = true;
	//TestHotkey1.Combination.push_back(VK_LWIN);
	//HkProc.RegisterHotkey(TestHotkey1);
	SocketServer Server;
	gServer = &Server;
	Server.StartRunning();
	MSG msg;
	BOOL bRet;
	while (bRet = GetMessage(&msg, NULL, NULL, NULL))
	{
		if (bRet == FALSE)
			goto CLEANUP;
		DispatchMessage(&msg);
		if (!Server.IsRunning())
			goto CLEANUP;
	}
CLEANUP:
	Server.Stop();
	UnhookWindowsHookEx(hKeyHook);
	return 0;
}
