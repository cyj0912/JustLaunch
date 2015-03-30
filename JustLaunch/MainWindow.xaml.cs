using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        System.Collections.Concurrent.ConcurrentQueue<bool> Notifications;

        public MainWindow()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            Topmost = true;
            Notifications = new System.Collections.Concurrent.ConcurrentQueue<bool>();
            ShortcutMgr = new ShortcutManager(true);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Hide();
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

        private void Window_ProcessNotifications()
        {
            if (Notifications.Count > 0)
            {
                bool notiCur;
                while(!Notifications.TryDequeue(out notiCur));
                if (notiCur)
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
                else
                {
                    Hide();
                }
            }
        }
    }
}
