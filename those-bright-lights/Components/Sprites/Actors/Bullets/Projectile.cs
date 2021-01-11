using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Bullets
{
    public class Projectile : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private float _elapsedTime = 0;

        public Projectile(AnimationHandler animationHandler,
                     Vector2 positionSpaceship,
                     float rotation,
                     Particle explosion,
                     Actor parent,
                     SoundEffect midAirSound,
                     SoundEffect impactSound,
                     float damage = 5) 
            : base(animationHandler,parent, explosion, midAirSound, impactSound, damage)
        {
            Rotation = rotation;
            Position = positionSpaceship;
            Velocity = 15;
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

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Spaceship s:
                    if(s == Parent)
                        return;
                    IsRemoveAble = true;
                    InvokeExplosion();
                    break;
                case Tile t:
                    IsRemoveAble = true;
                    InvokeExplosion();
                    break;
                
            }
        }
    }
}