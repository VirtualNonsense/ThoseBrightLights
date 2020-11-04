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
        public int NextLayerId;
        public int NextObjectId;
        public string orientation;
        public string RenderOrder;
        public string TiledVersion;
        public int TileHeight;
        public List<TileSet> TileSets;
        public int TileWidth;
        public string type;
        public float version;
        public int width;

        public LevelBlueprint(int compressionLevel, int height, bool infinite, List<LevelLayer> layers, int nextLayerId, int nextObjectId, string orientation, string renderOrder, string tiledVersion, int tileHeight, List<TileSet> tileSets, int tileWidth, string type, float version, int width)
        {
            compressionlevel = compressionLevel;
            this.height = height;
            this.infinite = infinite;
            this.layers = layers;
            NextLayerId = nextLayerId;
            NextObjectId = nextObjectId;
            this.orientation = orientation;
            RenderOrder = renderOrder;
            TiledVersion = tiledVersion;
            TileHeight = tileHeight;
            TileSets = tileSets;
            TileWidth = tileWidth;
            this.type = type;
            this.version = version;
            this.width = width;
        }
    }

    internal class TileSet
    {
       public int FirstGId;
       public string source;

       public TileSet(int firstGId, string source)
       {
           FirstGId = firstGId;
           this.source = source;
       }
    }
}
