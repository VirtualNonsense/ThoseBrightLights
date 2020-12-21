using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models.Tiled
{
   public class TileSetBlueprint
    {
        public int columns;
        public string image;
        public int imageheight;
        public int imagewidth;
        public int margin;
        public string name;
        public int spacing;
        public int tilecount;
        public string tiledversion;
        public int tileheight;
        public int tilewidth;
        public string type;
        public float version;
        public TileBluePrint[] tiles;
        

        public TileSetBlueprint(int columns, string image, int imageheight, int imagewidth, int margin, string name, int spacing, int tilecount, string tiledversion, int tileheight, int tilewidth, string type, float version, TileBluePrint []tiles)
        {
            this.columns = columns;
            this.image = image;
            this.imageheight = imageheight;
            this.imagewidth = imagewidth;
            this.margin = margin;
            this.name = name;
            this.spacing = spacing;
            this.tilecount = tilecount;
            this.tiledversion = tiledversion;
            this.tileheight = tileheight;
            this.tilewidth = tilewidth;
            this.type = type;
            this.version = version;
            this.tiles = tiles;
        }


    }
}
