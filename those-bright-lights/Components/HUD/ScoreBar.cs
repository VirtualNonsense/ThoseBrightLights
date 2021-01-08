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
    public class ScoreBar : HUDItem
    {
        private readonly AnimationSettings numberAnimationSettings;

        public ScoreBar(HUD parent, AnimationHandlerFactory animationHandlerFactory, TileSet tileSet, AnimationSettings numberAnimationSettings) : base(parent, animationHandlerFactory, tileSet)
        {
            parent.Player.OnScoreChanged += Player_OnScoreChanged;
            this.numberAnimationSettings = numberAnimationSettings;
        }

        private void Player_OnScoreChanged(object sender, EventArgs e)
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

        public void AmountDigits(int newScore)
        {
            var digits = Math.Log10(newScore);

            if (digits < _handler.Count)
            {
                for (int i = _handler.Count - 1; i >= 0; i--)
                {
                    _handler.RemoveAt(i);
                }
            }
            else
            {
                for (int i = _handler.Count; i < digits; i++)
                {
                    var posX = -i * tileSet.TileDimX;

                    _handler.Add(ConstrucDigit(new Vector2(posX, 0)));
                }
            }
        }

        public AnimationHandler ConstrucDigit(Vector2 position)
        {
            return animationHandlerFactory.GetAnimationHandler(tileSet, numberAnimationSettings, position, new Vector2(tileSet.TileDimX,0));
        }
    }
}
