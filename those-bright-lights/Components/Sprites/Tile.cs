using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using NLog;
using SE_Praktikum.Services;
using Microsoft.Xna.Framework.Audio;

namespace SE_Praktikum.Components.Sprites
{
    public class Tile:Actor
    {
        private Logger _logger;

        public Tile(AnimationHandler animationHandler, TileModifier tileModifier = TileModifier.None, SoundEffect impactSound = null) : base(animationHandler, impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            base._animationHandler.Origin = new Vector2(_animationHandler.FrameWidth/2f, animationHandler.FrameWidth/2f);
            SetTileModifier(tileModifier);
            Damage = 4;
            _indestructible = true;

        }

        public void SetTileModifier(TileModifier tileModifier)
        {
            switch (tileModifier)
            {
                case TileModifier.MirroredHorizontally:
                    _animationHandler.Settings.SpriteEffects = SpriteEffects.FlipHorizontally;
                    break;
                case TileModifier.MirroredVertically:
                    _animationHandler.Settings.SpriteEffects = SpriteEffects.FlipVertically;
                    break;
                case TileModifier.Turned180Deg:
                    _animationHandler.Settings.Rotation = (float)(180 * Math.PI / 180);
                    
                    break;
                case TileModifier.MirroredVerticallyTurnedRight:
                    _animationHandler.Settings.SpriteEffects = SpriteEffects.FlipVertically;
                    _animationHandler.Settings.Rotation = (float)(90 * Math.PI / 180); 
                    break;
                case TileModifier.TurnedRight:
                    _animationHandler.Settings.Rotation = (float)(90 * Math.PI / 180); 
                    break;
                case TileModifier.TurnedLeft:
                    _animationHandler.Settings.Rotation = (float)(-90 * Math.PI / 180); 
                    break;
                case TileModifier.MirroredHorizontallyTurnedRight:
                    _animationHandler.Settings.SpriteEffects = SpriteEffects.FlipHorizontally;
                    _animationHandler.Settings.Rotation = (float)(90 * Math.PI / 180); 
                    break;
            }
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
