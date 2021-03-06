﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace JustLaunch
{
    [DataContract]
    class Shortcut
    {
        public Shortcut()
        {
            ExecutablePath = "";
        }

        public Shortcut(String aExecPath)
        {
            ExecutablePath = aExecPath;
        }

        //public Shortcut(Unserialize?)
        //{

        //}

        public bool Launch()
        {
            Process procLaunching = new Process();
            if (ExecutablePath == null || ExecutablePath == "")
                return false;
            procLaunching.StartInfo.FileName = ExecutablePath;
            procLaunching.StartInfo.Arguments = "";
            procLaunching.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            try
            {
                return procLaunching.Start();
            }
            catch
            {
                return false;
            }
        }

        protected bool StartProcess()
        {
            return true;
        }

        public String GetPath()
        {
            return ExecutablePath;
        }

        [DataMember]
        protected String ExecutablePath;
    }
}
