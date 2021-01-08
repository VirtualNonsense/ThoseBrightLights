using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Components.HUD
{
    public abstract class HUDItem : IComponent
    {
        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsRemoveAble { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<AnimationHandler> _handler;
        public HUD _parent;

        public HUDItem(HUD parent, List<AnimationHandler> handler)
        {
            _parent = parent;
            _handler = handler;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _handler)
            {
                item.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

      
    }
}
