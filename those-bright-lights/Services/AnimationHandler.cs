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
        

        public Rectangle Frame =>
            new Rectangle(CurrentIndex * Tileset.FrameWidth,
                0,
                Tileset.FrameWidth,
                Tileset.FrameHeight);

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

        public Color[] GetDataOfFrame(int Row, int Column)
        {
            ////initialize array with size of one frame
            //var data = new Color[FrameWidth * FrameHeight];

            ////copy all the framedata to one array
            //var allData = new Color[FrameWidth * FrameHeight* Tileset.FrameCount];
            //Tileset.Texture.GetData(allData);

            ////row offset, number of frames beforehand multiply with width of frame
            //var rowOffset = _currentFrame * FrameWidth;

            ////iterate through all rows and cols
            //for (int row = 0; row < FrameHeight; row++)
            //{
            //    for (int col= 0; col < FrameWidth; col++)
            //    {
            //        data[row * FrameWidth + col] = allData[row * Tileset.Texture.Width + col + rowOffset];
            //    }
            //}

            //return data;



            var _texturewidth = Tileset.TileDimX * Tileset.Columns;
            var _textureheight = Tileset.TileDimY * Tileset.Rows;

            var _tilewidth = Tileset.TileDimX;
            var _tileheight = Tileset.TileDimY;

            //offset for all rows of all tiles above the tilerow we want
            var _rowOffsetForAllTilesAbove = _texturewidth * _tileheight*Row;
            //offset for all pixels in one tile calculated with the columnnumber 
            var _pixelColumnOffset = Column * _tilewidth;

            //array for one tile to copy sth in 
            Color[] _pixelArray = new Color[_tilewidth *_tileheight];
            
            //array filled with all tiles from tileset
            Color[] _allTiles = new Color[_texturewidth*_textureheight];
            Tileset.Texture.GetData(_pixelArray);


            
            //iterating over the tileheight in row steps
            for(int row = 0; row < Tileset.TileDimY; row++)
            {
                //offset for pixels in each row
                var _rowPixelOffset = row * _texturewidth;
                //iterating over the tilewidth in column steps in one row step
                for(int column = 0; row < Tileset.TileDimX; column++)
                {
                    //summing up all offsets until the pixel we need 
                    _pixelArray[row * _tilewidth + column] = _allTiles[_rowOffsetForAllTilesAbove + _pixelColumnOffset + _rowPixelOffset + column];
                }
            }
            return _pixelArray;


        }
    }
}