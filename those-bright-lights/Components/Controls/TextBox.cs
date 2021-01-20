using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Core;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Controls
{
    public sealed class TextBox : MenuItem
    {
        private readonly SpriteFont _font;
        

        // #############################################################################################################
        // constructor
        // #############################################################################################################
        /// <summary>
        /// Simple text box class
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="textColor"></param>
        /// <param name="camera"></param>
        /// <param name="text"></param>
        public TextBox(List<AnimationHandler> handler, SpriteFont font, Vector2 position, Color textColor, Camera camera, string text) : base(handler, camera)
        {
            _font = font;
            Position = position;
            Text = text;
            TextColor = textColor;
        }
        
        // #############################################################################################################
        // properties
        // #############################################################################################################
        public string Text { get; set; }
        public Color TextColor { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var animationHandler in _handler)
            {
                animationHandler.Draw(spriteBatch);
            }
            
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Frame.X + (Frame.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Frame.Y + (Frame.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), TextColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}