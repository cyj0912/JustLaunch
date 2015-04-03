using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustLaunch
{
    class HostSupport
    {
        public static void LaunchHotkeyListener()
        {
            //Hard-coded path
            //String ListenerPath = "D:\\Dev15a\\JustLaunch\\Debug\\HotkeyListener.exe";
            String ListenerPath = "HotkeyListener.exe";
            Process procLaunching = new Process();
            procLaunching.StartInfo.FileName = ListenerPath;
            procLaunching.StartInfo.Arguments = "";
            procLaunching.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            procLaunching.Start();
        }

        public static bool IsListenerRunning()
        {
            Process[] processlist = Process.GetProcesses();
            foreach(Process proc in processlist)
            {
                if (proc.ProcessName.Contains("HotkeyListener"))
                    return true;
            }
            return false;
        }

        public static void KillListener()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process proc in processlist)
            {
                if (proc.ProcessName.Contains("HotkeyListener"))
                {
                    proc.Kill();
                }
            }
        }
    }
}
