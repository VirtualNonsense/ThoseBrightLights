using Microsoft.Xna.Framework.Graphics;

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
        
        public TileSet(Texture2D texture, int tileDimX, int tileDimY, int startEntry)
        {
            Texture = texture;
            TileDimX = tileDimX;
            TileDimY = tileDimY;
            Columns = Texture.Width / TileDimX;
            Rows = Texture.Height / TileDimY;
            StartEntry = startEntry;
        }
    }
}