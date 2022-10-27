using System;
using System.Collections.Generic;
using System.Text;

namespace ThoseBrightLights.Models.Tiled
{
    /// <summary>
    /// Tileset animation fields
    /// </summary>
    public class TileSetAnimation
    {
        public float Duration;
        public int Tileid;
        public int Id;

        // Constructor
        public TileSetAnimation(float duration,int tileid,int id)
        {
            this.Duration = duration;
            this.Tileid = tileid;
            this.Id = id;
        }
           


    }
}
