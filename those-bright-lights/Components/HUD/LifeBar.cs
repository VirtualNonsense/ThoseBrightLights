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
        private readonly AnimationSettings animationSettingsLeftHeart;
        private readonly AnimationSettings animationSettingsRightHeart;

        public int HealthPerHeart { get; set; }

        public LifeBar(HUD parent, AnimationHandlerFactory animationHandlerFactory, TileSet tileSet, AnimationSettings animationSettingsLeftHeart, AnimationSettings animationSettingsRightHeart) : base(parent, animationHandlerFactory, tileSet)
        {
            HealthPerHeart = 5;
            parent.Player.OnHealthChanged += Player_OnHealthChanged;
            parent.Player.OnMaxHealthChanged += Player_OnMaxHealthChanged;
            this.animationSettingsLeftHeart = animationSettingsLeftHeart;
            this.animationSettingsRightHeart = animationSettingsRightHeart;
            UpdateAmountHearts(parent.Player.MaxHealth);
        }

        private void Player_OnMaxHealthChanged(object sender, EventArgs e)
        {
            UpdateAmountHearts(_parent.Player.MaxHealth);
        }

        private void Player_OnHealthChanged(object sender, EventArgs e)
        {
            UpdateAmountHearts(_parent.Player.Health);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void UpdateAmountHearts(float newHealth)
        {
            var hearts = (int)(HealthPerHeart / newHealth);

            if (hearts < _handler.Count)
            {
                for (int i = _handler.Count-1; i >= 0; i--)
                {
                    _handler.RemoveAt(i);
                }
            }
            else
            {
                for (int i = _handler.Count; i <= hearts; i++)
                {
                    if (i % 2 == 0)
                    {
                        _handler.Add(ConstructHeart(new Vector2(i * tileSet.TileDimX, 0), animationSettingsLeftHeart));
                    }
                    else
                    {
                        _handler.Add(ConstructHeart(new Vector2(i * tileSet.TileDimX, 0), animationSettingsRightHeart));
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
                if (healthThreshold >= health)
                {
                    _handler[i].CurrentIndex = 0;
                }
                else
                {
                    _handler[i].CurrentIndex = 1;
                }
            }
        }

        private AnimationHandler ConstructHeart(Vector2 position, AnimationSettings animationSettings)
        {
            return animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings,position);
        }
    }
}
