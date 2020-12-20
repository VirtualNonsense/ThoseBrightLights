using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SE_Praktikum.Services;
using SE_Praktikum.Components.Sprites.Weapons;
using System;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Models;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Extensions;

namespace SE_Praktikum.Components.Sprites
{
    public abstract class Spaceship : Actor
    {
        protected List<Weapon> Weapons;
        protected int CurrentWeapon;
        protected float Speed;
        private Logger _logger;
        protected KeyboardState CurrentKey;
        protected KeyboardState PreviousKey;
        protected bool FlippedHorizontal => _animationHandler.SpriteEffects == SpriteEffects.FlipVertically;
        


        #region Events
        public event EventHandler OnShoot;
        public event EventHandler OnDie;
        public event EventHandler OnPickUpWeapon;
        public event EventHandler OnTakeDamage;

        #endregion

        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                if (Rotation < 3 * Math.PI/2 && Rotation > Math.PI/2)
                {
                    if (FlippedHorizontal) return;
                    _animationHandler.SpriteEffects = SpriteEffects.FlipVertically;
                }
                else if (FlippedHorizontal)
                {
                    _animationHandler.SpriteEffects = SpriteEffects.None;
                }

            }
        }


        public Spaceship(AnimationHandler animationHandler, float speed = 3, float health = 100, SoundEffect impactSound = null) : base(
            animationHandler, impactSound)
        {
            Speed = speed;
            Health = health;
            Weapons = new List<Weapon>();
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var weapon in Weapons)
            {
                weapon.Update(gameTime);
            }
            base.Update(gameTime);
        }


        protected virtual void InvokeOnTakeDamage(float damage)
        {
            OnTakeDamage?.Invoke(this,EventArgs.Empty);
        }

        protected virtual void InvokeOnDie()
        {
            OnDie?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void InvokeOnShoot(Bullet b)
        {
            b.Layer = this.Layer;
            var e = new LevelEvent.ShootBullet {Bullet = b};
            if (e.Bullet is null)
                return;
            OnShoot?.Invoke(this,e);
        }

        public virtual void AddWeapon(Weapon weapon)
        {
            Weapons.Add(weapon);
            CurrentWeapon = Weapons.Count - 1;
            OnPickUpWeapon?.Invoke(this, EventArgs.Empty);
        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Bullet b:
                    // bullet shouldn't damage it's parent
                    if (this == b.Parent) return;
                    Health -= b.Damage;
                    _impactSound?.Play();
                    break;
            }
        }
    }
}