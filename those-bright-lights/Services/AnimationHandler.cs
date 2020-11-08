using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services
{
    public class AnimationHandler
    {
        
        public Animation Animation;
        public AnimationSettings Settings { get; }

        private float _timer;

        private bool _updated;

        private int _currentFrame;

        private ILogger _logger;
        
        public int FrameWidth => Animation.FrameWidth;

        public int FrameHeight => Animation.FrameHeight;

        public Vector2 Position { get; set; }
        
        public Vector2 Origin { get; set; }
        

        public Rectangle Frame =>
            new Rectangle(_currentFrame * Animation.FrameWidth,
                0,
                Animation.FrameWidth,
                Animation.FrameHeight);

        public event EventHandler OnAnimationComplete;

        public AnimationHandler(Animation animation, AnimationSettings settings, Vector2? position = null, Vector2? origin = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Animation = animation;
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
                Animation.Texture,
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
                if (!Settings.IsPlaying) return;
                if(_currentFrame < Animation.FrameCount-1)
                    _currentFrame++;

                if (_currentFrame >= Animation.FrameCount-1)
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

        public Color[] GetDataOfFrame()
        {
            //initialize array with size of one frame
            var data = new Color[FrameWidth * FrameHeight /Animation.FrameCount];
            
            //copy all the framedata to one array
            var allData = new Color[FrameWidth * FrameHeight];
            Animation.Texture.GetData(allData);
            
            //iterate through the pixels
            for (int row = 1; row < FrameHeight; row++)
            {
                for (int column= (Animation.FrameCount-1)*FrameWidth;column<Animation.FrameCount*FrameWidth;column++)
                {
                    data[row * FrameWidth + column] = allData[row*FrameWidth + column + (Animation.FrameCount-1)*FrameWidth];
                }
            }
            
            return data;

        }
    }
}