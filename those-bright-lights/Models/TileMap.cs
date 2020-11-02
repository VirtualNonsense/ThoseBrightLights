using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models
{
    class TileMap
    {
        int rows,collums;

        public Texture2D texture;

        int tilewidth, tileheight;


        public TileMap(Texture2D texture, int collums, int rows)
        {
            this.texture = texture;
            this.collums = collums;
            this.rows = rows;
            this.tilewidth = texture.Width / collums;
            this.tileheight = texture.Height / rows;

            
        }
        public Rectangle Frame(int x, int y)
        {
            var r = new Rectangle(x * tilewidth, y * tileheight, tilewidth, tileheight);
            return r;

            //new Rectangle(_currentFrame * _animation.FrameWidth,
            //    0,
            //    _animation.FrameWidth,
            //    _animation.FrameHeight);

        }


    }
}
