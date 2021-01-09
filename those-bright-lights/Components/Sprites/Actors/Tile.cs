using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors
{
    public class Tile:Actor
    {
        private Logger _logger;

        public Tile(AnimationHandler animationHandler, TileModifier tileModifier = TileModifier.None, SoundEffect impactSound = null) : base(animationHandler, impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            SetTileModifier(tileModifier);
            Damage = 4;
            _indestructible = true;
            _animationHandler.PointOfRotation =
                new Vector2(_animationHandler.FrameWidth / 2f, _animationHandler.FrameHeight / 2f);

        }

        public void SetTileModifier(TileModifier tileModifier)
        {
            switch (tileModifier)
            {
                case TileModifier.MirroredHorizontally:
                    _animationHandler.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case TileModifier.MirroredVertically:
                    _animationHandler.SpriteEffects = SpriteEffects.FlipVertically;
                    break;
                case TileModifier.Turned180Deg:
                    _animationHandler.Rotation = (float)(180 * Math.PI / 180);
                    
                    break;
                case TileModifier.MirroredVerticallyTurnedRight:
                    _animationHandler.SpriteEffects = SpriteEffects.FlipVertically;
                    _animationHandler.Rotation = (float)(90 * Math.PI / 180); 
                    break;
                case TileModifier.TurnedRight:
                    _animationHandler.Rotation = (float)(90 * Math.PI / 180); 
                    break;
                case TileModifier.TurnedLeft:
                    _animationHandler.Rotation = (float)(-90 * Math.PI / 180); 
                    break;
                case TileModifier.MirroredHorizontallyTurnedRight:
                    _animationHandler.SpriteEffects = SpriteEffects.FlipHorizontally;
                    _animationHandler.Rotation = (float)(90 * Math.PI / 180); 
                    break;
            }
        }

        protected override bool InteractAble(Actor other)
        {
            // tile doesnt care about interactions
            // a destroyable tile might be of use on some point
            return false;
        }
        
        protected override void ExecuteInteraction(Actor other)
        {
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
