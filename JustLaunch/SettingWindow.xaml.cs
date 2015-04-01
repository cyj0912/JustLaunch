using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JustLaunch
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// UGLY ---- Programed by a WPF noob
    /// </summary>
    public partial class SettingWindow : Window
    {
        public event EventHandler SettingsChanged;
        private ShortcutManager ShortcutMgr;

        public SettingWindow()
        {
            InitializeComponent();
            ReadSettings();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            WriteSettings();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            WriteSettings();
        }

        private void ReadSettings()
        {
            ShortcutMgr = new ShortcutManager(true);
            if (ShortcutMgr.Storage.ContainsKey("1"))
                SetApplicationIconFromPath(appBtn1, appImg1, appText1, ShortcutMgr.Storage["1"].GetPath());
            if (ShortcutMgr.Storage.ContainsKey("2"))
                SetApplicationIconFromPath(appBtn2, appImg2, appText2, ShortcutMgr.Storage["2"].GetPath());
            if (ShortcutMgr.Storage.ContainsKey("3"))
                SetApplicationIconFromPath(appBtn3, appImg3, appText3, ShortcutMgr.Storage["3"].GetPath());
            if (ShortcutMgr.Storage.ContainsKey("4"))
                SetApplicationIconFromPath(appBtn4, appImg4, appText4, ShortcutMgr.Storage["4"].GetPath());
            if (ShortcutMgr.Storage.ContainsKey("5"))
                SetApplicationIconFromPath(appBtn5, appImg5, appText5, ShortcutMgr.Storage["5"].GetPath());
            if (ShortcutMgr.Storage.ContainsKey("6"))
                SetApplicationIconFromPath(appBtn6, appImg6, appText6, ShortcutMgr.Storage["6"].GetPath());
        }

        private void WriteSettings()
        {
            ShortcutMgr.WriteToStorage();
            if (SettingsChanged != null)
                SettingsChanged(this, EventArgs.Empty);
        }

        private String PickApplication(Button btn, System.Windows.Controls.Image img, TextBlock text)
        {
            Microsoft.Win32.OpenFileDialog OFDialog = new Microsoft.Win32.OpenFileDialog();
            OFDialog.Multiselect = false;
            OFDialog.ShowDialog();
            if(System.IO.File.Exists(OFDialog.FileName))
                SetApplicationIconFromPath(btn, img, text, OFDialog.FileName);
            return OFDialog.FileName;
        }

        private void SetApplicationIconFromPath(Button btn, System.Windows.Controls.Image img, TextBlock text, String Path)
        {
            text.Text = System.IO.Path.GetFileNameWithoutExtension(Path);
            System.Drawing.Icon AppIcon = System.Drawing.Icon.ExtractAssociatedIcon(Path);
            Bitmap bitmap = AppIcon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap =
                 Imaging.CreateBitmapSourceFromHBitmap(
                      hBitmap, IntPtr.Zero, Int32Rect.Empty,
                      BitmapSizeOptions.FromEmptyOptions());
            img.Source = wpfBitmap;
        }

        private void SetApplicationIconEmpty(Button btn, System.Windows.Controls.Image img, TextBlock text)
        {
            BitmapImage CrossLogo = new BitmapImage();
            CrossLogo.BeginInit();
            CrossLogo.UriSource = new Uri("pack://application:,,,/Close.png");
            CrossLogo.EndInit();
            img.Source = CrossLogo;
            text.Text = "Empty";
        }

        private void appBtn1_Click(object sender, RoutedEventArgs e)
        {
            String ExecPath = PickApplication(appBtn1, appImg1, appText1);
            ShortcutMgr.Storage["1"] = new Shortcut(ExecPath);
        }

        private void appBtn2_Click(object sender, RoutedEventArgs e)
        {
            String ExecPath = PickApplication(appBtn2, appImg2, appText2);
            ShortcutMgr.Storage["2"] = new Shortcut(ExecPath);
        }

        private void appBtn3_Click(object sender, RoutedEventArgs e)
        {
            String ExecPath = PickApplication(appBtn3, appImg3, appText3);
            ShortcutMgr.Storage["3"] = new Shortcut(ExecPath);
        }

        private void appBtn4_Click(object sender, RoutedEventArgs e)
        {
            String ExecPath = PickApplication(appBtn4, appImg4, appText4);
            ShortcutMgr.Storage["4"] = new Shortcut(ExecPath);
        }

        private void appBtn5_Click(object sender, RoutedEventArgs e)
        {
            String ExecPath = PickApplication(appBtn5, appImg5, appText5);
            ShortcutMgr.Storage["5"] = new Shortcut(ExecPath);
        }

        private void appBtn6_Click(object sender, RoutedEventArgs e)
        {
            String ExecPath = PickApplication(appBtn6, appImg6, appText6);
            ShortcutMgr.Storage["6"] = new Shortcut(ExecPath);
        }
    }
}
