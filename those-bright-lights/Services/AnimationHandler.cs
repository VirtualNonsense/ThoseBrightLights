using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services
{
    public class AnimationHandler
    {
        private Vector2 _position;
        private Vector2 _origin;
        private Vector2 _offset;

        public TileSet TileSet;
        public List<AnimationSettings> AllSettings;
        public AnimationSettings Settings;

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
                else if (value >= Settings.UpdateList.Count)
                    _currentIndex = Settings.UpdateList.Count - 1;
                else
                    _currentIndex = value;

                CurrentHitBox = TileSet.GetHitBox(Settings.UpdateList[_currentIndex].Item1);
                HitBoxTransition();
                HitBoxUpdate();
            }
        }

        //TODO: find better name, frame is for rectangle
        public (int, float) CurrentFrame => Settings.UpdateList[_currentIndex];

        public Polygon[] CurrentHitBox
        {
            get;
            private set;
        }

        private ILogger _logger;
        
        public int FrameWidth => TileSet.TileDimX;

        public int FrameHeight => TileSet.TileDimY;

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
            get => Settings.Rotation;
            set
            {
                if (Math.Abs(value - Settings.Rotation) < float.Epsilon) return;
                    Settings.Rotation = value > 0? 
                        (float) (value  % (2 * Math.PI)) : 
                        (float)(2 * Math.PI + value % (2 * Math.PI));
                    foreach (var setting in AllSettings)
                    {
                        setting.Rotation = Settings.Rotation;
                    }
                    if (CurrentHitBox==null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Rotation = Settings.Rotation;
                }
            }
        }

        public bool IsPlaying
        {
            get => Settings.IsPlaying;
            set => Settings.IsPlaying = value;
        }

        public SpriteEffects SpriteEffects
        {
            get=>Settings.SpriteEffects;
            set
            {
                if (value == SpriteEffects) return;
                if (SpriteEffects != SpriteEffects.None)
                {
                    // resetting HitBox to make transition easier
                    CurrentHitBox = TileSet.GetHitBox(Settings.UpdateList[_currentIndex].Item1);
                    HitBoxTransition();
                }

                foreach (var setting in AllSettings)
                {
                    setting.SpriteEffects = value;
                }
                Settings.SpriteEffects = value;
                HitBoxUpdate();
            }
        }

        public Color Color
        {
            get=>Settings.Color;
            set=>Settings.Color=value;
        }
        

        public float Scale
        {
            get => Settings.Scale;
            set
            {
                foreach (var setting in AllSettings)
                {
                    setting.Scale = value;
                }
                Settings.Scale = value;
                if (CurrentHitBox == null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Scale = value;
                }
            }
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
            set => _origin = value;
        }

        public Vector2 PointOfRotation
        {
            get;
            set;
        }

        public float Opacity
        {
            get => Settings.Opacity;
            set
            {
                foreach (var setting in AllSettings)
                {
                    setting.Opacity = value;
                }
                Settings.Opacity = value;
            }
        }

        public float Layer
        {
            
            get => Settings.Layer;
            set
            {
                foreach (var setting in AllSettings)
                    setting.Layer = value;
                Settings.Layer = value;
                if (CurrentHitBox==null) return;
                foreach (var polygon in CurrentHitBox)
                {
                    polygon.Layer = Settings.Layer;
                }
            }
        }
        

        public Rectangle Frame => TileSet.GetFrame((uint)Settings.UpdateList[_currentIndex].Item1);

        public event EventHandler OnAnimationComplete;
        public event EventHandler<AnimationProgressArgs> OnAnimationProgressUpdate;

        public AnimationHandler(TileSet tileSet, List<AnimationSettings> settings, Vector2? position = null, Vector2? origin = null)
        {
            _logger = LogManager.GetCurrentClassLogger();
            TileSet = tileSet;
            AllSettings = settings;
            Settings = settings[0];
            _position = position ?? new Vector2(0,0);
            _origin = origin ?? new Vector2(0,0);
            _offset = Vector2.Zero;
            PointOfRotation = Vector2.Zero;
            CurrentHitBox = TileSet.GetHitBox(Settings.UpdateList[_currentIndex].Item1);
            HitBoxTransition();
            HitBoxUpdate();
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
                TileSet.Texture,
                Position + Offset + PointOfRotation,
                Frame,
                Settings.Color * Settings.Opacity,
                Settings.Rotation,
                Origin + PointOfRotation,
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
                    CurrentIndex = 0;
                    if (Settings.IsLooping)
                    {
                        return;
                    }
                    InvokeOnAnimationComplete();
                    Settings.IsPlaying = false;
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
        

        private void HitBoxTransition()
        {
            if (CurrentHitBox == null) return;
            foreach (var polygon in CurrentHitBox)
            {
                if (polygon.Origin != Vector2.Zero) return;
                polygon.MoveVertices(-Origin);
            }
        }
        
        
        private void HitBoxUpdate()
        {
            if (CurrentHitBox == null) return;
            foreach (var polygon in CurrentHitBox)
            {
                polygon.Position = Position + Offset;

                polygon.Rotation = Rotation;

                polygon.Layer = Layer;

                polygon.Scale = Scale;
            }
            switch (SpriteEffects)
            {
                case SpriteEffects.None:
                    break;
                case SpriteEffects.FlipHorizontally:
                    CurrentHitBox = CurrentHitBox.MirrorHorizontal(Position);
                    break;
                case SpriteEffects.FlipVertically:
                    CurrentHitBox = CurrentHitBox.MirrorVertical(Position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void InvokeOnAnimationProgressUpdate()
        {
            OnAnimationProgressUpdate?.Invoke(this, new AnimationProgressArgs{Progress = (float)_currentIndex/Settings.UpdateList.Count});
        }

        public class AnimationProgressArgs : EventArgs
        {
            public float Progress;
        }
    }
}