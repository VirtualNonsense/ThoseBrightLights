using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SE_Praktikum.Models
{
    public class TileMap
    {
        public readonly List<TileSet> TileSets;

        public int Tiles
        {
            get
            {
                var r = 0;
                foreach (var tileSet in TileSets)
                {
                    r += tileSet.Tiles;
                }
                return r;
            }
        }


        public TileMap(List<TileSet> tileSets)
        {
            TileSets = tileSets;
        }

    }
}
