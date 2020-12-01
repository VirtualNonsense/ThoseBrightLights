using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Core;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Controls
{
    public class Slider : MenuItem
    {
        public Slider(List<AnimationHandler> handler, Camera camera, bool useCenterAsOrigin) : base(handler, camera, useCenterAsOrigin)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}