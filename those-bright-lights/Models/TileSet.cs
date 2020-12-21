﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using System;
using System.Collections.Generic;
 using System.Linq;

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
        private readonly Dictionary<int, Polygon[]> _hitBoxDict;
        private ILogger _logger;
        public int FrameCount => Columns * Rows;
        public int TextureWidth => Texture.Width;
        public int TextureHeight => Texture.Height;

        public bool HasHitBox => _hitBoxDict != null;
        
        public TileSet(Texture2D texture, int tileDimX, int tileDimY, Dictionary<int, Polygon[]> hitBoxDict, int startEntry=0)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Texture = texture;
            TileDimX = tileDimX;
            TileDimY = tileDimY;
            Columns = Texture.Width / TileDimX;
            Rows = Texture.Height / TileDimY;
            StartEntry = startEntry;
            _hitBoxDict = hitBoxDict;
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

        public Vector2 GetFrameCenter()
        {
            return new Vector2(TileDimX/2f, TileDimY/2f);
        }
        
        [Obsolete]
        public Byte[] GetDataOfFrame(int tile)
        {
            tile -= StartEntry;
            int rowOfTile = tile / Columns;
            int columnOfTile = tile % Columns;


            var tilewidth = TileDimX;
            var tileheight = TileDimY;

            //offset for all rows of all tiles above the tilerow we want
            var rowOffsetForAllTilesAbove = TextureWidth * tileheight*rowOfTile ;
            //offset for all pixels in one tile calculated with the columnnumber 
            var pixelColumnOffset = columnOfTile * tilewidth;

            //array for one tile to copy sth in 
            Byte[] pixelArray = new Byte[TextureWidth * TextureHeight];
            
            //array filled with all tiles from tileset 
            Color[] allTiles = new Color[TextureWidth * TextureHeight];
            Texture.GetData(allTiles);


            
            //iterating over the tileheight in row steps
            for(int row = 0; row < TileDimY; row++)
            {
                //offset for pixels in each row
                var rowPixelOffset = row * TextureWidth;
                //iterating over the tilewidth in column steps in one row step
                for(int column = 0; column < TileDimX; column++)
                {
                    //summing up all offsets until the pixel we need 
                    pixelArray[row * tilewidth + column] = allTiles[rowOffsetForAllTilesAbove + pixelColumnOffset + rowPixelOffset + column].A;
                }
            }
            return pixelArray;
        }

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
    }
}