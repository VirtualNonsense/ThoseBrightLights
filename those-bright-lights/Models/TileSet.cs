using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using System;

namespace SE_Praktikum.Models
{
    public class TileSet
    {
        public int TileDimX;
        public int TileDimY;
        public int Columns;
        public int Rows;
        public Texture2D Texture;
        public int Tiles => Columns * Rows;
        public int StartEntry;
        private ILogger _logger;

        
        public TileSet(Texture2D texture, int tileDimX, int tileDimY, int startEntry)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Texture = texture;
            TileDimX = tileDimX;
            TileDimY = tileDimY;
            Columns = Texture.Width / TileDimX;
            Rows = Texture.Height / TileDimY;
            StartEntry = startEntry;
        }

        internal Rectangle GetFrame(uint index)
        {
            var c = 1;
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (index == c)
                        return new Rectangle(Columns * column, Rows * row, TileDimX, TileDimY);
                    c++;
                }
            }
            _logger.Warn($"index{index} not found");
            return Rectangle.Empty;
        }
    }
}