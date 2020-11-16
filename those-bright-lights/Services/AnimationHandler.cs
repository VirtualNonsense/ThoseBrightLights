using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services
{
    public class AnimationHandler
    {
        
        public TileSet Tileset;
        public AnimationSettings Settings { get; }

        private float _timer;

        private bool _updated;

        private int _currentIndex;
        public int CurrentIndex
        {
            get=>_currentIndex;
            //only sets boundaries of animation
            set
            {
                if (value < 0)
                    _currentIndex = 0;
                else if (value >= Settings.UpdateList.Count)
                    _currentIndex = Settings.UpdateList.Count - 1;
                else
                    _currentIndex = value;
            }
        }

        public (int, float) CurrentFrame => Settings.UpdateList[_currentIndex];

        private ILogger _logger;
        
        public int FrameWidth => Tileset.TileDimX;

        public int FrameHeight => Tileset.TileDimY;

        public Vector2 Position { get; set; }
        
        public Vector2 Origin { get; set; }
        

        public Rectangle Frame => Tileset.GetFrame((uint)Settings.UpdateList[_currentIndex].Item1);

        public event EventHandler OnAnimationComplete;

        public AnimationHandler(TileSet tileset, AnimationSettings settings, Vector2? position = null, Vector2? origin = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Tileset = tileset;
            Settings = settings;
            Position = position ?? new Vector2(0,0);
            Origin = origin ?? new Vector2(0,0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_updated)
            {
                _logger.Error("Need to call 'Update' first");
                //throw new Exception("Need to call 'Update' first");
            }

            _updated = false;

            spriteBatch.Draw(
                Tileset.Texture,
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

            if (_timer > Settings.UpdateList[CurrentIndex].Item2)
            {
                _timer = 0f;
                if (!Settings.IsPlaying) return;
                if(CurrentIndex < Tileset.FrameCount-1)
                    CurrentIndex++;

                if (CurrentIndex >= Tileset.FrameCount-1)
                {
                    if(Settings.IsLooping)
                        CurrentIndex = 0;
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