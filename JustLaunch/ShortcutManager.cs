using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustLaunch
{
    class ShortcutManager
    {
        // TODO: Fix the ugly design
        public Dictionary<String, Shortcut> Storage;
        private String StoragePath;

        public ShortcutManager(bool UsaDefaultSettingFile)
        {
            if (UsaDefaultSettingFile)
            {
                String AppDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                StoragePath = AppDataLocal + "\\JustLaunch\\JustLaunch\\Shortcut.json";
                Console.WriteLine(StoragePath);
                Storage = new Dictionary<String, Shortcut>();
                LoadFromFile(StoragePath);
            }
            else
            {
                Storage = new Dictionary<String, Shortcut>();
                Storage["1"] = new Shortcut("D:\\Dev14d\\cmake\\bin\\cmake-gui.exe");
                Storage["2"] = new Shortcut("D:\\msys64\\msys2_shell.bat");
                Storage["3"] = new Shortcut("notepad.exe");
            }
        }

        public ShortcutManager(String aFile)
        {
            Storage = new Dictionary<String, Shortcut>();
            StoragePath = aFile;
            LoadFromFile(StoragePath);
        }

        protected void LoadFromFile(String aFile)
        {
            // Make sure the directory exists
            System.IO.FileInfo file = new System.IO.FileInfo(aFile);
            file.Directory.Create();

            if(!file.Exists)
            {
                Storage["1"] = new Shortcut("C:\\Windows\\System32\\notepad.exe");
                Storage["2"] = new Shortcut("C:\\Windows\\System32\\taskmgr.exe");
                //Storage["1"] = new Shortcut("D:\\Dev14d\\cmake\\bin\\cmake-gui.exe");
                //Storage["2"] = new Shortcut("D:\\msys64\\msys2_shell.bat");
                //Storage["3"] = new Shortcut("notepad.exe");
                WriteToStorage();
                return;
            }

            String Buffer = System.IO.File.ReadAllText(StoragePath, Encoding.Default);
            Storage = Newtonsoft.Json.JsonConvert.DeserializeObject <Dictionary<String, Shortcut>>(Buffer);
        }

        public void WriteToStorage()
        {
            WriteToFile(StoragePath);
        }

        public void WriteToFile(String aFile)
        {
            String Buffer = Newtonsoft.Json.JsonConvert.SerializeObject(Storage);
            using(System.IO.StreamWriter Writer = new System.IO.StreamWriter(StoragePath))
            {
                Writer.Write(Buffer);
            }
        }

        public bool Launch(String aIdentifier)
        {
            if (!Storage.ContainsKey(aIdentifier))
                return false;
            Shortcut Running = Storage[aIdentifier];
            if (Running == null)
                return false;
            return Running.Launch();
        }
    }
}
