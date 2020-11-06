using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.Sprites
{
    public class Tile:IComponent
    {
        TileMap _tilemap;
        private readonly Rectangle _frame;
        Vector2 _origin;
        private float _layer;

        public Tile(TileMap tilemap, Rectangle frame, Vector2 origin, float layer)
        {
            _tilemap = tilemap;
            _frame = frame;
            _origin = origin;
            _layer = layer;
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
