using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using ThoseBrightLights.Services;
using ThoseBrightLights.Services.Abilities;

namespace ThoseBrightLights.Components.Sprites.Actors.Weapons
{
    public abstract class ClipWeapon : Weapon
    {
        private int _clips;
        private int _ammunitionInClip;
        private bool _ammoUsage;
        private readonly int _clipSize;
        private readonly CastTimeAbility _reloadDownTime;
        private readonly SoundEffect _clipEmptySound;
        private readonly SoundEffect _weaponEmptySound;
        private readonly SoundEffect _reloadSoundEffect;
        private readonly Logger _logger;


        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Base class for all weapons with a magazine
        /// </summary>
        /// <param name="bulletSpawnPoint"></param>
        /// <param name="shotSoundEffect"></param>
        /// <param name="reloadSoundEffect"></param>
        /// <param name="impactSound"></param>
        /// <param name="nameTag">Name of the Weapon</param>
        /// <param name="clipSize">Amount of bullets in magazine</param>
        /// <param name="clips">Amount of magazine</param>
        /// <param name="ammoUsage"></param>
        /// <param name="reloadTime">in Milliseconds</param>
        /// <param name="maxHealth"></param>
        /// <param name="clipEmptySound"></param>
        /// <param name="weaponEmptySound"></param>
        /// <param name="initialBulletsInClip"></param>
        /// <param name="animationHandler"></param>
        /// <param name="parent"></param>
        /// <param name="relativePosition"></param>
        /// <param name="relativeRotation"></param>
        /// <param name="health"></param>
        protected ClipWeapon(AnimationHandler animationHandler, 
                             Actor parent,   
                             Vector2 relativePosition,
                             float relativeRotation,
                             Vector2 bulletSpawnPoint,
                             SoundEffect shotSoundEffect, 
                             SoundEffect impactSound,
                             string nameTag, 
                             float health,
                             float? maxHealth ,
                             SoundEffect clipEmptySound,
                             SoundEffect weaponEmptySound,
                             SoundEffect reloadSoundEffect,
                             int clipSize,
                             int clips,
                             bool ammoUsage,
                             int reloadTime = 1000,
                             int? initialBulletsInClip = null)
            : base(animationHandler,parent, relativePosition, relativeRotation,bulletSpawnPoint,shotSoundEffect, impactSound,nameTag, health, maxHealth)
        {
            _logger = LogManager.GetCurrentClassLogger();
            Clips = clips;
            _clipSize = clipSize;
            AmmunitionInClip = initialBulletsInClip ?? clipSize;
            _reloadDownTime = new CastTimeAbility(reloadTime, ExecuteReload);
            _clipEmptySound = clipEmptySound;
            _weaponEmptySound = weaponEmptySound;
            _reloadSoundEffect = reloadSoundEffect;
            _ammoUsage = ammoUsage;
        }
        // #############################################################################################################
        // Events
        // #############################################################################################################
        /// <summary>
        /// Fires if clip is empty
        /// </summary>
        public event EventHandler OnClipEmpty;

        /// <summary>
        /// Fires if weapon is empty
        /// </summary>
        public event EventHandler OnWeaponEmpty;
        
        /// <summary>
        /// Updates subscriber about the reload progress
        /// </summary>
        public event EventHandler<TimeControlledAbility.CooldownProgressArgs> OnReloadProgressUpdate
        {
            add => _reloadDownTime.OnCoolDownUpdate += value;
            remove => _reloadDownTime.OnCoolDownUpdate -= value;
        }

        /// <summary>
        /// Fires when the reload is finished
        /// </summary>
        public event EventHandler OnReloadFinished
        {
            add => _reloadDownTime.OnCoolAbilityAvailable += value;
            remove => _reloadDownTime.OnCoolAbilityAvailable -= value;
        }

        /// <summary>
        /// Fires when someone sets the ammoUsageProperty
        /// </summary>
        public event EventHandler OnAmmoUsageChanged;
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public bool IsAmmunitionInClip => AmmunitionInClip > 0;
        public bool AreClipsLeft => Clips > 0;
        public int TotalAmmunition => _clips * _clipSize + AmmunitionInClip;

        public bool WeaponEmpty => TotalAmmunition <= 0;

        public int AmmunitionInClip
        {
            get => _ammunitionInClip;
            set
            {
                // when ammoUsage is disabled the ammunition is not decrementable
                if (!_ammoUsage && value < _ammunitionInClip) return;
                value = value < 0 ? 0 : value;
                value = value > _clipSize ? _clipSize : value;
                _ammunitionInClip = value;
            }
        }

        public int Clips
        {
            get => _clips;
            set => _clips = value < 0? 0 : value;
        }

        public override void Update(GameTime gameTime)
        {
            _reloadDownTime.Update(gameTime);
            base.Update(gameTime);
        }

        public bool AmmoUsage
        {
            get => _ammoUsage;
            set
            {
                _ammoUsage = value;
                OnOnAmmoUsageChanged();
            }
        }

        // #############################################################################################################
        // public methods
        // #############################################################################################################
        /// <summary>
        /// Fires a shot
        /// </summary>
        public override void Fire()
        {
            if (WeaponEmpty)
            {
                InvokeOnWeaponEmpty();
                return;
            }
            if (!IsAmmunitionInClip)
            {
                InvokeOnClipEmpty();
                return;
            }
            base.Fire();
        }
        /// <summary>
        /// Use this to Reload the gun manually
        /// </summary>
        public void Reload()
        {
            if (_reloadDownTime.CastingInProgress) return;
            _reloadSoundEffect?.Play();
            _reloadDownTime.Fire();
        }

        public override string ToString()
        {
            if (!_reloadDownTime.TargetTimeReached)
                return $"{NameTag} reloading: {_reloadDownTime.CoolDownProgress * 100}%";
            return $"{NameTag} Clip: {_ammunitionInClip}/{_clipSize}, total: {TotalAmmunition}";
        }


        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################

        protected override void FireAbility()
        {
            base.FireAbility();
            AmmunitionInClip--;
        }

        private void ExecuteReload()
        {
            _logger.Trace($"Reloading {NameTag} bullets in clips: {AmmunitionInClip} clips: {_clips}");
            AmmunitionInClip = _clipSize;
            Clips--;
        }

        private void InvokeOnClipEmpty()
        {
            _clipEmptySound?.Play();
            OnClipEmpty?.Invoke(this, EventArgs.Empty);
        }

        private void InvokeOnWeaponEmpty()
        {
            _weaponEmptySound?.Play();
            OnWeaponEmpty?.Invoke(this, EventArgs.Empty);
        }

        private void OnOnAmmoUsageChanged()
        {
            OnAmmoUsageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}