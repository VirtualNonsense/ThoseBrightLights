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

        //TODO: find better name, frame is for rectangle
        public (int, float) CurrentFrame => Settings.UpdateList[_currentIndex];

        private ILogger _logger;
        
        public int FrameWidth => Tileset.TileDimX;

        public int FrameHeight => Tileset.TileDimY;

        public Vector2 Position { get; set; }
        
        /// <summary>
        /// Offset that will be added to the position.
        /// Useful when dealing with multiple animationHandler within one class
        /// </summary>
        public Vector2 Offset { get; set; }
        
        public Vector2 Origin { get; set; }
        

        public Rectangle Frame => Tileset.GetFrame((uint)Settings.UpdateList[_currentIndex].Item1);

        public event EventHandler OnAnimationComplete;

        public AnimationHandler(TileSet tileset, AnimationSettings settings, Vector2? position = null, Vector2? origin = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Tileset = tileset;
            Settings = settings;
            Position = position ?? new Vector2(0,0);
            Origin = origin ?? Tileset.GetFrameCenter();
            Offset = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_updated)
            {
                //_logger.Error("Need to call 'Update' first");
                //throw new Exception("Need to call 'Update' first");
            }

            _updated = false;

            spriteBatch.Draw(
                Tileset.Texture,
                Position + Offset,
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
            if (!Settings.IsPlaying) return;
            
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            if (_timer > Settings.UpdateList[CurrentIndex].Item2)
            {
                _timer = 0f;
                if(CurrentIndex < Settings.UpdateList.Count-1)
                    CurrentIndex++;
                else
                {
                    if (Settings.IsLooping)
                    {
                        CurrentIndex = 0;
                        return;
                    }
                    OnOnAnimationComplete();
                    Settings.IsPlaying = false;
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
            return Tileset.GetDataOfFrame(CurrentFrame.Item1);
        }

    }
}