using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Components.Sprites.Actors.Weapons
{
    public abstract class Weapon : SpaceshipAddOn
    {
        protected readonly SoundEffect ShotSoundEffect;

        //private readonly CooldownAbility _shotAbility;
        private float _rotation;
        private Logger _logger;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Base class for all weapons.
        /// It implements the base mechanism for bullet creation, bullet firing and shot cooldown
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="parent"></param>
        /// <param name="relativePosition"></param>
        /// <param name="relativeRotation"></param> Rotation in body space of parent
        /// <param name="bulletSpawnPoint"></param> in relative coordinates to parent
        /// <param name="shotSoundEffect"></param>
        /// <param name="impactSound"></param>
        /// <param name="nameTag">Name of the gun</param>
        /// <param name="maxHealth"></param>
        /// <param name="shotCoolDown">in milliseconds</param>
        /// <param name="health"></param>
        public Weapon(
                AnimationHandler animationHandler, 
                Actor parent, 
                Vector2 relativePosition,
                float relativeRotation,
                Vector2 bulletSpawnPoint,
                SoundEffect shotSoundEffect, 
                SoundEffect impactSound,
                string nameTag, 
                float health,
                float maxHealth)
                : base(animationHandler, parent,relativePosition, relativeRotation, impactSound,health,maxHealth)
        {
            _logger = LogManager.GetCurrentClassLogger();
            //_shotAbility = new CooldownAbility(shotCoolDown, FireAbility);
            Parent = parent;
            ShotSoundEffect = shotSoundEffect;
            NameTag = nameTag;
            BulletSpawnPoint = bulletSpawnPoint;
        }

        // #############################################################################################################
        // Events
        // #############################################################################################################
        public event EventHandler<EmitBulletEventArgs> OnEmitBullet;
        
        /// <summary>
        /// Informs the subscriber about the current state of the shotCooldown.
        /// the value will be between 0 and 1 and can be read as percent/100
        /// </summary>
        public event EventHandler<AnimationHandler.AnimationProgressArgs> OnShotCooldownProgressUpdate
        {
            add => _animationHandler.OnAnimationProgressUpdate += value;
            remove => _animationHandler.OnAnimationProgressUpdate -= value;
        }
        
        /// <summary>
        /// will be triggered when the weapon is able to fire again
        /// </summary>
        public event EventHandler OnShotAvailable
        {
            add => _animationHandler.OnAnimationComplete += value;
            remove => _animationHandler.OnAnimationComplete -= value;
        }
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public bool IsRemoveAble { get; set; }

        public Type BulletType { get; private set; }
        
        /// <summary>
        /// the owner of the gun.
        /// </summary>
        //public Actor Parent { get; set; }
        public bool CanShoot => !_animationHandler.IsPlaying;


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
        
        /// <summary>
        /// Position is in space of Weapon, so relative position
        /// </summary>
        public Vector2 BulletSpawnPoint { get; set; }
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################
      

        public virtual void Fire()
        {
            if (_animationHandler.IsPlaying) return;
            _animationHandler.IsPlaying = true;
            FireAbility();
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
            ShotSoundEffect?.Play();
        }
        


        // #############################################################################################################
        // Invoker
        // #############################################################################################################
        protected virtual void InvokeOnEmitBullet(EmitBulletEventArgs e)
        {
            if(OnEmitBullet == null) _logger.Warn($"{NameTag} of {Parent} tries to fire but has no subscriber to OnEmitBullet");
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