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
    public class ScoreBar : HUDItem // Is a HUD-Element
    {
        // Fields
        private readonly AnimationSettings numberAnimationSettings;
        private const int margin = 5;

        // Constructor
        public ScoreBar(HUD parent, AnimationHandlerFactory animationHandlerFactory, TileSet tileSet, AnimationSettings numberAnimationSettings) : base(parent, animationHandlerFactory, tileSet)
        {
            parent.Player.OnScoreChanged += Player_OnScoreChanged;
            this.numberAnimationSettings = numberAnimationSettings;
            UpdateAmountDigits(parent.Player.Score);
            UpdateDigits(_parent.Player.Score);
        }

        // Event - for Score changed
        private void Player_OnScoreChanged(object sender, EventArgs e)
        {
            UpdateAmountDigits(_parent.Player.Score);
            UpdateDigits(_parent.Player.Score);
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

        // Both functions keep track of the score
        public void UpdateAmountDigits(int newScore)
        {
            var digits = newScore == 0 ? 1 : Math.Floor(Math.Log10(newScore)+1);

            if (digits < _handler.Count)
            {
                for (var i = _handler.Count - 1; i >= 0; i--)
                {
                    _handler.RemoveAt(i);
                }
            }
            else
            {
                for (var i = _handler.Count; i < digits; i++)
                {
                    var posX = -i * (tileSet.TileDimX+margin)-margin;

                    _handler.Add(ConstructDigit(new Vector2(posX, 0)));
                }
            }
        }

        public void UpdateDigits(int score)
        {
            int rest = score;
            foreach (var item in _handler)
            {
                var digit = rest % 10;
                item.CurrentIndex = digit;
                rest /= 10;
            }
        }

        // The seperate digits of the score were built
        public AnimationHandler ConstructDigit(Vector2 position)
        {
            return animationHandlerFactory.GetAnimationHandler(tileSet,
                new List<AnimationSettings>(new[] {numberAnimationSettings}), position,
                new Vector2(tileSet.TileDimX, 0));
        }
    }
}
