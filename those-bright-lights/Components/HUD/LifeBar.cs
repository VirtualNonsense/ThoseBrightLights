using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.HUD
{
    public class LifeBar : HUDItem
    {
        public int HealthPerHeart { get; set; }

        public LifeBar(HUD parent, List<AnimationHandler> handler) : base(parent, handler)
        {
            HealthPerHeart = 5;
            parent.Player.OnHealthChanged += Player_OnHealthChanged; 
        }

        private void Player_OnHealthChanged(object sender, EventArgs e)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void UpdateHealthBar()
        {

        }
    }
}
