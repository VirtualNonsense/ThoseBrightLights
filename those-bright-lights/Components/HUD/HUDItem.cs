using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using SE_Praktikum.Components.Sprites.Actors;

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
        public Vector2 Origin => new Vector2(ElementWidth/2f, ElementHeight/2f);

        public float ElementWidth => _handler.Count * tileSet.TileDimX;

        public float ElementHeight => tileSet.TileDimY;

        protected List<AnimationHandler> _handler;
        protected HUD _parent;
        protected readonly AnimationHandlerFactory animationHandlerFactory;
        protected readonly TileSet tileSet;

        public HUDItem(HUD parent, AnimationHandlerFactory animationHandlerFactory, TileSet tileSet)
        {
            _handler = new List<AnimationHandler>();
            _parent = parent;
            this.animationHandlerFactory = animationHandlerFactory;
            this.tileSet = tileSet;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _handler)
            {
                item.Offset = Offset + _parent.Position;
                item.Layer = _parent.Layer;
                item.Draw(spriteBatch);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
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
