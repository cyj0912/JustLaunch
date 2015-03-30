#include "Common.h"
#include "Network.h"
#include <WinSock2.h>
#include <ws2tcpip.h>
#pragma comment(lib, "Ws2_32.lib")
#include <iostream>

SocketServer::SocketServer()
{

}

SocketServer::~SocketServer()
{

}

void SocketServer::StartRunning()
{
	WorkingThread = thread(SocketServer::ThreadWorker);
}

void SocketServer::Stop()
{
	WorkingThread.join();
}

void SocketServer::ThreadWorker()
{
	return;

	int iRet;
	WSADATA wsaData;
	iRet = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iRet != 0)
		return;

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
		return;
	}
	SOCKET ListenSocket = INVALID_SOCKET;
	ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	if (ListenSocket == INVALID_SOCKET)
	{
		freeaddrinfo(result);
		closesocket(ListenSocket);
		WSACleanup();
		return;
	}
	iRet = ::bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
	if (iRet != 0)
	{
		freeaddrinfo(result);
		closesocket(ListenSocket);
		WSACleanup();
		return;
	}
	freeaddrinfo(result);

	if (listen(ListenSocket, SOMAXCONN) == SOCKET_ERROR)
	{
		closesocket(ListenSocket);
		WSACleanup();
		return;
	}
	for (;;)
	{
		SOCKET ClientSocket = INVALID_SOCKET;
		ClientSocket = accept(ListenSocket, NULL, NULL);
		if (ClientSocket == INVALID_SOCKET)
		{
			closesocket(ListenSocket);
			WSACleanup();
			return;
		}
		closesocket(ClientSocket);
	}
}
