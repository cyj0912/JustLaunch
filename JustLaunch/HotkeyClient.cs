using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace JustLaunch
{
    class HotkeyClient
    {
        Socket ClientSocket;
        bool Running;

        public HotkeyClient()
        {
            Running = false;
        }

        public bool TryToConnect()
        {
            IPEndPoint LocalIPEP = new IPEndPoint(IPAddress.Loopback, 20015);
            ClientSocket = new Socket(LocalIPEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ClientSocket.Connect(LocalIPEP);
                if (ClientSocket.Connected)
                {
                    Running = true;
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool IsRunning()
        {
            return Running;
        }

        byte[] RecvBuffer = new byte[8];
        bool IsBuffering = false;
        public void TryBuffering()
        {
            if (!IsBuffering)
            {
                ClientSocket.BeginReceive(RecvBuffer, 0, 8, 0, ClientSocket_Received, null);
                IsBuffering = true;
            }
        }

        public void ClientSocket_Received(IAsyncResult ar)
        {
            Console.WriteLine("Received " + RecvBuffer[0] + RecvBuffer[1] + RecvBuffer[2] + RecvBuffer[3] + RecvBuffer[4]);
            IsBuffering = false;
        }

        public bool IsThereNewMessage()
        {
            return false;
        }
    }
}
