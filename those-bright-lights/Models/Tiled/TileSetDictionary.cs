namespace SE_Praktikum.Models.Tiled
{
    public class TileSetDictionary
    {
        public int FirstGId;
        public string Source;

        public TileSetDictionary(int firstGId, string source)
        {
            FirstGId = firstGId;
            Source = source;
        }
    }
}