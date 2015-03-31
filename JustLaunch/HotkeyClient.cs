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
            //IPEndPoint LocalIPEP = new IPEndPoint(IPAddress.Loopback, 0);
            //ClientSocket = new Socket(LocalIPEP.AddressFamily, SocketType.Stream, ProtocolType.Udp);
            return false;
        }

        public bool IsRunning()
        {
            return Running;
        }
    }
}
