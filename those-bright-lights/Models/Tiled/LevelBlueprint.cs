using System.Collections.Generic;

namespace SE_Praktikum.Models.Tiled
{
    public class LevelBlueprint
    {
        public int CompressionLevel;
        public int Height;
        public bool Infinite;
        public List<LevelLayerBluePrint> Layers;
        public int NextLayerId;
        public int NextObjectId;
        public string Orientation;
        public string RenderOrder;
        public string TiledVersion;
        public int TileHeight;
        public List<TileSetDictionary> tilesets;
        public int TileWidth;
        public string Type;
        public string Version;
        public int Width;

        public LevelBlueprint(int compressionLevel, int height, bool infinite, List<LevelLayerBluePrint> layers, int nextLayerId, int nextObjectId, string orientation, string renderOrder, string tiledVersion, int tileHeight, List<TileSetDictionary> tileSetsBlueprints, int tileWidth, string type, string version, int width)
        {
            CompressionLevel = compressionLevel;
            Height = height;
            Infinite = infinite;
            Layers = layers;
            NextLayerId = nextLayerId;
            NextObjectId = nextObjectId;
            Orientation = orientation;
            RenderOrder = renderOrder;
            TiledVersion = tiledVersion;
            TileHeight = tileHeight;
            tilesets = tileSetsBlueprints;
            TileWidth = tileWidth;
            Type = type;
            Version = version;
            Width = width;
        }
    }
}
