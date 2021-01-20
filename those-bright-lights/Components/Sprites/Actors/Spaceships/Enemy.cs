using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public abstract class Enemy : Spaceship
    {
        private Logger _logger;
        protected InterAction InterAction;
        protected Actor Target;
        protected CooldownAbility ForgetTarget;
        protected CooldownAbility Shoot;
        protected bool RotateWeapon = false;
        protected float RotateVelocity;
        /// <summary>
        /// Defines the angle in which the enemy doesn't rotate anymore -> it's close enough
        /// </summary>
        protected readonly float RotationThreshold = MathExtensions.DegToRad(5);
        protected bool HitBoxFlipped = false;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// base class for all enemies
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="acceleration"></param>
        /// <param name="rotationAcceleration"></param>
        /// <param name="maxRotationSpeed"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="impactDamage"></param>
        /// <param name="impactSound"></param>
        protected Enemy(AnimationHandler animationHandler, 
            float maxSpeed = 3,
            float acceleration = 3,
            float rotationAcceleration = .1f,
            float maxRotationSpeed = 1000,
            float health = 50,
            float? maxHealth = null,
            float impactDamage = 5,
            SoundEffect impactSound = null) : base(animationHandler, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed, health, maxHealth, impactDamage,  impactSound:impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Shoot = new CooldownAbility(2000, _shootTarget);
            InterAction = InterAction.None;
        }
        
        // #############################################################################################################
        // public Methods
        // #############################################################################################################
        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                if (Rotation < 3 * Math.PI/2 && Rotation > Math.PI/2)
                {
                    if (HitBoxFlipped) return;
                    HitBoxFlipped = true;
                }
                else if (HitBoxFlipped)
                {
                    HitBoxFlipped = false;
                }
            }
        }

        // #############################################################################################################
        // protected/private Methods
        // #############################################################################################################
        protected virtual void _shootTarget()
        {
            if (InterAction != InterAction.InView || Target == null) return;
            var rotation = Rotation;

            var directVector = Target.Position - Position;
            var directAngle = MathExtensions.GetVectorRotation(directVector);
            var viewVector = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * directVector.Length();
            var viewAngle = MathExtensions.GetVectorRotation(viewVector);
            var offSetRotation = MathExtensions.Modulo2PiAlsoNegative(directAngle - viewAngle);
            if (directAngle < 0)
                rotation -= offSetRotation;
            else if(directAngle > 0)
                rotation += offSetRotation;
        
            // Weapon currently grabs parents position and rotation
            // this will change most likely give me a bit time to figure out how to implement it properly
            ShootCurrentWeapon();
            Target = null;
        }

        protected override bool InteractAble(Actor other)
        {
            var t = base.InteractAble(other);
            if (t)
                InterAction = InterAction.BodyCollision;
            return t;
        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Player p:
                    Target = p;
                    switch (InterAction)
                    {
                        case InterAction.BodyCollision:
                            base.ExecuteInteraction(other);
                            break;
                        case InterAction.None:
                            break;
                        case InterAction.InView:
                            break;
                        case InterAction.InViewAndBodyCollision:
                            base.ExecuteInteraction(other);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    base.ExecuteInteraction(other);
                    break;
            }
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.EnemyDiedEventArgs();
        }

        protected virtual void Rotate(Actor target, GameTime gameTime)
        {
            //only rotate when enemy has target in view
            if (Target == null || InterAction != InterAction.InView) return;
            var desiredRotation = MathExtensions.RotationToTarget(target, this);
            //stop wiggling
            if (!(Math.Abs(desiredRotation - Rotation) > RotationThreshold)) return;
            var rotationPortion =
                (float) ((gameTime.ElapsedGameTime.TotalMilliseconds / MaxRotationSpeed) * (2 * Math.PI));
            //turn clock or anticlockwise
            var angleToRotate = MathExtensions.Modulo2PiAlsoNegative(desiredRotation - Rotation);
            Rotation += Math.Sign(Math.Abs(angleToRotate-Math.PI)) * Math.Sign(angleToRotate) * rotationPortion;
        }

    }

    public enum InterAction
    {
        None,
        InView,
        BodyCollision,
        InViewAndBodyCollision
    }
}