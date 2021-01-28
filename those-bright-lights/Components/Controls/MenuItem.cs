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

        // #############################################################################################################
        // constructor
        // #############################################################################################################
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
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public bool IsRemoveAble { get; set; }
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
            get => _handler.First().Layer;
            set
            {
                foreach (var animationHandler in _handler)
                {
                    animationHandler.Layer = value;
                }
            }
        }

        public Rectangle Frame { get; private set; }
        
        protected Vector2 Offset => _position - _origin;
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public  abstract void Draw(SpriteBatch spriteBatch);
        
        public abstract void Update(GameTime gameTime);
        
        
        // #############################################################################################################
        // private methods
        // #############################################################################################################
        /// <summary>
        /// This Method updates the position of all Rectangles in the background and returns the combined area as rectangle
        /// </summary>
        /// <returns></returns>
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
    }
}