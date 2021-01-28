namespace SE_Praktikum.Models.Tiled
{
    // Dictionary for tilesets
    public class TileSetDictionary
    {
        public int FirstGId;
        public string Source;

        // Constructor
        public TileSetDictionary(int firstGId, string source)
        {
            FirstGId = firstGId;
            Source = source;
        }
    }
}