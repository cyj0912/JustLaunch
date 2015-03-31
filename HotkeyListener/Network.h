#pragma once
#include "Common.h"
#include <thread>
#include <mutex>
using namespace std;

class SocketServer
{
public:
	SocketServer();
	~SocketServer();
	void StartRunning();
	void Stop();
	bool IsRunning();

protected:
	static void ThreadWorker(SocketServer* aServer);

private:
	thread WorkingThread;
	bool bRunning;
};
