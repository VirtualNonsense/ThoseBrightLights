using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SE_Praktikum.Services;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Components.Controls;
using System;

namespace SE_Praktikum.Components.Sprites
{
    public class Spaceship : Actor
    {
        private float _health;
        private float _speed;
        private List<Weapon> _weapons;
        private int _currentWeapon;


        #region Events
        public event EventHandler OnShoot;
        
        #endregion
        
        
        public Spaceship(AnimationHandler animationHandler, float speed = 3, float health = 100) : base(animationHandler)
        {
            _speed = speed;
            _health = health;
        }

        public override void BaseCollide(Actor actor)
        {
            throw new System.NotImplementedException();
        }

        public void Shoot()
        {
            
        }

        public void PickUpWeapon(Weapon weapon)
        {
            _weapons.Add(weapon);
        }

        public void TakeDamage(float damage)
        {
            
        }


    }

}