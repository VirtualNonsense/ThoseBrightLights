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
        public Player Player;
        private readonly List<HUDItem> _hUDItems;

        public Vector2 Position { get; set; }
        public bool IsRemoveAble { get; set; }

       

        public HUD(Player player)
        {
            Player = player;
            _hUDItems = new List<HUDItem>();
            Player.OnPositionChanged += _player_OnPositionChanged;
        }

        private void _player_OnPositionChanged(object sender, EventArgs e)
        {
            Position = Player.Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _hUDItems)
            {
                item.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void AddHUDItem(HUDItem hUDItem)
        {
            _hUDItems.Add(hUDItem);
        }
    }
}
