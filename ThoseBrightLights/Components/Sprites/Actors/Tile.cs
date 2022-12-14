using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using ThoseBrightLights.Components.Sprites.Actors.Bullets;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors
{
    /// <summary>
    /// Whenever a tile is needed
    /// </summary>
    public class Tile: Actor
    {
        //field
        private readonly Logger _logger;

        //Constructor
        public Tile(AnimationHandler animationHandler, TileModifier tileModifier = TileModifier.None, SoundEffect impactSound = null, float impactDamage = 5) : base(animationHandler, impactSound, impactDamage: impactDamage)
        {
            _logger = LogManager.GetCurrentClassLogger();
            SetTileModifier(tileModifier);
            Indestructible = true;
            _animationHandler.PointOfRotation =
                new Vector2(_animationHandler.FrameWidth / 2f, _animationHandler.FrameHeight / 2f);

        }

        // tile modifications 
        private void SetTileModifier(TileModifier tileModifier)
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
                case TileModifier.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileModifier), tileModifier, null);
            }
        }

        // Make tiles interactable with actors
        protected override bool InteractAble(Actor other)
        {
            return !Indestructible && base.InteractAble(other);
        }
        
        // Interaction with bullets and the ship
        protected override void ExecuteInteraction(Actor other)
        {
            if (Indestructible) return;
            switch (other)
            {
                case Bullet b:
                    LastAggressor = b;
                    Tool = b;
                    Health -= b.Damage;
                    break;
                case Spaceship s:
                    LastAggressor = s;
                    Tool = s;
                    Health -= s.Damage;
                    break;
            }
            _logger.Debug(Health);
        }
        

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.TileDiedEventArgs();
        }
    }
    
    
    // Enumeration for tile modifications (binary)
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
