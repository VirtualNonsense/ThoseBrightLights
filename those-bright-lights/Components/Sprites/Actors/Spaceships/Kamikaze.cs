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
        private readonly Logger _logger;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public Kamikaze(AnimationHandler animationHandler,
                        Polygon viewBox,
                        float acceleration = .1f,
                        float maxSpeed = 10,
                        float rotationAcceleration = .1f,
                        float maxRotationSpeed = 1000,
                        float health = 1,
                        float? maxHealth = null,
                        float impactDamage = 60,
                        SoundEffect impactSound = null) 
            : base(animationHandler, viewBox, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed, health,
                maxHealth, impactDamage, impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Shoot = new CooldownAbility(500, _shootTarget);
        }
        
        // #############################################################################################################
        // public Methods
        // #############################################################################################################

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InterAction != InterAction.InView || Target == null)
            {
                _animationHandler.Settings = _animationHandler.AllSettings[0];
                return;
            }

            _animationHandler.Settings = _animationHandler.AllSettings[1];
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