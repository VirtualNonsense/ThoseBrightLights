using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.HUD
{
    public class HUD : IComponent
    {
        private readonly Player _player;
        private readonly List<HUDItem> _hUDItems;

        public Vector2 Position { get; set; }
        public bool IsRemoveAble { get; set; }

       

        public HUD(Player player, List<HUDItem> hUDItems)
        {
            this._player = player;
            this._hUDItems = hUDItems;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
