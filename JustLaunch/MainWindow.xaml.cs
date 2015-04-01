using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace JustLaunch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShortcutManager ShortcutMgr;
        HotkeyClient NetHotkey;
        //System.Collections.Concurrent.ConcurrentQueue<bool> Notifications;

        public MainWindow()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            Topmost = true;
            //Notifications = new System.Collections.Concurrent.ConcurrentQueue<bool>();
            ShortcutMgr = new ShortcutManager(true);

            System.Windows.Forms.NotifyIcon TrayIcon = new NotifyIcon();
            Stream IconStream = 
                System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Tray.ico")).Stream;
            TrayIcon.Icon = new System.Drawing.Icon(IconStream);
            TrayIcon.Visible = true;
            TrayIcon.DoubleClick += TrayIcon_DoubleClick;
        }

        void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            HostSupport.KillListener();
            Close();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Hide();
            NotificationWindow InitWindow = new NotificationWindow();
            InitWindow.Show();
            NetHotkey = new HotkeyClient();
            bool KillSwitch = false;
            int Attempts = 0;
            while (!NetHotkey.TryToConnect() && Attempts < 4)
            {
                if (!HostSupport.IsListenerRunning())
                {
                    HostSupport.LaunchHotkeyListener();
                    Thread.Sleep(1000);
                }
                else if (!KillSwitch)
                {
                    Thread.Sleep(1000);
                    KillSwitch = !KillSwitch;
                }
                else
                {
                    HostSupport.KillListener();
                    HostSupport.LaunchHotkeyListener();
                    Thread.Sleep(1000);
                    KillSwitch = !KillSwitch;
                }
                Attempts++;
            }
            if (Attempts >= 4)
            {
                InitWindow.Close();
                HostSupport.KillListener();
                System.Windows.MessageBox.Show("Unable to connect to the hotkey server.", "Error");
                this.Close();
            }
            System.Windows.Threading.DispatcherTimer NetTimer = new System.Windows.Threading.DispatcherTimer();
            NetTimer.Tick += NetTimer_Tick;
            NetTimer.Interval = new TimeSpan(10);
            NetTimer.Start();
            InitWindow.Close();
            Console.WriteLine("Initialized!");
        }

        private void NetTimer_Tick(object sender, EventArgs e)
        {
            if(!NetHotkey.IsRunning())
            {
                return;
            }
            NetHotkey.TryBuffering();
        }

        int CurrentSelection = -1;

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point p = e.GetPosition(this);
            double px = p.X - 175;
            double py = 175 - p.Y;
            double distFromCenter = Math.Sqrt(px * px + py * py);
            if(distFromCenter <= 68)
            {
                CurrentSelection = -1;
                arcInner.ArcThickness = 0;
                arcIndicator.ArcThickness = 0;
                return;
            }
            arcInner.ArcThickness = 15;
            arcIndicator.ArcThickness = 92;

            double angle = Math.Atan2(py, px);
            arcInner.StartAngle = -(angle / 3.141592654 * 180 - 90) - 30;
            arcInner.EndAngle = -(angle / 3.141592654 * 180 - 90) + 30;

            double angleArcDeg = -(angle / 3.141592654 * 180 - 90);
            if (angleArcDeg < 0)
                angleArcDeg += 360;
            int seg = 1;
            while (angleArcDeg > seg * 60)
                seg++;

            arcIndicator.StartAngle = seg * 60 - 60;
            arcIndicator.EndAngle = seg * 60;

            CurrentSelection = seg;
        }

        private void Window_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // GetCurrentShortcut();
            if (CurrentSelection == -1)
                return;
            ShortcutMgr.Launch(CurrentSelection.ToString());
        }

        private void Panel_Show()
        {
            CurrentSelection = -1;
            arcInner.ArcThickness = 0;
            arcIndicator.ArcThickness = 0;

            PresentationSource pSource = PresentationSource.FromVisual(this);
            System.Drawing.Point mousePos = Control.MousePosition;
            Left = mousePos.X / pSource.CompositionTarget.TransformToDevice.M11 - Width / 2;
            Top = mousePos.Y / pSource.CompositionTarget.TransformToDevice.M22 - Height / 2;
            Show();
        }

        private void Panel_Hide()
        {
            Hide();
        }

        //private void Window_ProcessNotifications()
        //{
        //    if (Notifications.Count > 0)
        //    {
        //        bool notiCur;
        //        while(!Notifications.TryDequeue(out notiCur));
        //        if (notiCur)
        //        {
        //            CurrentSelection = -1;
        //            arcInner.ArcThickness = 0;
        //            arcIndicator.ArcThickness = 0;

        //            PresentationSource pSource = PresentationSource.FromVisual(this);
        //            System.Drawing.Point mousePos = Control.MousePosition;
        //            Left = mousePos.X / pSource.CompositionTarget.TransformToDevice.M11 - Width / 2;
        //            Top = mousePos.Y / pSource.CompositionTarget.TransformToDevice.M22 - Height / 2;
        //            Show();
        //        }
        //        else
        //        {
        //            Hide();
        //        }
        //    }
        //}
    }
}
