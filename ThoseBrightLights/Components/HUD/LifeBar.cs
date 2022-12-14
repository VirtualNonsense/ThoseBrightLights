using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;
using ThoseBrightLights.Services.Factories;

namespace ThoseBrightLights.Components.HUD
{
    public class LifeBar : HUDItem // Is a HUD-Element
    {
        // Quantised damage in form of a half heart
        private readonly AnimationSettings animationSettingsLeftHeart;
        private readonly AnimationSettings animationSettingsRightHeart;

        // Properties
        public int HealthPerHeart { get; set; }

        // Constructor
        public LifeBar(HUD parent, AnimationHandlerFactory animationHandlerFactory, TileSet tileSet, AnimationSettings animationSettingsLeftHeart, AnimationSettings animationSettingsRightHeart) : base(parent, animationHandlerFactory, tileSet)
        {
            HealthPerHeart = 5;
            parent.Player.OnHealthChanged += Player_OnHealthChanged;
            parent.Player.OnMaxHealthChanged += Player_OnMaxHealthChanged;
            this.animationSettingsLeftHeart = animationSettingsLeftHeart;
            this.animationSettingsRightHeart = animationSettingsRightHeart;
            UpdateAmountHearts(parent.Player.MaxHealth);
        }

        // Event - for maximum health is changed (e.g. through powerup)
        private void Player_OnMaxHealthChanged(object sender, EventArgs e)
        {
            UpdateAmountHearts(_parent.Player.MaxHealth);
        }

        // Event - for the "normal" health loss (e.g. enemy attack, collision) 
        private void Player_OnHealthChanged(object sender, EventArgs e)
        {
            UpdateHearts(_parent.Player.Health);
        }

        // Monogame functions
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // Both functions keep track of hearts depending on the actual health
        private void UpdateAmountHearts(float newHealth)
        {
            var hearts = (int)(newHealth / HealthPerHeart);

            if (hearts < _handler.Count)
            {
                for (int i = _handler.Count-1; i >= 0; i--)
                {
                    _handler.RemoveAt(i);
                }
            }
            else
            {
                for (int i = _handler.Count; i < hearts; i++)
                {
                    var posX = i * tileSet.TileDimX;
                    
                    if (i % 2 == 0)
                    {
                        _handler.Add(ConstructHeart(new Vector2(posX, 0), animationSettingsLeftHeart));
                    }
                    else
                    {
                        _handler.Add(ConstructHeart(new Vector2(posX, 0), animationSettingsRightHeart));
                    }
                }
            }
            UpdateHearts(_parent.Player.Health);
        }

        public void UpdateHearts(float health)
        {
            for (int i = 0; i < _handler.Count; i++)
            {
                var healthThreshold = (i + 1) * HealthPerHeart;

                // 0 represents empty heart and 1 a full heart
                if (healthThreshold > health)
                {
                    _handler[i].CurrentIndex = 0;
                }
                else
                {
                    _handler[i].CurrentIndex = 1;
                }
            }
        }

        // A heart is build
        private AnimationHandler ConstructHeart(Vector2 position, AnimationSettings animationSettings)
        {
            return animationHandlerFactory.GetAnimationHandler(tileSet,
                new List<AnimationSettings>(new[] {animationSettings}), position, Vector2.Zero);
        }
    }
}
