using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using ThoseBrightLights.Core;
using ThoseBrightLights.Services;


namespace ThoseBrightLights.Components.Controls
{
    public class MenuButton : MenuItem
    {

        private Logger _logger;
        
        private readonly SpriteFont _font;
        private readonly int _textOffSetWhenPressed;

        private bool _isHovering;
        
        private MouseState _currentMouse;

        private MouseState _previousMouse;

        private SoundEffect _soundEffect;
        
        // #############################################################################################################
        // constructor
        // #############################################################################################################
        /// <summary>
        /// Simple button with a click event
        /// </summary>
        /// <param name="handler">A list with properly configured animation handler. Frame zero should contain an
        /// unpressed button tile. frame 1 a pressed one (if necessary)
        /// </param>
        /// <param name="font">Monogamefont. its necessary for writing text</param>
        /// <param name="soundEffect">Click sound</param>
        /// <param name="textColor">Text color. With the current rendering settings the color will be ignored. unfortunately :/</param>
        /// <param name="position"></param>
        /// <param name="camera">The button needs a way to figure out the absolute position of the mouse in order to
        /// process click events. the camera is used to perform this calculation.</param>
        /// <param name="text">Button text</param>
        /// <param name="textOffSetWhenPressed">move button text down</param>
        /// <param name="enabled">determines whether the button is clickable or not</param>
        public MenuButton(List<AnimationHandler> handler,
                          SpriteFont font,
                          SoundEffect soundEffect,
                          Color? textColor = null,
                          Vector2? position = null,
                          Camera camera = null,
                          string text = "",
                          int textOffSetWhenPressed = 0,
                          bool enabled = true) : base(handler, camera)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _position = position ?? Vector2.Zero;
            _font = font;
            _soundEffect = soundEffect; 

            _textOffSetWhenPressed = textOffSetWhenPressed;
            TextColor = textColor ?? Color.CornflowerBlue;
            Text = text;
            Enabled = true;
        }
        
        // #############################################################################################################
        // Events
        // #############################################################################################################
        /// <summary>
        /// triggers when the player clicks the button
        /// </summary>
        public event EventHandler Click;
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        /// <summary>
        /// current button state
        /// </summary>
        public bool Clicked { get; private set; }
        
        /// <summary>
        /// Unfortunately the renderer will ignore this feature due to the chosen basic effect 
        /// </summary>
        public Color TextColor { get; set; }
        
        /// <summary>
        /// Button Text
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Clickable state of the button
        /// </summary>
        public bool Enabled { get; set; }
        
        // #############################################################################################################
        // Methods
        // #############################################################################################################
        public override void Draw(SpriteBatch spriteBatch)
        {
            var co = Color.White;
            
            // Mouse over button?
            if (_isHovering)
                co = Color.Gray; // set tinting color

            
            if (Clicked)
            {
                // clicked color effect
                co = Color.Black;
            }
            
            // update each button tile
            foreach (var handler in _handler)
            {
                handler.Color = co;
                // the clicked button tile should be on position 1 so it will be set when the button is clicked or disabled
                handler.CurrentIndex = Clicked || !Enabled? 1 : 0;
                handler.Draw(spriteBatch);
            }
            
            // text placement if text is sett
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Frame.X + (Frame.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Frame.Y + (Frame.Height / 2)) - (_font.MeasureString(Text).Y / 2) +
                        (Clicked ? _textOffSetWhenPressed : 0);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), TextColor);
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            // update mouse states
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            
            // get position on current layer when camera is set
            var pos = _camera?.ProjectScreenPosIntoWorld(_currentMouse.Position.ToVector2()) ?? new Vector2(_currentMouse.X, _currentMouse.Y);

            // create a rectangle for ez overlap calculation
            var mouseRectangle = new Rectangle((int) Math.Round(pos.X), (int) Math.Round(pos.Y), 1, 1);
            // reset isHovering state
            _isHovering = false;
            
            // check if the button is enabled and for intersection with mouse
            if (mouseRectangle.Intersects(Frame) && Enabled)
            {
                // updating hovering flag
                _isHovering = true;
                
                // click event and state handling
                if (_previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Clicked = true;
                    if(_currentMouse.LeftButton == ButtonState.Released)
                    {
                        _soundEffect?.Play();
                        Click?.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                {
                    Clicked = false;
                }
            }
            else
            {
                // reset flag when player leaves button area 
                Clicked = false;
            }
        }
    }
}
