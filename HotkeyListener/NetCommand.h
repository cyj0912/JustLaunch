#pragma once
#include "Common.h"

struct NetCommand
{
	static const int PROTOCOL_VERSION = 0x01;
	bool Identify();
	virtual void Visit() = 0;
	int Id;
	static const int ThisId = -1;
};

struct NC_NEW_HKEY : public NetCommand
{
	virtual void Visit();
	static const int ThisId = 0x00;
};

struct NC_HKEY_PRESSED : public NetCommand
{
	virtual void Visit();
	static const int ThisId = 0x01;
};

struct NC_HKEY_RELEASED : public NetCommand
{
	virtual void Visit();
	static const int ThisId = 0x02;
};
