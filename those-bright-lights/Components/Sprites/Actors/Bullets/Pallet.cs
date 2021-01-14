using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Bullets
{
    public class Pallet : Bullet
    {
        
        private readonly Vector2 _spaceShipVelocity;
        private float _elapsedTime = 0;

        public Pallet(AnimationHandler animationHandler,
            Vector2 positionSpaceship,
            float rotation,
            Particle explosion,
            Actor parent,
            SoundEffect midAirSound,
            SoundEffect impactSound,
            float damage = 1,
            float health = 1,
            float? maxHealth = null,
            bool indestructible = false) 
            : base(animationHandler,parent, explosion, midAirSound, impactSound, damage, health: health, maxHealth: maxHealth, indestructible: indestructible)
        {
            Rotation = rotation;
            Position = positionSpaceship;
            Speed = 8;
            Acceleration = 0;
            _spaceShipVelocity = Vector2.Zero;
            MaxTime = 5;
            Damage = 1;
            MidAirSoundCooldown = 1000;
            TimeSinceUsedMidAir = MidAirSoundCooldown;
        }
        
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