using SE_Praktikum.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Components.Sprites;
using System.Data;
using System.Linq;
using SE_Praktikum.Models.Tiled;

namespace SE_Praktikum.Components
{
    public class Map: IComponent, IEnumerable<Tile>
    {
        private List<Tile> _tiles;
        
        public float TopLayer { get; }


        public Map (List<Tile> tileses)
        {
            _tiles = tileses;
            TopLayer = 0;
            foreach (var tile in _tiles.Where(tile => TopLayer < tile.Layer))
            {
                TopLayer = tile.Layer;
            }
        }

        public Vector2 Position { get=>throw new NotImplementedException(); set=>throw new NotImplementedException(); }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var tile in _tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public bool IsRemoveAble { get; set; }
        public IEnumerator<Tile> GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
