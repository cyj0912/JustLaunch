#pragma once
#include "Common.h"
#include <vector>
using namespace std;

#include "Hotkey.h"
class HotkeyProcessor
{
public:
	HotkeyProcessor(unsigned int aNumOfKeys);
	~HotkeyProcessor();
	void Setup(bool aInitialState[], unsigned int aSize);

	// Returns whether the hot key blocks next hook or not
	bool ProcessKeyEvent(unsigned int aKeyCode, bool aIsDownOrUp);
	void RegisterHotkey(const Hotkey& aHotkey);

protected:
	struct HotkeyState
	{
		bool Active = false;
		bool LastActive = false;
	};
	unsigned int KeyStateSize;
	bool *KeyState;
	bool *KeyAssociated;
	vector<pair<Hotkey, HotkeyState>> HotkeyList;
};
