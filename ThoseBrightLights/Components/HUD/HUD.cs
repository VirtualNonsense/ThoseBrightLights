using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;

namespace ThoseBrightLights.Components.HUD
{
    public class HUD : IComponent
    {
        // Fields
        public Player Player;
        private readonly List<HUDItem> _hUDItems;
        public float Layer;

        // Properties
        public Vector2 Position { get; set; }
        public bool IsRemoveAble { get; set; }

        // Constructor
        public HUD(Player player)
        {
            Layer = player.Layer + 1;
            Player = player;
            _hUDItems = new List<HUDItem>();
            Player.OnPositionChanged += _player_OnPositionChanged;
        }

        // Event when Player changed its position
        private void _player_OnPositionChanged(object sender, EventArgs e)
        {
            Position = Player.Position;
        }

        // Monogame functions
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

        // Adds any Hud-Item
        public void AddHUDItem(HUDItem hUDItem)
        {
            _hUDItems.Add(hUDItem);
        }
    }
}
