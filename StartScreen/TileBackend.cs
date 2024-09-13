using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace StartScreen
{
    public sealed class TileBackend
    {
        public Dictionary<string, List<tileData>> data = new Dictionary<string, List<tileData>>();

        public void saveTile()
        {

        }

        public void loadAllTiles()
        {

        }

        public void getTile(tileData tileData)
        {

        }

        public void initDefaultTiles()
        {
            Logger.info("Initializing Default Tiles");
            data.Add(
                "Default",
                new List<tileData> {
                    new tileData
                    {
                        Size = tileSize.wide,
                        name = "startScreen[specialTiles(desktop)];",
                        programPath = "startScreen[hidefunc()];",
                        tilePosX = 0, tilePosY = 0,
                        group = "Default"
                    },
                }
            );
            data.Add(
                "Internet",
                new List<tileData> {
                    new tileData
                    {
                        Size = tileSize.wide,
                        name = "Internet Explorer",
                        programPath = "iexplore",
                        tilePosX = 0, tilePosY = 0,
                        group = "Internet"
                    }
                }
            );
        }

        // Tile Data JSON Structure
        public class tileData
        {
            public tileSize Size { get; set; }

            public string name { get; set; }

            public string programPath { get; set; }
            public int tilePosX { get; set; }

            public int tilePosY { get; set; }

            public string group { get; set; }

            public BitmapSource appIcon { get; set; }
        }

        // Tile size struct
        public enum tileSize
        {
            rsmall, // 0.5x0.5
            small, // 1x1
            wide, // 1x2
            large // 2x2
        }
    }
}
