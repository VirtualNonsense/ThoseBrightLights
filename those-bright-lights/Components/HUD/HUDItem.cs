﻿using Microsoft.Xna.Framework;
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
        /// <summary>
        /// HUD Item coordinates in parent system
        /// </summary>
        public Vector2 Position { get; set; }
        public bool IsRemoveAble { get; set; }
        public Vector2 Offset => Position-Origin;
        public Vector2 Origin;

        protected List<AnimationHandler> _handler;
        protected HUD _parent;

        public HUDItem(HUD parent, List<AnimationHandler> handler)
        {
            _parent = parent;
            _handler = handler;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _handler)
            {
                item.Draw(spriteBatch);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        private void UpdateAnimationHandler(Vector2 position)
        {
            foreach (var item in _handler)
            {
                item.Offset = Offset+_parent.Position;
            }
        }
    }
}
