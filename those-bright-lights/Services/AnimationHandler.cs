using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services
{
    public class AnimationHandler
    {
        private Vector2 _position;
        private Vector2 _origin;
        private Vector2 _offset;

        public TileSet Tileset;
        private AnimationSettings _settings;

        private float _timer;

        private bool _updated;

        private int _currentIndex;
        public int CurrentIndex
        {
            get=>_currentIndex;
            //only sets boundaries of animation
            set
            {
                if (_currentIndex == value) return;
                if (value < 0)
                    _currentIndex = 0;
                else if (value >= _settings.UpdateList.Count)
                    _currentIndex = _settings.UpdateList.Count - 1;
                else
                    _currentIndex = value;

                CurrentHitBox = Tileset.GetHitBox(_currentIndex);
                HitBoxTransition();
            }
        }

        //TODO: find better name, frame is for rectangle
        public (int, float) CurrentFrame => _settings.UpdateList[_currentIndex];

        public Polygon[] CurrentHitBox
        {
            get;
            private set;
        }

        private ILogger _logger;
        
        public int FrameWidth => Tileset.TileDimX;

        public int FrameHeight => Tileset.TileDimY;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                if (CurrentHitBox==null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Position = _position + _offset;
                }
            }
        }

        public float Rotation
        {
            get => _settings.Rotation;
            set
            {
                _settings.Rotation = value;
                if (CurrentHitBox==null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Rotation = value;
                }
            }
        }

        public SpriteEffects SpriteEffects
        {
            get=>_settings.SpriteEffects;
            set=>_settings.SpriteEffects=value;
        }

        public Color Color
        {
            get=>_settings.Color;
            set=>_settings.Color=value;
        }
        

        public float Scale
        {
            get => _settings.Scale;
            set => _settings.Scale = value;
        }

        /// <summary>
        /// Offset that will be added to the position.
        /// Useful when dealing with multiple animationHandler within one class
        /// </summary>
        public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                if (CurrentHitBox==null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Position = _position + _offset;
                }
            }
        }

        public Vector2 Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                if (CurrentHitBox==null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Origin = _origin;
                }
            }
        }

        public float Opacity
        {
            get => _settings.Opacity;
            set => _settings.Opacity = value;
        }

        public float Layer
        {
            
            get => _settings.Layer;
            set
            {
                _settings.Layer = value;
                if (CurrentHitBox==null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Layer = _settings.Layer;
                }
            }
        }
        

        public Rectangle Frame => Tileset.GetFrame((uint)_settings.UpdateList[_currentIndex].Item1);

        public event EventHandler OnAnimationComplete;

        public AnimationHandler(TileSet tileset, AnimationSettings settings, Vector2? position = null, Vector2? origin = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Tileset = tileset;
            _settings = settings;
            Position = position ?? new Vector2(0,0);
            Origin = origin ?? new Vector2(0,0);
            Offset = Vector2.Zero;
            CurrentHitBox = Tileset.GetHitBox(_currentIndex);
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
                _settings.Color * _settings.Opacity,
                _settings.Rotation,
                Origin,
                _settings.Scale,
                _settings.SpriteEffects,
                _settings.Layer
                );
        }


        public void Update(GameTime gameTime)
        {
            _updated = true;
            if (!_settings.IsPlaying) return;
            
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            if (_timer > _settings.UpdateList[CurrentIndex].Item2)
            {
                _timer = 0f;
                if(CurrentIndex < _settings.UpdateList.Count-1)
                    CurrentIndex++;
                else
                {
                    if (_settings.IsLooping)
                    {
                        CurrentIndex = 0;
                        return;
                    }
                    InvokeOnAnimationComplete();
                    _settings.IsPlaying = false;
                }
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        protected virtual void InvokeOnAnimationComplete()
        {
            OnAnimationComplete?.Invoke(this, EventArgs.Empty);
        }
        
        [Obsolete]
        public Byte[] GetDataOfFrame()
        {
            return Tileset.GetDataOfFrame(CurrentFrame.Item1);
        }
        private void HitBoxTransition()
        {
            if (CurrentHitBox == null) return;
            foreach (var polygon in CurrentHitBox)
            {
                    polygon.Position = Position + Offset;

                    polygon.Origin = Origin;

                    polygon.Rotation = Rotation;

                    polygon.Layer = Layer;


            }
        }

    }
}