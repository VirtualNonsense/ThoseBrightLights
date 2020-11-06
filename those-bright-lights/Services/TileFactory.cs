using Microsoft.Xna.Framework;
using NLog;
using NVorbis.Ogg;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Services
{
    public class TileFactory
    {
        private ILogger _logger;
        public TileFactory()
        {
             _logger = LogManager.GetCurrentClassLogger();
        }
        public Tile GenerateTile(uint index, Vector2 position, float layer, List<TileSet> tilesets)
        {
             
            foreach(var tileset in tilesets)
            {
                if (index > tileset.StartEntry + tileset.Tiles-1)
                    continue;
                return new Tile(tileset.Texture, tileset.GetFrame(index), position, layer);

            }
            return null;

        }

        public List<Tile> GenerateTiles(List<uint> indices, float layer, List<TileSet> tilelist, int tilewidth, int tileheight, int rows, int columns)
        {

            if (indices.Count > rows * columns)
            {
                _logger.Error("Indices out of range");
                throw new IndexOutOfRangeException();
            }
            List<Tile> list = new List<Tile>();
            int row = 0;
            int column = 0;

            foreach(var index in indices)
            {
                if (index == 0)
                    continue;
                var p = new Vector2(column * tilewidth, row * tileheight);
                list.Add(GenerateTile(index, p, layer, tilelist));
                column++;
                if(column == columns)
                {
                    column = 0;
                    row++;
                }

                
            }




            return list;


        }

    }
}
