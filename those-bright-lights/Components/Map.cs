using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Components.Sprites;
using System.Data;
using SE_Praktikum.Models.Tiled;

namespace SE_Praktikum.Components
{
    public class Map: IComponent
    {
        private List<Tile> _tile;


        public Map (List<Tile> tiles)
        {
            _tile = tiles;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }


    }
}
