#include "Common.h"
#include "Network.h"
#include <WinSock2.h>
#include <ws2tcpip.h>
#pragma comment(lib, "Ws2_32.lib")
#include <iostream>

SocketServer::SocketServer()
{
	bRunning = false;
}

SocketServer::~SocketServer()
{

}

void SocketServer::StartRunning()
{
	WorkingThread = thread(SocketServer::ThreadWorker, this);
}

void SocketServer::Stop()
{
	WorkingThread.join();
}

bool SocketServer::IsRunning()
{
	return bRunning;
}

void SocketServer::SendCmd(SimpleCommand& cmd)
{
	lock_guard<mutex> lock(OutCmdQueueMutex);
	OutCmdQueue.push_back(cmd);
}

void SocketServer::ThreadWorker(SocketServer* aServer)
{
	aServer->bRunning = true;

	int iRet;
	WSADATA wsaData;
	iRet = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iRet != 0)
		goto CLEANUP;

	struct addrinfo *result = NULL, *ptr = NULL, hints;
	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;
	hints.ai_flags = AI_PASSIVE;
	iRet = getaddrinfo(NULL, "20015", &hints, &result);
	if (iRet != 0)
	{
		WSACleanup();
		goto CLEANUP;
	}
	SOCKET ListenSocket = INVALID_SOCKET;
	ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	if (ListenSocket == INVALID_SOCKET)
	{
		freeaddrinfo(result);
		closesocket(ListenSocket);
		WSACleanup();
		goto CLEANUP;
	}
	iRet = ::bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
	if (iRet != 0)
	{
		freeaddrinfo(result);
		closesocket(ListenSocket);
		WSACleanup();
		goto CLEANUP;
	}
	freeaddrinfo(result);

	if (listen(ListenSocket, SOMAXCONN) == SOCKET_ERROR)
	{
		closesocket(ListenSocket);
		WSACleanup();
		goto CLEANUP;
	}

	for (;;)
	{
		SOCKET ClientSocket = INVALID_SOCKET;
		ClientSocket = accept(ListenSocket, NULL, NULL);
		if (ClientSocket == INVALID_SOCKET)
		{
			closesocket(ListenSocket);
			WSACleanup();
			goto CLEANUP;
		}
		while (1)
		{
			Sleep(10);
			aServer->OutCmdQueueMutex.lock();
			for (vector<SimpleCommand>::iterator iter = aServer->OutCmdQueue.begin(); iter != aServer->OutCmdQueue.end();
				iter++)
			{
				cout << hex << *(int*)&(*iter) << " ";
				cout << hex << *((int*)&(*iter) + 1) << endl;
				int res = send(ClientSocket, (const char*)&(*iter), sizeof(SimpleCommand), 0);
				cout << res << endl;
			}
			aServer->OutCmdQueue.clear();
			aServer->OutCmdQueueMutex.unlock();
		}
		closesocket(ClientSocket);
	}

CLEANUP:
	aServer->bRunning = false;
}
