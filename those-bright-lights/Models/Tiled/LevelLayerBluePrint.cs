using System.Collections.Generic;

namespace SE_Praktikum.Models.Tiled
{
    public class LevelLayerBluePrint
    {
        public List<uint> Data;
        public int Height;
        public int Id;
        public string Name;
        public float Opacity;
        public string Type;
        public bool Visible;
        public int Width;
        public int X;
        public int Y;

        public LevelLayerBluePrint(List<uint> data, int height, int id, string name, float opacity, string type, bool visible, int width, int x, int y)
        {
            Data = data;
            Height = height;
            Id = id;
            Name = name;
            Opacity = opacity;
            Type = type;
            Visible = visible;
            Width = width;
            X = x;
            Y = y;
        }
    }
}
