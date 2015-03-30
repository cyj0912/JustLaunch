#include "Common.h"
#include "HotkeyProcessor.h"

HotkeyProcessor::HotkeyProcessor(unsigned int aNumOfKeys)
{
	if (KeyStateSize = aNumOfKeys > 0)
	{
		KeyState = new bool[aNumOfKeys];
		KeyAssociated = new bool[aNumOfKeys];
		memset(KeyState, 0, aNumOfKeys * sizeof(bool));
		memset(KeyAssociated, 0, aNumOfKeys * sizeof(bool));
	}
	else
		KeyAssociated = KeyState = nullptr;
}

HotkeyProcessor::~HotkeyProcessor()
{
	delete[] KeyState;
	delete[] KeyAssociated;
}

void HotkeyProcessor::Setup(bool aInitialState[], unsigned int aSize)
{
	if (aSize > KeyStateSize)
		return;
	for (unsigned int i = 0; i < aSize; i++)
		KeyState[i] = aInitialState[i];
}

bool HotkeyProcessor::ProcessKeyEvent(unsigned int aKeyCode, bool aIsDownOrUp)
{
	KeyState[aKeyCode] = aIsDownOrUp;
	bool Ret = false;
	if (KeyAssociated[aKeyCode])
	{
		for (auto HKIter = HotkeyList.begin(); HKIter != HotkeyList.end(); HKIter++)
		{
			bool Success = true;
			bool ContainsCurrentKey = false;
			for (auto KeyIter = (*HKIter).first.Combination.begin(); KeyIter != (*HKIter).first.Combination.end(); KeyIter++)
			{
				if (*KeyIter == aKeyCode)
					ContainsCurrentKey = true;
				if (!KeyState[*KeyIter])
					Success = false;
			}
			(*HKIter).second.LastActive = (*HKIter).second.Active;
			(*HKIter).second.Active = Success;
			if (Success && ContainsCurrentKey && (*HKIter).first.Block)
				Ret = true;
		}
	}
	return Ret;
}

void HotkeyProcessor::RegisterHotkey(const Hotkey& aHotkey)
{
	HotkeyList.push_back(make_pair(aHotkey, HotkeyState()));
	for (auto iter = aHotkey.Combination.begin(); iter != aHotkey.Combination.end(); iter++)
	{
		KeyAssociated[*iter] = true;
	}
}
