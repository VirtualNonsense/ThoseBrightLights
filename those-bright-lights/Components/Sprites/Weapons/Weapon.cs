using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Bullets;
using SE_Praktikum.Services;
using SE_Praktikum.Extensions;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public abstract class Weapon
    {
        protected readonly SoundEffect _shotSoundEffect;

        protected readonly CooldownAbility _shotAbility;
        private float _rotation;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Base class for all weapons.
        /// It implements the base mechanism for bullet creation, bullet firing and shot cooldown
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="shotSoundEffect"></param>
        /// <param name="nameTag">Name of the gun</param>
        /// <param name="shotCoolDown">in milliseconds</param>
        public Weapon(Actor Parent, SoundEffect shotSoundEffect, string nameTag, int shotCoolDown = 10)
        {
            _shotAbility = new CooldownAbility(shotCoolDown, FireAbility);
            _shotSoundEffect = shotSoundEffect;
            NameTag = nameTag;
        }

        // #############################################################################################################
        // Events
        // #############################################################################################################
        public event EventHandler<EmitBulletEventArgs> OnEmitBullet;
        
        /// <summary>
        /// Informs the subscriber about the current state of the shotCooldown.
        /// the value will be between 0 and 1 and can be read as percent/100
        /// </summary>
        public event EventHandler<CooldownAbility.CooldownProgressArgs> OnShotCooldownProgressUpdate
        {
            add => _shotAbility.OnCoolDownUpdate += value;
            remove => _shotAbility.OnCoolDownUpdate -= value;
        }
        
        /// <summary>
        /// will be triggered when the weapon is able to fire again
        /// </summary>
        public event EventHandler OnShotAvailable
        {
            add => _shotAbility.OnCoolAbilityAvailable += value;
            remove => _shotAbility.OnCoolAbilityAvailable -= value;
        }
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public bool CanShoot => _shotAbility.AbilityAvailable;

        /// <summary>
        /// bullet rotation
        /// </summary>
        public float Rotation
        {
            get => _rotation;
            set => _rotation = MathExtensions.Modulo2PiPositive(value);
        }
        
        /// <summary>
        /// bullet spawn point
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }
        
        /// <summary>
        /// Velocity of the moving system e.g. the spaceship
        /// </summary>
        public Vector2 Velocity
        {
            get;
            set;
        }

        public string NameTag
        {
            get;
            protected set;
        }
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public void Update(GameTime gameTime)
        {
            _shotAbility.Update(gameTime);
        }

        public virtual void Fire()
        {
            _shotAbility.Fire();
        }
        
        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################
        /// <summary>
        /// use this to create the desired bullet
        /// it will be called in the baseClasses FireAbility method which is triggered via the shotCooldownAbility
        /// </summary>
        /// <returns></returns>
        protected abstract Bullet GetBullet();

        /// <summary>
        /// Method used to emit the actual bullet
        /// override this to customize spray pattern, fire burst etc.
        /// </summary>
        protected virtual void FireAbility()
        {
            var e = new EmitBulletEventArgs() { Bullet = GetBullet()};
            InvokeOnEmitBullet(e);
            _shotSoundEffect?.Play();
        }

        // #############################################################################################################
        // Invoker
        // #############################################################################################################
        protected virtual void InvokeOnEmitBullet(EmitBulletEventArgs e)
        {
            OnEmitBullet?.Invoke(this, e);
        }

        // #############################################################################################################
        // Subclasses
        // #############################################################################################################
        public class EmitBulletEventArgs : EventArgs
        {
            public Bullet Bullet { get; set; }
        }
    }
}