using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models
{
    class LevelLayer
    {
        public List<uint> data;
        public int height;
        public int id;
        public string name;
        public float opacity;
        public string type;
        public bool visible;
        public int width;
        public int x;
        public int y;

        public LevelLayer(List<uint> data, int height, int id, string name, float opacity, string type, bool visible, int width, int x, int y)
        {
            this.data = data;
            this.height = height;
            this.id = id;
            this.name = name;
            this.opacity = opacity;
            this.type = type;
            this.visible = visible;
            this.width = width;
            this.x = x;
            this.y = y;
        }
    }
}
