using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Laser : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private float _elapsedTime = 0;

        public Laser(AnimationHandler animationHandler,Vector2 positionSpaceship,float rotation, Particle explosion, Sprite parent, SoundEffect midAirSound, SoundEffect impactSound) : base(animationHandler, explosion, midAirSound, impactSound)
        {
            Rotation = rotation;
            Parent = parent;
            Position = positionSpaceship;
            Velocity = 5;
            Acceleration = 0;
            _spaceShipVelocity = Vector2.Zero;
            MaxTime = 5;
            Damage = 5;
            _midAirSoundCooldown = 1000;
            _timeSinceUsedMidAir = _midAirSoundCooldown;
        }
        
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            _timeSinceUsedMidAir += gameTime.ElapsedGameTime.Milliseconds;
            Position = Movement(_spaceShipVelocity,_elapsedTime);
            if (_midAirSoundCooldown < _timeSinceUsedMidAir)
            {
                _timeSinceUsedMidAir = 0;
                _midAirSound?.Play();
            }
            base.Update(gameTime);
        }
    }
}