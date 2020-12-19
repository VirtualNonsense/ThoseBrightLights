using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Enemy : Spaceship
    {
        private Logger _logger;
        private bool _shot = false;
        public Polygon ViewBox;
        private float _shootIntervall;
        private float _timeSinceLastShot = 0;
        private bool _canShoot;
        private InterAction _i;
        

        public Enemy(AnimationHandler animationHandler, float speed = 3, float health = 50, SoundEffect impactSound = null) : base(animationHandler, speed, health, impactSound)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _shootIntervall = 2000;
            _i = InterAction.None;
        }
        
        
        public override void Update(GameTime gameTime)
        {
            _timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timeSinceLastShot >= _shootIntervall)
            {
                _canShoot = true;
                _timeSinceLastShot = 0;
            }
            Vector2 velocity = Vector2.Zero;
            
            base.Update(gameTime);
            ViewBox.Position = Position;
            ViewBox.Rotation = Rotation;
            ViewBox.Layer = Layer;
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
                    foreach (var polygon in p.HitBox)
                    {
                        if (ViewBox.Overlap(polygon))
                        {
                            _i = InterAction.InView;
                            return true;
                        }
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
            switch (_i)
            {
                case InterAction.None:
                    break;
                case InterAction.InView:
                    switch (other)
                    {
                        case Player p:
                            Vector2 vector = p.Position - Position;
                            float rotation = (float)Math.Asin(vector.Y / vector.Length());
                            // _logger.Trace(rotation);
                            var b =  Weapons[CurrentWeapon].GetBullet(Velocity, Position, rotation, this);
                            InvokeOnShoot(b);
                            break;
                    }
                    break;
                case InterAction.BodyCollision:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        // protected override void InvokeOnShootPlayer(Vector2 velocity, Actor player)
        // {
        //     if (!_canShoot)
        //         return;
        //     _canShoot = false;
        //     base.InvokeOnShootPlayer(velocity, player);
        // }
    }

    public enum InterAction
    {
        None,
        InView,
        BodyCollision
    }
}