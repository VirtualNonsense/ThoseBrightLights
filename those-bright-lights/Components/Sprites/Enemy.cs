using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SE_Praktikum.Components.Sprites
{
    public class Enemy : Spaceship
    {
        private Logger _logger;
        public Polygon ViewBox;
        protected InterAction I;
        protected Actor Target;
        protected CooldownAbility ForgetTarget;
        protected CooldownAbility Shoot;
        public bool RotateAndShoot = false;
        protected float RotateVelocity;
        /// <summary>
        /// Defines the angle in which the enemy doesn't rotate anymore -> it's close enough
        /// </summary>
        private float _rotationThreshold = MathExtensions.DegToRad(5);
        

        private bool _hitBoxFlipped = false;
        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                if (Rotation < 3 * Math.PI/2 && Rotation > Math.PI/2)
                {
                    if (_hitBoxFlipped) return;
                    ViewBox = ViewBox.MirrorSingleVertical(Position);
                    _hitBoxFlipped = true;
                }
                else if (_hitBoxFlipped)
                {
                    ViewBox = ViewBox.MirrorSingleVertical(Position);
                    _hitBoxFlipped = false;
                }
            }
        }


        public Enemy(AnimationHandler animationHandler, float speed = 3, float health = 50, SoundEffect impactSound = null) : base(animationHandler, speed, health, impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Shoot = new CooldownAbility(2000, _shootTarget);
            I = InterAction.None;
        }

        protected virtual void _shootTarget()
        {
            if (I == InterAction.InView && Target != null)
            {
                float rotation = Rotation;

                var directVector = Target.Position - Position;
                var directAngle = MathExtensions.GetVectorRotation(directVector);
                var viewVector = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * directVector.Length();
                var viewAngle = MathExtensions.GetVectorRotation(viewVector);
                var offSetRotation = MathExtensions.Modulo2PiAlsoNegative(directAngle - viewAngle);
                if (directAngle < 0)
                    rotation -= offSetRotation;
                else if(directAngle > 0)
                    rotation += offSetRotation;
        
                Weapons[CurrentWeapon].Fire();
                // InvokeOnShoot(b);
            }
        }
        
        
        public override void Update(GameTime gameTime)
        {
            if(RotateAndShoot)
                Rotate(Target, gameTime);
            Vector2 velocity = Vector2.Zero;

            ViewBox.Position = Position;
            ViewBox.Rotation = Rotation;
            ViewBox.Layer = Layer;
            base.Update(gameTime);
        }

        

        protected override void InvokeOnTakeDamage(float damage)
        {
            _logger.Info(Health);
            Health -= damage;
            base.InvokeOnTakeDamage(damage);
        }
        

        protected override bool InteractAble(Actor other)
        {
            switch (other)
            {
                case Player p:
                    if (p.HitBox.Any(polygon => ViewBox.Overlap(polygon)))
                    {
                        I = InterAction.InView;
                        return true;
                    }

                    break;
            }
            var t = base.InteractAble(other);
            if (t)
                I = InterAction.BodyCollision;
            return t;
        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Player p:
                    Target = p;
                    switch (I)
                    {
                        case InterAction.BodyCollision:
                            Health -= p.Damage;
                            break;
                    }
                    break;
                default:
                    if (other.Parent == this) return;
                    Health -= other.Damage;
                    _logger.Debug($"health {Health}");
                    _impactSound?.Play();
                    break;
            }
        }

        protected void Rotate(Actor target, GameTime gameTime)
        {
            if (Target != null && I == InterAction.InView)
            {
                var desiredRotation = MathExtensions.RotationToTarget(target, this);
                if (Math.Abs(desiredRotation - Rotation) > _rotationThreshold)
                {
                    float rotationPortion =
                        (float) ((gameTime.ElapsedGameTime.TotalMilliseconds / RotationSpeed) * (2 * Math.PI));
                    _logger.Info("Current Rotation: " + Rotation);
                    _logger.Info("Final Rotation: " + desiredRotation);
                    //turn clock or anticlockwise
                    var angleToRotate = MathExtensions.Modulo2PiAlsoNegative(desiredRotation - Rotation);
                    _logger.Info("AngleToRotate: " + angleToRotate);
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
        BodyCollision
    }
}