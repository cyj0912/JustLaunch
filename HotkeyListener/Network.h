#pragma once
#include "Common.h"
#include "NetCommand.h"
#include <thread>
#include <mutex>
#include <vector>
using namespace std;

class SocketServer
{
public:
	SocketServer();
	~SocketServer();
	void StartRunning();
	void Stop();
	bool IsRunning();

	void SendCmd(SimpleCommand& cmd);

protected:
	static void ThreadWorker(SocketServer* aServer);

private:
	thread WorkingThread;
	bool bRunning;

	mutex OutCmdQueueMutex;
	vector<SimpleCommand> OutCmdQueue;
};
