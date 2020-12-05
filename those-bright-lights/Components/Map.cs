using SE_Praktikum.Models;
using System;
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
    public class Map: IComponent
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(var tile in _tiles)
            {
                tile.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public bool IsRemoveAble { get; set; }
    }
}
