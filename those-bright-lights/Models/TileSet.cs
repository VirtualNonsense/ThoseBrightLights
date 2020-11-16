using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public int FrameCount => Columns * Rows;
        public int FrameWidth => Rows * TileDimX;
        public int FrameHeight => Columns * TileDimY;

        
        public TileSet(Texture2D texture, int tileDimX, int tileDimY, int startEntry=0)
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
            _logger.Warn($"index{index} not found");
            return Rectangle.Empty;
        }
        
        public Color[] GetDataOfFrame(int tile)
        {
            int rowOfTile = tile / Columns;
            int columnOfTile = tile % Columns;

            var texturewidth = TileDimX * Columns;
            var textureheight = TileDimY * Rows;

            var tilewidth = TileDimX;
            var tileheight = TileDimY;

            //offset for all rows of all tiles above the tilerow we want
            var rowOffsetForAllTilesAbove = texturewidth * tileheight*rowOfTile ;
            //offset for all pixels in one tile calculated with the columnnumber 
            var pixelColumnOffset = columnOfTile * tilewidth;

            //array for one tile to copy sth in 
            Color[] pixelArray = new Color[tilewidth *tileheight];
            
            //array filled with all tiles from tileset
            Color[] allTiles = new Color[texturewidth*textureheight];
            Texture.GetData(pixelArray);


            
            //iterating over the tileheight in row steps
            for(int row = 0; row < TileDimY; row++)
            {
                //offset for pixels in each row
                var rowPixelOffset = row * texturewidth;
                //iterating over the tilewidth in column steps in one row step
                for(int column = 0; row < TileDimX; column++)
                {
                    //summing up all offsets until the pixel we need 
                    pixelArray[row * tilewidth + column] = allTiles[rowOffsetForAllTilesAbove + pixelColumnOffset + rowPixelOffset + column];
                }
            }
            return pixelArray;


        }
    }
}