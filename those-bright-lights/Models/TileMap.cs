using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SE_Praktikum.Models
{
    class TileMap
    {
        int rows,collums;

        public Texture2D texture;

        int tilewidth, tileheight;

        public int Tiles =>  collums* rows;


        public TileMap(Texture2D texture, int collums, int rows)
        {
            this.texture = texture;
            this.collums = collums;
            this.rows = rows;
            this.tilewidth = texture.Width / collums;
            this.tileheight = texture.Height / rows;

            
        }
        public Rectangle? Frame(int index)
        {
            if (index <= 0)
                return null;
            else if (index > rows * collums)
                return null;
            
            int x = (index % collums) - 1;
            int y = index / rows;
            var r = new Rectangle(x * tilewidth, y * tileheight, tilewidth, tileheight);
            return r;

            //new Rectangle(_currentFrame * _animation.FrameWidth,
            //    0,
            //    _animation.FrameWidth,
            //    _animation.FrameHeight);

        }


    }
}
