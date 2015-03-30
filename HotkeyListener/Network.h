#pragma once
#include "Common.h"
#include <thread>
using namespace std;

class SocketServer
{
public:
	SocketServer();
	~SocketServer();
	void StartRunning();
	void Stop();

protected:
	static void ThreadWorker();

private:
	thread WorkingThread;
};
