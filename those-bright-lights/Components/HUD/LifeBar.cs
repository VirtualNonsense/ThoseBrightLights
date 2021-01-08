using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.HUD
{
    public class LifeBar : HUDItem
    {
        private readonly AnimationSettings animationSettings;

        public int HealthPerHeart { get; set; }

        public LifeBar(HUD parent, AnimationHandlerFactory animationHandlerFactory, TileSet tileSet, AnimationSettings animationSettings) : base(parent, animationHandlerFactory, tileSet)
        {
            HealthPerHeart = 5;
            parent.Player.OnHealthChanged += Player_OnHealthChanged;
            this.animationSettings = animationSettings;
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

        private AnimationHandler ConstructHeart(Vector2 position)
        {
            return animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings,position);
        }
    }
}
