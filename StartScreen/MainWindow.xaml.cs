using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using ModernWpf.Media.Animation;
using System.Windows.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Security.RightsManagement;
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        public DispatcherTimer counter2 = new DispatcherTimer();
        DispatcherTimer counter = new DispatcherTimer();

        /// Is the window initialized?
        private bool initialized = false;

        /// <summary>
        /// Sets whether to use the user desktop wallpaper or not
        /// (before this will be used to draw the entire desktop with no windows)
        /// </summary>
        bool userBackgroundEnabled = true;

        public AllApps allApps;
        public Home homeScreen;

        public MainWindow()
        {
            alreadyShowing = true;
            
            InitializeComponent();
            Logger.info("Object initialized!");

            Instance = this;
            Logger.info("Instance is set to \"This\"!");

            Opacity = 0;
            Logger.info("Opacity has been set to 0");

#if !DEBUG
            Topmost = true;
            Logger.info("Window has been set to Top-Most");
            
#else   
            Topmost = false;
#endif

            ShowInTaskbar = false;
            Logger.info("Successfully hidden from taskbar");

            if (!initialized)
            {
                counter.Tick += new EventHandler(windowAnim);
                counter.Interval = new TimeSpan(0, 0, 0, 0, 2);
                counter.Start();
                Logger.info("windowAnim timer has initialized and started");
                
                //Loaded += MainWindow_Loaded;
                homeScreen = new Home();
                content.Navigate(homeScreen, new EntranceNavigationTransitionInfo());
                
                new Thread(() =>
                {
                    if (userBackgroundEnabled)
                    {
                        Logger.info("Getting user's desktop wallpaper");
                        imageBackground.Dispatcher.Invoke(() =>
                            imageBackground.Source = Utils.BitmapFromUri(new Uri(Utils.getWallpaperPath()))
                        );
                        Logger.info("Done getting user's desktop wallpaper");
                    }

                }).Start();

                imageBackground.Stretch = Stretch.UniformToFill;

                counter2.Tick += new EventHandler(MainWindow.Instance.windowAnim2);
                counter2.Interval = new TimeSpan(0, 0, 0, 0, 1);
                
                Logger.info("Listing All Apps");

                foreach (var app in KnownFolderHelper.FromKnownFolderId(new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}")))
                {
                    // The friendly app name
                    string name = app.Name;
                    
                    // The ParsingName property is the AppUserModelID
                    string appUserModelID = app.ParsingName; // or app.Properties.System.AppUserModel.ID
                    
                    Logger.info($"Name : {name} | ID : {appUserModelID}");
                    
                    BitmapSource icon = app.Thumbnail.BitmapSource;

                    appList.Add(new TileBackend.tileData
                    {
                        appIcon = icon,
                        name = name + "[" + appUserModelID
                    });

                    appListNameFriendly.Add(new TileBackend.tileData
                    {
                        appIcon = icon,
                        name = name,
                    });

                    Logger.info("Added Successfully");
                }
                initialized = true;
                allApps = new AllApps();
            }
        }
        public List<TileBackend.tileData> appList = new List<TileBackend.tileData>();
        public List<TileBackend.tileData> appListNameFriendly = new List<TileBackend.tileData>();

        public bool alreadyShowing = false;

        public void windowAnim2(object? sender, EventArgs e)
        {
            if (mainWindow.Opacity > 0.1)
            {
                mainWindow.Opacity -= 0.1;
            }
            else
            {
                counter2.Stop();
                //MainWindow.Instance.Hide();
                //GC.Collect();
                Environment.Exit(0);
            }
        }

        private void windowAnim(object? sender, EventArgs e)
        {
            if (mainWindow.Opacity < 1)
                mainWindow.Opacity += 0.1;
            else
                counter.Stop();
        }

        //private HwndSource hwndSource;
        //private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    return IntPtr.Zero;
        //}

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
        //    Logger.info("Adding HWND Hook for receiving messages");
        //    hwndSource.AddHook(new HwndSourceHook(WndProc));
        //}
        public void HideWindow()
        {
            Environment.Exit(0);
        }

        private void mainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.LWin || e.Key == Key.RWin) &&
                !e.Key.ToString().Contains("|")) // <- '|' operator
            {
                Environment.Exit(0);
            }
        }
    }
}
