using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE_Praktikum.Core;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Controls
{
    public abstract class MenuItem : IComponent
    {
        protected readonly List<AnimationHandler> _handler;
        protected readonly Camera _camera;
        public Vector2 _position;
        public Vector2 _origin;

        public MenuItem(List<AnimationHandler> handler, Camera camera)
        {
            _handler = handler;
            _camera = camera;
            Frame = GetRectangle();
            _origin = new Vector2(Frame.Width/2f, Frame.Height/2f);
            
            // a bit stupid but currently necessary
            // consider rewriting it....
            Frame = GetRectangle();
        }

        public virtual Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                Frame = GetRectangle();
            }
        }

        public virtual Vector2 Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                Frame = GetRectangle();
            }
        }

        public virtual float Layer
        {
            get => _handler.First().Settings.Layer;
            set
            {
                foreach (var animationHandler in _handler)
                {
                    animationHandler.Settings.Layer = value;
                }
            }
        }

        public Rectangle Frame { get; private set; }
        
        protected Vector2 Offset => _position - _origin;
        
        
        private Rectangle GetRectangle()
        {
            // making sure an intersection between r and the other is impossible.
            Rectangle r = new Rectangle(int.MaxValue, Int32.MaxValue, 0, 0);
            foreach (AnimationHandler animationHandler in _handler)
            {
                // will be updated during update method but must be set initially as well.
                animationHandler.Offset = Offset;
                var frame = animationHandler.Frame;
                frame.X =  (int) (animationHandler.Position.X + animationHandler.Offset.X -
                                  animationHandler.Origin.X);
                frame.Y =  (int) (animationHandler.Position.Y + animationHandler.Offset.Y -
                                  animationHandler.Origin.Y );
                Rectangle.Union(ref frame, ref r, out r);
            }
            return r;
        }
        
        public  abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public bool IsRemoveAble { get; set; }
    }
}