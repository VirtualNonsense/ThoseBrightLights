using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models
{
    class LevelBlueprint
    {
        int compressionlevel;
        int height;
        bool infinite;
        List<LevelLayer> layers;
        int nextlayerid;
        int nextobjectid;
        string orientation;
        string renderorder;
        string tiledversion;
        int tileheight;
        List<Tileset> tilesets;
        int tilewidth;
        string type;
        float version;
        int width;

    }

    internal class Tileset
    {
        int firstgid;
        string source;
        
    }
}
