using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SE_Praktikum.Services;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Components.Controls;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Core;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Spaceship : Actor
    {
        private List<Weapon> _weapons;
        private int _currentWeapon;
        protected float Speed;
        private Logger _logger;
        protected float Health;
        protected KeyboardState CurrentKey;
        protected KeyboardState PreviousKey;


        #region Events
        public event EventHandler OnShoot;
        public event EventHandler OnDie;
        public event EventHandler OnPickUpWeapon;
        public event EventHandler OnTakeDamage;
        
        
        #endregion


        public Spaceship(AnimationHandler animationHandler, float speed = 3, float health = 100) : base(
            animationHandler)
        {
            Speed = speed;
            Health = health;
        }

        
        public override void BaseCollide(Actor actor)
        {
            throw new System.NotImplementedException();
            _logger.Info(Health);
        }

       

        protected virtual void InvokeOnTakeDamage(float damage)
        {
            OnTakeDamage?.Invoke(this,EventArgs.Empty);
        }

        protected virtual void InvokeOnDie()
        {
            OnDie?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void InvokeOnShoot()
        {
            LevelEvent.ShootBullet e = new LevelEvent.ShootBullet();
            e.Bullet = _weapons[_currentWeapon].GetBullet();
            
            OnShoot?.Invoke(this,e);
            //TODO: wie löse ich jetzt hier das shootbullet event in level aus?
        }

        protected virtual void OnOnPickUpWeapon(Weapon weapon)
        {
            _weapons.Add(weapon);
            _currentWeapon = _weapons.Count - 1;
            OnPickUpWeapon?.Invoke(this, EventArgs.Empty);
        }
    }
}