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
    public class Enemy : Spaceship
    {
        private Logger _logger;
        public InterAction InterAction;
        protected Actor Target;
        protected CooldownAbility ForgetTarget;
        protected CooldownAbility Shoot;
        public bool RotateWeapon = false;
        protected float RotateVelocity;
        /// <summary>
        /// Defines the angle in which the enemy doesn't rotate anymore -> it's close enough
        /// </summary>
        protected float RotationThreshold = MathExtensions.DegToRad(5);

        // TODO: consider enforcing the HitboxFlipped in the setter
        // this way you don't have to check/set it every couple of lines
        // and you can be sure the bool does not lie
        protected bool HitBoxFlipped = false;
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


        public Enemy(AnimationHandler animationHandler, 
                     float maxSpeed = 3,
                     float acceleration = 3,
                     float rotationAcceleration = .1f,
                     float maxRotationSpeed = 10,
                     float health = 50,
                     float? maxHealth = null,
                     float impactDamage = 5,
                     SoundEffect impactSound = null) : base(animationHandler, maxSpeed, acceleration, rotationAcceleration, maxRotationSpeed, health, maxHealth, impactDamage,  impactSound:impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Shoot = new CooldownAbility(2000, _shootTarget);
            InterAction = InterAction.None;
        }

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
        
        
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
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
            if (Target != null && InterAction == InterAction.InView)
            {
                var desiredRotation = MathExtensions.RotationToTarget(target, this);
                if (Math.Abs(desiredRotation - Rotation) > RotationThreshold)
                {
                    float rotationPortion =
                        (float) ((gameTime.ElapsedGameTime.TotalMilliseconds / RotationSpeed) * (2 * Math.PI));
                    //turn clock or anticlockwise
                    var angleToRotate = MathExtensions.Modulo2PiAlsoNegative(desiredRotation - Rotation);
                    if (Math.Abs(angleToRotate) > Math.PI)
                    {
                        if (angleToRotate < 0)
                        {
                            Rotation += rotationPortion;
                        }
                        else
                        {
                            Rotation -= rotationPortion;
                        }
                    }
                    else
                    {
                        if (angleToRotate < 0)
                        {
                            Rotation -= rotationPortion;
                        }
                        else
                        {
                            Rotation += rotationPortion;
                        }
                    }
                }
            }
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