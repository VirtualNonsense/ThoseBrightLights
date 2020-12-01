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
    public class MenuButton : MenuItem
    {
        #region Fields

        private Logger _logger;
        
        private readonly SpriteFont _font;

        private bool _isHovering;
        
        private MouseState _currentMouse;

        private MouseState _previousMouse;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color TextColor { get; set; }

        
        public string Text { get; set; }

        #endregion

        #region Methods

        public MenuButton(List<AnimationHandler> handler,
                          SpriteFont font,
                          Color? textColor = null,
                          Vector2? position = null,
                          bool useCenterAsOrigin = true,
                          Camera camera = null,
                          string text = "") : base(handler, camera, useCenterAsOrigin)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _position = position ?? Vector2.Zero;
            _font = font;
            TextColor = textColor ?? Color.CornflowerBlue;
            Text = text;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var pos = _camera is null? new Vector2(_currentMouse.X, _currentMouse.Y) : 
                                              _camera.ProjectScreenPosIntoWorld(_currentMouse.Position.ToVector2());

            var mouseRectangle = new Rectangle((int) Math.Round(pos.X), (int) Math.Round(pos.Y), 1, 1);
            _isHovering = false;
            _logger.Debug(pos);
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

        #endregion
    }
}
