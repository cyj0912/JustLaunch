#include "Common.h"
#include "NetCommand.h"

bool NetCommand::Identify()
{
	if (Id == ThisId)
		return true;
	else
		return false;
}

void NC_NEW_HKEY::Visit()
{

}

void NC_HKEY_PRESSED::Visit()
{

}

void NC_HKEY_RELEASED::Visit()
{

}
