using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models.Tiled
{
    public class TileSetAnimation
    {
        public float Duration;
        public int Tileid;
        public int Id;

        public TileSetAnimation(float duration,int tileid,int id)
        {
            this.Duration = duration;
            this.Tileid = tileid;
            this.Id = id;
        }
           


    }
}
