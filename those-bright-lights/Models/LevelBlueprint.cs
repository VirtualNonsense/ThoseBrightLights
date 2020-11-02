using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models
{
    class LevelBlueprint
    {
        public int compressionlevel;
        public int height;
        public bool infinite;
        public List<LevelLayer> layers;
        public int nextlayerid;
        public int nextobjectid;
        public string orientation;
        public string renderorder;
        public string tiledversion;
        public int tileheight;
        public List<Tileset> tilesets;
        public int tilewidth;
        public string type;
        public float version;
        public int width;

    }

    internal class Tileset
    {
       public int firstgid;
       public string source;
        
    }
}
