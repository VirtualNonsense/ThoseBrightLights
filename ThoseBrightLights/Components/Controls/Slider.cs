using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using ThoseBrightLights.Models;
using ThoseBrightLights.Core;
using ThoseBrightLights.Extensions;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Controls
{
    public class Slider : MenuItem
    {
        private Logger _logger;
        private float _value;
        private readonly float _min;
        private readonly float _max;
        private readonly SliderHandle _sliderHandle;
        private MouseState _currentMouse;
        private const float sliderWidth = 4;
        private readonly SoundEffect _soundEffect;
        private float _elapsedTime;
        
        // #############################################################################################################
        // constructor
        // #############################################################################################################
        public Slider(float initialValue, float min, float max, Vector2 position, SliderHandle sliderHandle, List<AnimationHandler> handler, SoundEffect soundEffect, Camera camera) : base(handler, camera)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _value = initialValue;
            _min = min;
            _max = max;
            _sliderHandle = sliderHandle;
            Position = position;
            _sliderHandle.Position = Position;
            _soundEffect = soundEffect;
        }
        // #############################################################################################################
        // events
        // #############################################################################################################
        public event EventHandler OnValueChanged;
        
        // #############################################################################################################
        // properties
        // #############################################################################################################
        public float Value
        {
            get => _value;
            set
            {
                // making sure the value is in bounds
                if (value > _max)
                    _value = _max;
                else if (value < _min)
                    _value = _min;
                else
                    _value = value;
                
                // updating position and making sure it's not outside the slider
                _sliderHandle.Position = new Vector2(getPosition(_value), _position.Y);
            } 
        }

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                // updating handle position
                _sliderHandle.Position = new Vector2(getPosition(_value), _position.Y);
            }
        }

        public override float Layer
        {
            get=>base.Layer;
            set
            {
                base.Layer = value;
                _sliderHandle.Layer = value + .1f;
            }
        }

        private float UpperPos => _position.X + Frame.Width/2f - sliderWidth;
        private float LowerPos => _position.X - Frame.Width/2f + sliderWidth;
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var animationHandler in _handler)
            {
                animationHandler.Draw(spriteBatch);
            }
            _sliderHandle.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // mouse position
            _currentMouse = Mouse.GetState();
            var pos = _camera?.ProjectScreenPosIntoWorld(_currentMouse.Position.ToVector2()) ?? new Vector2(_currentMouse.X, _currentMouse.Y);
            var mouseRectangle = new Rectangle((int) Math.Round(pos.X), (int) Math.Round(pos.Y), 1, 1);
            
            // setting handle state
            if (mouseRectangle.Intersects(_sliderHandle.Frame) && _currentMouse.LeftButton == ButtonState.Pressed)
            {
                _sliderHandle.Drag = true;
            }
            else if (_sliderHandle.Drag && _currentMouse.LeftButton == ButtonState.Released)
            {
                _sliderHandle.Drag = false;
            }
            
            // updating handle position 
            if (_sliderHandle.Drag)
            {
                var newValue = getValue(pos.X);
                if (Math.Abs(newValue - Value) > float.Epsilon)
                {
                    Value = newValue;
                    // sound cooldown 
                    if (75 < _elapsedTime)
                    {
                        _soundEffect?.Play();
                        _elapsedTime = 0;
                    }
                    InvokeOnValueChanged();
                }
            }
            _sliderHandle.Update(gameTime);
        }
        // #############################################################################################################
        // private methods
        // #############################################################################################################
        
        /// <summary>
        /// returns the current value by remapping the x position between the min and max values
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private float getValue(float position)
        {
            return MathExtensions.Remap(position, LowerPos, UpperPos, _min, _max);
        }
        
        /// <summary>
        /// returns the current position by remapping the value between the outer limits
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float getPosition(float value)
        {
            return MathExtensions.Remap(value, _min, _max, LowerPos, UpperPos);
        }
        
        protected virtual void InvokeOnValueChanged()
        {
            OnValueChanged?.Invoke(this, EventArgs.Empty);
        }
        
        // #############################################################################################################
        // subclasses
        // #############################################################################################################

        public class SliderHandle : MenuItem
        {
            public bool Drag;
            // #########################################################################################################
            // constructor
            // #########################################################################################################
            /// <summary>
            /// simple class that represents the handle of the slider
            /// </summary>
            /// <param name="handler"></param>
            /// <param name="camera"></param>
            public SliderHandle(List<AnimationHandler> handler, Camera camera) : base(handler, camera)
            {
            }
            // #########################################################################################################
            // public methods
            // #########################################################################################################

            public override void Draw(SpriteBatch spriteBatch)
            {
                foreach (var animationHandler in _handler)
                {
                    animationHandler.Draw(spriteBatch);
                }
            }

            public override void Update(GameTime gameTime)
            {
                foreach (var animationHandler in _handler)
                {
                    animationHandler.CurrentIndex = Drag? 1 : 0;
                }
            }
        }
    }
}