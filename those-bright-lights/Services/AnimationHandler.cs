using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services
{
    public class AnimationHandler
    {
        
        private Animation _animation;
        public AnimationSettings Settings { get; }

        private float _timer;

        private bool _updated;

        private int _currentFrame;

        private ILogger _logger;
        
        public int FrameWidth => _animation.FrameWidth;

        public int FrameHeight => _animation.FrameHeight;

        public Vector2 Position { get; set; }
        
        public Vector2 Origin { get; set; }
        
        public bool IsPlaying { get; set; }

        public Rectangle Frame =>
            new Rectangle(_currentFrame * _animation.FrameWidth,
                0,
                _animation.FrameWidth,
                _animation.FrameHeight);

        public event EventHandler OnAnimationComplete;

        public AnimationHandler(Animation animation, AnimationSettings settings, Vector2? position = null, Vector2? origin = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _animation = animation;
            Settings = settings;
            Position = position ?? new Vector2(0,0);
            Origin = origin ?? new Vector2(0,0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_updated)
            {
                _logger.Error("Need to call 'Update' first");
                throw new Exception("Need to call 'Update' first");
            }

            _updated = false;

            spriteBatch.Draw(
                _animation.Texture,
                Position,
                Frame,
                Settings.Color * Settings.Opacity,
                Settings.Rotation,
                Origin,
                Settings.Scale,
                Settings.SpriteEffects,
                Settings.Layer
                );
        }


        public void Update(GameTime gameTime)
        {
            _updated = true;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > Settings.UpdateInterval)
            {
                _timer = 0f;
                if (!IsPlaying) return;
                if(_currentFrame < _animation.FrameCount-1)
                    _currentFrame++;

                if (_currentFrame >= _animation.FrameCount-1)
                {
                    if(Settings.IsLooping)
                        _currentFrame = 0;
                    OnOnAnimationComplete();
                }
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        protected virtual void OnOnAnimationComplete()
        {
            OnAnimationComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}