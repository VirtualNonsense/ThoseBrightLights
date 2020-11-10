using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using NLog;

namespace SE_Praktikum.Components.Sprites
{
    public class Tile:IComponent
    {
        Texture2D _texture;
        private readonly Rectangle _frame;
        Vector2 _position;
        private float _layer;
        private readonly float _opacity;
        private Logger _logger;
        private SpriteEffects _spriteEffects = SpriteEffects.None;
        private float _rotation;
        private Vector2 _origin;

        public Tile(Texture2D texture, 
                    Rectangle frame,
                    Vector2 position,
                    float layer,
                    float opacity,
                    int width,
                    int height,
                    TileModifier tileModifier = TileModifier.None)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _texture = texture;
            _frame = frame;
            _position = position;
            _origin = new Vector2(width/2, height/2);
            _layer = layer;
            _opacity = opacity; 
            SetTileModifier(tileModifier);
        }

        public void SetTileModifier(TileModifier tileModifier)
        {
            switch (tileModifier)
            {
                case TileModifier.MirroredHorizontally:
                    _spriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case TileModifier.MirroredVertically:
                    _spriteEffects = SpriteEffects.FlipVertically;
                    break;
                case TileModifier.Turned180Deg:
                    _rotation = (float)(180 * Math.PI / 180);
                    
                    break;
                case TileModifier.MirroredVerticallyTurnedRight:
                    _spriteEffects = SpriteEffects.FlipVertically;
                    _rotation = (float)(90 * Math.PI / 180); 
                    break;
                case TileModifier.TurnedRight:
                    _rotation = (float)(90 * Math.PI / 180); 
                    break;
                case TileModifier.TurnedLeft:
                    _rotation = (float)(-90 * Math.PI / 180); 
                    break;
                case TileModifier.MirroredHorizontallyTurnedRight:
                    _spriteEffects = SpriteEffects.FlipHorizontally;
                    _rotation = (float)(90 * Math.PI / 180); 
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, 
                             _position,
                             _frame,
                             Color.White * _opacity,
                             _rotation,
                             _origin, 
                             1, 
                             _spriteEffects, 
                             _layer);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        
        
    }

    public enum TileModifier
    {
        None,                                 //0000
        MirroredHorizontally,                 //1000
        MirroredVertically,                   //0100
        Turned180Deg,                         //1100
        MirroredVerticallyTurnedRight,        //0010
        TurnedRight,                          //1010      
        TurnedLeft,                           //0110
        MirroredHorizontallyTurnedRight       //1110
    }
}
