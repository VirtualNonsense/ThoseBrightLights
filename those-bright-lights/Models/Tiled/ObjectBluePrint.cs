using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models.Tiled
{
    /// <summary>
    /// Object fields 
    /// </summary>
    public class ObjectBluePrint
    {
        public float height;
        public int id;
        public string name;
        public float rotation;
        public string type;
        public bool visible;
        public float width;
        public float x;
        public float y;
        public ObjectVector2[] polygon;
        public class ObjectVector2
        {
            public float x;
            public float y;
        }

    }
}
