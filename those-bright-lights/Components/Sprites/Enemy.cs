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
        private bool _shot = false;
        public Polygon ViewBox;
        private InterAction _i;
        private Actor _target;
        protected CooldownAbility ForgetTarget;
        protected CooldownAbility Shoot;
        

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
            _i = InterAction.None;
        }

        private void _shootTarget()
        {
            float rotation = 0;
            var vector = _target.Position - Position;
            if(Math.Abs(vector.X) > Math.Abs(vector.Y)) 
                rotation = (float)Math.Asin(vector.Y / vector.Length());
            if(Math.Abs(vector.X) < Math.Abs(vector.Y)) 
                rotation = (float)Math.Acos(vector.X / vector.Length());
            // _logger.Trace(rotation);
            var b =  Weapons[CurrentWeapon].GetBullet(Velocity, Position, rotation, this);
            InvokeOnShoot(b);
        }
        
        
        public override void Update(GameTime gameTime)
        {
            Shoot.Update(gameTime);
            if (_i == InterAction.InView && _target != null)
                Shoot.Fire();
            
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
                        _i = InterAction.InView;
                        return true;
                    }

                    break;
            }
            var t = base.InteractAble(other);
            if (t)
                _i = InterAction.BodyCollision;
            return t;
        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Player p:
                    _target = p;
                    if(_i != InterAction.BodyCollision) return;
                    Health -= p.Damage;
                    break;
                default:
                    if (other.Parent == this) return;
                    Health -= other.Damage;
                    _logger.Debug($"health {Health}");
                    _impactSound?.Play();
                    break;
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