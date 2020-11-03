using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.Sprites
{
    class Tile:IComponent
    {
        TileMap _tilemap;
        int _index;
        Vector2 _origin;

        public Tile(TileMap tilemap, int index, Vector2 origin)
        {
            _tilemap = tilemap;
            this._index = index;
            _origin = origin;
        }






        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_tilemap.Frame(_index) == null)
                return;
            spriteBatch.Draw(_tilemap.texture, (Rectangle)_tilemap.Frame(_index), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
