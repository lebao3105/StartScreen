using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public static Home Instance;
        TileBackend tile = new TileBackend();

        public Home()
        {
            Instance = this;
            InitializeComponent();
            
            // User name
            username.Content = Environment.UserName;

            // User avatar
            profilePicture.Fill = new ImageBrush(Utils.GetUserimage());

            AllApps_Button.Style = Assets.Styles.circleButtonStyle;

            beginTilesInit();
            MainWindow.Instance.counter2.Tick += new EventHandler(MainWindow.Instance.windowAnim2);
            MainWindow.Instance.counter2.Interval = new TimeSpan(0, 0, 0, 0, 2);
        }

        public void beginTilesInit()
        {
            Logger.info("Initializing Tiles");
            tile.initDefaultTiles();
            
            TileList.Items.Clear();
            
            foreach (TileBackend.tileData data in tile.data)
            {
                Logger.info("Adding " + data.name + " to tile list");
                Tile tile;
                Style tileStyle = data.Size switch
                {
                    TileBackend.tileSize.rsmall => Assets.Styles.SmallerTileStyle,
                    TileBackend.tileSize.small => Assets.Styles.SmallTileStyle,
                    TileBackend.tileSize.wide => Assets.Styles.WideTileStyle,
                    TileBackend.tileSize.large => Assets.Styles.LargeTileStyle
                };

                if (data.name == "startScreen[specialTiles(desktop)];")
                {
                    var bck = new ImageBrush(Utils.BitmapFromUri(new Uri(Utils.getWallpaperPath())))
                    {
                        Stretch = Stretch.UniformToFill
                    };
                    tile = new Tile
                    {
                        Content = "Desktop",
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        Background = bck,
                        Style = tileStyle
                    };
                    TileList.Items.Add(tile);
                    tile.Click += hideDesktopTile_Click;
                }
                else
                {
                    tile = new Tile
                    {
                        Content = data.name,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        Style = tileStyle
                    };
                    TileList.Items.Add(tile);
                    tile.Click += Tile_Click;
                }
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            foreach (TileBackend.tileData data in tile.data)
            {
                if (sender is Tile)
                {
                    if ((sender as Tile).Content == data.name)
                    {
                        Logger.info("Executing " + data.programPath);
                        try
                        {
                            Process process = new Process();
                            process.StartInfo.UseShellExecute = true;
                            process.StartInfo.FileName = data.programPath;
                            process.Start();
                        }
                        catch (Exception ex)
                        {
                            Logger.info("An error occurred while trying to run " + data.programPath);
                            Logger.info(ex.ToString());
                        }
                    }
                }
            }
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
                Environment.Exit(0); //by oliik2013
            }
            catch
            {

            }
        }
        private void AllApps_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.Navigate(MainWindow.Instance.allApps, new EntranceNavigationTransitionInfo());
        }

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void Search_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void userButton_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ms-settings:yourinfo",
                UseShellExecute = true
            };
            Process.Start(psi);
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
    }

}
