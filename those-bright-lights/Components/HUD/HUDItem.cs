using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.HUD
{
    public abstract class HUDItem : IComponent
    {
        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsRemoveAble { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
