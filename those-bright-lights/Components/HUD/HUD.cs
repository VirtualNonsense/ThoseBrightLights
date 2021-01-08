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

       

        public HUD(Player player)
        {
            _player = player;
            _hUDItems = new List<HUDItem>();
            _player.OnPositionChanged += _player_OnPositionChanged;
        }

        private void _player_OnPositionChanged(object sender, EventArgs e)
        {
            Position = _player.Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void AddHUDItem(HUDItem hUDItem)
        {
            _hUDItems.Add(hUDItem);
        }
    }
}
