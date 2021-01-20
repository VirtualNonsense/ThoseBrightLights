using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
 using System.Collections.Generic;
 using System.Linq;

 namespace SE_Praktikum.Models
{
    public class TileSet
    {
        public int Rows;
        public int Columns;
        public int TileDimX;
        public int TileDimY;
        public int StartEntry;
        public Texture2D Texture;
        public int Tiles => Columns * Rows;

        private ILogger _logger;
        public bool HasHitBox => _hitBoxDict != null;
        private readonly List<TileInfo> tileSettings;
        private readonly Dictionary<int, Polygon[]> _hitBoxDict;

        // #############################################################################################################
        // constructor
        // #############################################################################################################
        public TileSet(Texture2D texture, int tileDimX, int tileDimY, Dictionary<int, Polygon[]> hitBoxDict, int startEntry = 0, List<TileInfo> tileSettings = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Texture = texture;
            TileDimX = tileDimX;
            TileDimY = tileDimY;
            Columns = Texture.Width / TileDimX;
            Rows = Texture.Height / TileDimY;
            StartEntry = startEntry;
            _hitBoxDict = hitBoxDict;
            this.tileSettings = tileSettings;
        }
        public TileSet(Texture2D texture, Polygon[] hitBox = null, int startEntry = 0)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Texture = texture;
            TileDimX = Texture.Width;
            TileDimY = Texture.Height;
            Columns = 1;
            Rows = 1;
            StartEntry = startEntry;
            if (hitBox == null) return;
            _hitBoxDict = new Dictionary<int, Polygon[]>
            {
                {0, hitBox}
            };
        }
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public int FrameCount => Columns * Rows;
        public int TextureWidth => Texture.Width;
        public int TextureHeight => Texture.Height;
        
        // #############################################################################################################
        // public methods 
        // #############################################################################################################
        public TileInfo GetInfo(int index)
        {
            if (tileSettings == null)
                return null;
            var info = tileSettings.Where(t => t.ID == index);
            if (info.Count() == 0)
                return null;
            return info.ElementAt(0);
        }

        /// <summary>
        /// Returns the hitbox of a given frame if available otherwise null 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Polygon[] GetHitBox(int index)
        {
            if (_hitBoxDict == null)
            {
                // logging is quite slow
                // _logger.Warn("Hitbox does not exist");
                return null;
            }
            if (_hitBoxDict.ContainsKey(index))
            {
                return _hitBoxDict[index].Select(polygon => (Polygon) polygon.Clone()).ToArray();
            }
            _logger.Warn($"hitbox of {index} ({index}) not found");
            return null;
        }
        // #############################################################################################################
        // internal methods 
        // #############################################################################################################
        /// <summary>
        /// generates a rectangle with tileDim and matches its position within the picture
        /// </summary>
        /// <param name="index">tile index</param>
        /// <returns></returns>
        internal Rectangle GetFrame(uint index)
        {
            var c = 0;
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (index == StartEntry + c)
                        return new Rectangle(TileDimX * column, TileDimY * row, TileDimX, TileDimY);
                    c++;
                }
            }
            return Rectangle.Empty;
        }
    }
}