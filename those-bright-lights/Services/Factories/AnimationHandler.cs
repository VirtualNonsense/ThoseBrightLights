using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class AnimationHandler
    {
        
        private Animation _animation;
        private readonly AnimationSettings _settings;

        private float _timer;

        private bool _updated;

        private int _currentFrame;

        public int FrameWidth => _animation.FrameWidth;

        public int FrameHeight => _animation.FrameHeight;

        public Vector2 Position { get; set; }

        public float Layer { get; set; }

        public event EventHandler OnAnimationComplete;

        public AnimationHandler(Animation animation, AnimationSettings settings)
        {
            _animation = animation;
            _settings = settings;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_updated)
            {
                throw new Exception("Need to call 'Update' first");
            }

            _updated = false;

            spriteBatch.Draw(_animation.Texture,
                Position,
                new Rectangle(_currentFrame * _animation.FrameWidth,
                    0,
                    _animation.FrameWidth,
                    _animation.FrameHeight),
                Color.White,
                0f,
                new Vector2(0, 0),
                1f,
                SpriteEffects.None,
                Layer);
        }


        public void Update(GameTime gameTime)
        {
            _updated = true;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _settings.UpdateInterval)
            {
                _timer = 0f;
                if(_currentFrame < _animation.FrameCount)
                    _currentFrame++;

                if (_currentFrame >= _animation.FrameCount)
                {
                    if(_settings.IsLooping)
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