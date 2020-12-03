using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Core;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Controls
{
    public class Slider : MenuItem
    {
        private Logger _logger;
        private float _value;
        private readonly float _min;
        private readonly float _max;
        private readonly SliderBlade _sliderBlade;
        private MouseState _previousMouse;
        private MouseState _currentMouse;
        private const float sliderWidth = 4;
        
        public event EventHandler OnValueChanged;
        public float Value
        {
            get => _value;
            set
            {
                if (value > _max)
                    value = _max;
                else if (value < _min)
                    value = _min;
                else
                    _value = value;
                _sliderBlade.Position = new Vector2(getPosition(_value), _position.Y);
            } 
        }

        public sealed override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                _sliderBlade.Position = new Vector2(getPosition(_value), _position.Y);
            }
        }

        public override float Layer
        {
            get=>base.Layer;
            set
            {
                base.Layer = value;
                _sliderBlade.Layer = value + .1f;
            }
        }

        private float UpperPos => _position.X + Frame.Width/2f - sliderWidth;
        private float LowerPos => _position.X - Frame.Width/2f + sliderWidth;
        
        public Slider(float initialValue, float min, float max, Vector2 position, SliderBlade sliderBlade, List<AnimationHandler> handler, Camera camera) : base(handler, camera)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _value = initialValue;
            _min = min;
            _max = max;
            _sliderBlade = sliderBlade;
            Position = position;
            _sliderBlade.Position = Position;
        }
         

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var animationHandler in _handler)
            {
                animationHandler.Draw(spriteBatch);
            }
            _sliderBlade.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var pos = _camera is null? new Vector2(_currentMouse.X, _currentMouse.Y) : 
                _camera.ProjectScreenPosIntoWorld(_currentMouse.Position.ToVector2());

            var mouseRectangle = new Rectangle((int) Math.Round(pos.X), (int) Math.Round(pos.Y), 1, 1);

            if (mouseRectangle.Intersects(_sliderBlade.Frame) && _currentMouse.LeftButton == ButtonState.Pressed)
            {
                _sliderBlade.Drag = true;
            }
            else if (_sliderBlade.Drag && _currentMouse.LeftButton == ButtonState.Released)
            {
                _sliderBlade.Drag = false;
            }

            if (_sliderBlade.Drag)
            {
                Value = getValue(pos.X);
                ValueChanged();
            }
            _sliderBlade.Update(gameTime);
            
        }

        private float getValue(float position)
        {

            return MathExtensions.Remap(position, LowerPos, UpperPos, _min, _max);
        }
        
        private float getPosition(float value)
        {

             return MathExtensions.Remap(value, _min, _max, LowerPos, UpperPos);
        }

        protected virtual void ValueChanged()
        {
            OnValueChanged?.Invoke(this, EventArgs.Empty);
        }


        public class SliderBlade : MenuItem
        {
            public bool Drag;
            private Logger _logger;
            public SliderBlade(List<AnimationHandler> handler, Camera camera) : base(handler, camera)
            {
                _logger = LogManager.GetCurrentClassLogger();
            }

            public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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