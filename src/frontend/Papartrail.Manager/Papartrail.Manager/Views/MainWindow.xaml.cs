using HandyControl.Tools;
using Papartrail.Manager.Model.Consts;
using Prism.Regions;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Papartrail.Manager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        private const int WM_NCHITTEST = 0x0084;
        private const int HTCLIENT = 0x01;
        private const int HTCAPTION = 0x02;
        private IRegionManager RegionManager { get; set; }
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            SourceInitialized += MainWindow_SourceInitialized;
            this.RegionManager = regionManager;
        }
       
        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle).AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_NCHITTEST)
            {
                handled = true;
                return (IntPtr)HTCAPTION;
            }
            return IntPtr.Zero;
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            RegionManager.RequestNavigate(regionName: RegionNames.ContentRegion, "LoginView");
        }

    }
}
