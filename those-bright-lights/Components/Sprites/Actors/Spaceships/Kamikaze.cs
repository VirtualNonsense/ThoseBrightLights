using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public class Kamikaze : EnemyWithViewbox
    {
        private Logger _logger;
        public Kamikaze(AnimationHandler animationHandler,
                        Polygon viewBox,
                        float acceleration = .1f,
                        float maxSpeed = 10,
                        float rotationAcceleration = .1f,
                        float maxRotationSpeed = 10,
                        float health = 1,
                        float? maxHealth = null,
                        float impactDamage = 60,
                        SoundEffect impactSound = null) : base(animationHandler, viewBox, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed, health, maxHealth, impactDamage, impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Debug($"{health}");
            Shoot = new CooldownAbility(500, _shootTarget);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (I != InterAction.InView || Target == null)
            {
                return;
            }
            var t = gameTime.ElapsedGameTime.Milliseconds/10f;
            var direction = Target.Position - Position;
            var velocity = Velocity;
            direction.Normalize();
            velocity += direction * Acceleration * t * t;
            
            var newVelocity = velocity.Length();
            
            if (newVelocity > 0)
            {
                velocity /= newVelocity;
                newVelocity = 1 * newVelocity * (1 - (newVelocity / MaxSpeed));
                if(newVelocity < 0)
                    _logger.Warn($"newVelocity < 0 consider rising MaxSpeed or decreasing acceleration");
                
                Velocity = Math.Abs(newVelocity) * velocity;
            }
            
            DeltaPosition = Velocity * t;
            Position += DeltaPosition;


        }
    }
}