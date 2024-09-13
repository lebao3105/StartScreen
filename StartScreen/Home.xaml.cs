using MahApps.Metro.Controls;
using ModernWpf.Media.Animation;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        TileBackend tile = new TileBackend();

        public Home()
        {
            InitializeComponent();

            tile.initDefaultTiles();

            // User name
            username.Content = $"{Environment.UserName}\n{Environment.UserDomainName}";

            // User avatar
            profilePicture.Fill = new ImageBrush(Utils.GetUserimage());

            AllApps_Button.Style = Assets.Styles.circleButtonStyle;

            beginTilesInit();
            //MainWindow.Instance.counter2.Tick += new EventHandler(MainWindow.Instance.windowAnim2);
            //MainWindow.Instance.counter2.Interval = new TimeSpan(0, 0, 0, 0, 2);
        }

        public void beginTilesInit()
        {
            Logger.info("Initializing Tiles");

            foreach (var pair in tile.data)
            {
                GroupList.Items.Add(new StartGroup(pair.Key, pair.Value));
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
        }

        private void hideDesktopTile_Click(object sender, RoutedEventArgs e)
        {
            closeAppAnim();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            closeAppAnim();
        }

        public static void closeAppAnim()
        {
            try
            {
                Logger.info("Hiding StartScreen!");
                MainWindow.Instance.alreadyShowing = false;
                Thread.Sleep(100);
                MainWindow.Instance.HideWindow();
            }
            catch
            {

            }
        }
        private void AllApps_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.Navigate(MainWindow.Instance.allApps, new EntranceNavigationTransitionInfo());
        }

        private void userButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "ms-settings:yourinfo",
                UseShellExecute = true
            });
        }

        private void powerAction_Click(object sender, RoutedEventArgs e)
        {
            powerAction.ContextMenu.IsOpen = true;
        }

        private void powerOff_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "-s -hybrid -t 000");
        }

        private void reboot_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "-r -t 000");
        }

        [DllImport("winuser.dll")]
        public static extern int ExitWindows(int one, int two);

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            ExitWindows(0, 0);
        }

        [DllImport("user32.dll")]
        public static extern int PostMessage(int h, int m, int w, int l);
        private void standby_Click(object sender, RoutedEventArgs e)
        {
            PostMessage(-1, 0x0112, 0xF170, 2);
        }
        public void ExecuteAsAdmin(string fileName, string args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        private void Unpin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Run_As_Admin_Click(object sender, RoutedEventArgs e)
        {
            //ExecuteAsAdmin(
            //    tile.data.First(
            //        (item) => item.name == (TileList.SelectedItem as Tile).Content
            //    ).programPath,
            //    ""
            //);
        }

        private void Resize_Large(object sender, RoutedEventArgs e)
        {
            //(TileList.SelectedItem as Tile).Style = Assets.Styles.LargeTileStyle;
        }

        private void Resize_Wide(object sender, RoutedEventArgs e)
        {
            //(TileList.SelectedItem as Tile).Style = Assets.Styles.WideTileStyle;
        }

        private void Resize_Small(object sender, RoutedEventArgs e)
        {
            //(TileList.SelectedItem as Tile).Style = Assets.Styles.SmallerTileStyle;
        }

        private void Resize_Normal(object sender, RoutedEventArgs e)
        {
            //(TileList.SelectedItem as Tile).Style = Assets.Styles.SmallTileStyle;
        }
    }

}
