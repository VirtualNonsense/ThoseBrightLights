using System.Collections.Generic;

namespace ThoseBrightLights.Models.Tiled
{
    /// <summary>
    /// Layer fields
    /// </summary>
    public class LevelLayerBluePrint
    {
        public List<uint> Data;
        public ObjectBluePrint[] objects;
        public int Height;
        public int Id;
        public string Name;
        public float Opacity;
        public string Type;
        public bool Visible;
        public int Width;
        public int X;
        public int Y;

        
    }
}
