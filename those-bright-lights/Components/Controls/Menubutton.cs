using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Core;
using SE_Praktikum.Services;


namespace SE_Praktikum.Components.Controls
{
    public class Menubutton : IComponent
    {
        #region Fields

        private MouseState _currentMouse;

        private readonly List<AnimationHandler> _handler;
        private SpriteFont _font;
        private readonly Camera _camera;

        private bool _isHovering;

        private MouseState _previousMouse;

        
        private Vector2 _origin;
        private Vector2 _position;

        private Logger _logger;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color TextColor { get; set; }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                Frame = GetRectangle();
            }
        }

        public Vector2 Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                Frame = GetRectangle();
            }
        }

        public Rectangle Frame { get; private set; }

        private Vector2 Offset => _position - _origin;

        public string Text { get; set; }

        #endregion

        #region Methods

        public Menubutton(List<AnimationHandler> handler,
                          SpriteFont font,
                          Color? textColor = null,
                          Vector2? position = null,
                          bool useCenterAsOrigin = true,
                          Camera camera = null,
                          string text = "")
        {
            _logger = LogManager.GetCurrentClassLogger();
            _position = position ?? Vector2.Zero;
            _handler = handler;
            _origin = Vector2.Zero;
            _font = font;
            _camera = camera;
            TextColor = textColor ?? Color.CornflowerBlue;
            Text = text;
            Frame = GetRectangle();
            if (useCenterAsOrigin)
            {
                _origin = new Vector2(Frame.Width/2f, Frame.Height/2f);
                // a bit stupid but currently necessary
                // consider rewriting it....
                Frame = GetRectangle();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var co = Color.White;

            if (_isHovering)
                co = Color.Gray;

            if (Clicked)
            {
                co = Color.Black;
            }

            foreach (var handler in _handler)
            {
                handler.Settings.Color = co;
                handler.CurrentIndex = Clicked? 1 : 0;
                handler.Offset = Offset;
                handler.Draw(spriteBatch);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Frame.X + (Frame.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Frame.Y + (Frame.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), TextColor);
            }
        }

        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var pos = _camera is null? new Vector2(_currentMouse.X, _currentMouse.Y) : 
                                              _camera.ProjectScreenPosIntoWorld(_currentMouse.Position.ToVector2());

            var mouseRectangle = new Rectangle((int) Math.Round(pos.X), (int) Math.Round(pos.Y), 1, 1);
            _isHovering = false;

            if (mouseRectangle.Intersects(Frame))
            {
                _isHovering = true;

                if (_previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Clicked = true;
                    if(_currentMouse.LeftButton == ButtonState.Released)
                        Click?.Invoke(this, new EventArgs());
                }
                else
                {
                    Clicked = false;
                }
            }
            else
            {
                Clicked = false;
            }
        }

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

        #endregion
    }
}
