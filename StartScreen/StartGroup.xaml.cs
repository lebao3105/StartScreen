using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for StartGroup.xaml
    /// </summary>
    public partial class StartGroup : UserControl
    {
        private readonly List<TileBackend.tileData> tiles;

        public StartGroup(string groupName, List<TileBackend.tileData> tiles)
        {
            InitializeComponent();
            GroupName.Content = groupName;
            this.tiles = tiles;

            //if (!ItemsList.Items.IsEmpty)
            //{
            //    ItemsList.Items.Clear();
            //}

            GroupName.Content = groupName;

            for (int i = 0; i <= tiles.Max((item) => item.tilePosX); i++)
            {
                ItemsGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i <= tiles.Max((item) => item.tilePosY); i++)
            {
                ItemsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            foreach (var item in tiles)
            {
                try
                {
                    if (ItemsGrid.Children
                                 .Cast<UIElement>()
                                 .First((e) => Grid.GetColumn(e) == item.tilePosX && Grid.GetRow(e) == item.tilePosY)
                        != null)
                    {
                        throw new NotImplementedException("Found tile with the same position as another one:");
                    }
                }
                catch
                {
                    // Just ignore this.
                }

                Style tileStyle = item.Size switch
                {
                    TileBackend.tileSize.rsmall => Assets.Styles.SmallerTileStyle,
                    TileBackend.tileSize.small => Assets.Styles.SmallTileStyle,
                    TileBackend.tileSize.wide => Assets.Styles.WideTileStyle,
                    TileBackend.tileSize.large => Assets.Styles.LargeTileStyle
                };

                Tile tile;

                if (item.name == "startScreen[specialTiles(desktop)];")
                {
                    tile = new Tile
                    {
                        Content = "Desktop",
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        Background = new ImageBrush(Utils.BitmapFromUri(new Uri(Utils.getWallpaperPath())))
                        {
                            Stretch = Stretch.UniformToFill
                        },
                        Style = tileStyle
                    };
                    tile.Click += hideDesktopTile_Click;
                }
                else
                {
                    tile = new Tile
                    {
                        Content = item.name,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        Style = tileStyle,
                    };
                    tile.Click += Tile_Click;
                }

                //ItemsGrid.RowDefinitions.ElementAt(item.tilePosY).Height = new GridLength(tile.Height);
                //ItemsGrid.ColumnDefinitions.ElementAt(item.tilePosX).Width = new GridLength(tile.Width);

                Grid.SetColumn(tile, item.tilePosX);
                Grid.SetRow(tile, item.tilePosY);
                ItemsGrid.Children.Add(tile);
            }
        }
        private void Tile_Click(object sender, RoutedEventArgs e)
        {
        }

        private void hideDesktopTile_Click(object sender, RoutedEventArgs e)
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

        private void Enter_GroupDetails(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.Navigate(
                new StartGroupDetails(GroupName.Content.ToString(), tiles)
            );
        }
    }
}
