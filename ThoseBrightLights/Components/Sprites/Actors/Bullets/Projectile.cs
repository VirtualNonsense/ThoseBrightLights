using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.Bullets
{
    public class Projectile : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private float _elapsedTime = 0;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Projectile shot from Minigun
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="positionSpaceship"></param>
        /// <param name="rotation"></param>
        /// <param name="explosion"></param>
        /// <param name="parent"></param>
        /// <param name="midAirSound"></param>
        /// <param name="impactSound"></param>
        /// <param name="damage"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="indestructible"></param>
        public Projectile(AnimationHandler animationHandler,
                     Vector2 positionSpaceship,
                     float rotation,
                     Particle explosion,
                     Actor parent,
                     SoundEffect midAirSound,
                     SoundEffect impactSound,
                     float damage = 5,
                     float health = 1,
                     float? maxHealth = null,
                     bool indestructible = false) 
            : base(animationHandler,parent, explosion, midAirSound, impactSound, damage, health: health, maxHealth: maxHealth, indestructible: indestructible)
        {
            Rotation = rotation;
            Position = positionSpaceship;
            Speed = 15;
            Acceleration = 0;
            _spaceShipVelocity = Vector2.Zero;
            MaxTime = 5;
            MidAirSoundCooldown = 1000;
            TimeSinceUsedMidAir = MidAirSoundCooldown;
        }
        
        // #############################################################################################################
        // public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            TimeSinceUsedMidAir += gameTime.ElapsedGameTime.Milliseconds;
            Position = Movement(_spaceShipVelocity,_elapsedTime);
            if (MidAirSoundCooldown < TimeSinceUsedMidAir)
            {
                TimeSinceUsedMidAir = 0;
                //MidAirSound?.Play();
            }
            base.Update(gameTime);
        }

    }
}