using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

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
        protected float FinalRotation;
        /// <summary>
        /// Defines the angle in which the enemy doesn't rotate anymore -> it's close enough
        /// </summary>
        private float _rotationThreshold = MathExtensions.DegToRad(15);
        

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
            float rotation = Rotation;
            if (!RotateAndShoot)
            {
                var vector = Target.Position - Position;
                if (Math.Abs(vector.X) > Math.Abs(vector.Y))
                    rotation -= (float) Math.Asin(vector.Y / vector.Length());
                if (Math.Abs(vector.X) < Math.Abs(vector.Y))
                    rotation += (float) Math.Acos(vector.X / vector.Length());
            }
            // _logger.Trace(rotation);
            var b =  Weapons[CurrentWeapon].GetBullet(Velocity, Position, rotation, this);
            InvokeOnShoot(b);
        }
        
        
        public override void Update(GameTime gameTime)
        {
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
            if (Math.Abs(FinalRotation - Rotation) > _rotationThreshold)
            {
                float rotation = (float)((gameTime.ElapsedGameTime.TotalMilliseconds / RotationSpeed) * (2 * Math.PI));
                _logger.Info("Current Rotation: " + Rotation);
                _logger.Info("Final Rotation: " + FinalRotation);
                //turn clock or anticlockwise
                var angleToRotate = MathExtensions.Modulo2PiPositive(FinalRotation - Rotation);
                _logger.Info("AngleToRotate: " + angleToRotate);
                if (angleToRotate > Math.PI)
                    Rotation -= rotation;
                else
                    Rotation += rotation;
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