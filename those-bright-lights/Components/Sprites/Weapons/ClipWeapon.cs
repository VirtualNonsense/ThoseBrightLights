using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public abstract class ClipWeapon : Weapon
    {
        private int _clips;
        private int _ammunitionInClip;
        private readonly int _clipSize;
        private readonly CooldownAbility _reloadAbility;
        private readonly SoundEffect _clipEmptySound;
        private readonly SoundEffect _weaponEmptySound;
        private readonly SoundEffect _reloadSoundEffect;


        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Base class for all weapons with a magazine
        /// </summary>
        /// <param name="Parent">Wielder of gun</param>
        /// <param name="shotSoundEffect"></param>
        /// <param name="reloadSoundEffect"></param>
        /// <param name="nameTag">Name of the Weapon</param>
        /// <param name="clipSize">Amount of bullets in magazine</param>
        /// <param name="clips">Amount of magazine</param>
        /// <param name="shotCoolDown">in Milliseconds</param>
        /// <param name="reloadTime">in Milliseconds</param>
        /// <param name="clipEmptySound"></param>
        /// <param name="weaponEmptySound"></param>
        /// <param name="initialBulletsInClip"></param>
        protected ClipWeapon(Actor Parent,
                             SoundEffect shotSoundEffect,
                             SoundEffect clipEmptySound,
                             SoundEffect weaponEmptySound,
                             SoundEffect reloadSoundEffect,
                             string nameTag,
                             int clipSize,
                             int clips,
                             int shotCoolDown = 10,
                             int reloadTime = 1000,
                             int? initialBulletsInClip = null)
            : base(Parent, shotSoundEffect, nameTag, shotCoolDown)
        {
            Clips = clips;
            _clipSize = clipSize;
            AmmunitionInClip = initialBulletsInClip ?? clipSize;
            _reloadAbility = new CooldownAbility(reloadTime, ExecuteReload);
            _clipEmptySound = clipEmptySound;
            _weaponEmptySound = weaponEmptySound;
            _reloadSoundEffect = reloadSoundEffect;
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
        public event EventHandler<CooldownAbility.CooldownProgressArgs> OnReloadProgressUpdate
        {
            add => _reloadAbility.OnCoolDownUpdate += value;
            remove => _reloadAbility.OnCoolDownUpdate -= value;
        }

        /// <summary>
        /// Fires when the reload is finished
        /// </summary>
        public event EventHandler OnReloadFinished
        {
            add => _reloadAbility.OnCoolAbilityAvailable += value;
            remove => _reloadAbility.OnCoolAbilityAvailable -= value;
        }
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public bool IsAmmunitionInClip => AmmunitionInClip > 0;
        public bool AreClipsLeft => Clips > 0;
        public int TotalAmmunition => _clips * _clipSize + AmmunitionInClip;

        public int AmmunitionInClip
        {
            get => _ammunitionInClip;
            set
            {
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
            _reloadAbility.Update(gameTime);
            base.Update(gameTime);
        }

        // #############################################################################################################
        // public methods
        // #############################################################################################################
        /// <summary>
        /// Fires a shot
        /// </summary>
        public override void Fire()
        {
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
        public virtual void Reload()
        {
            _reloadSoundEffect?.Play();
            _reloadAbility.Fire();
        }

        public override string ToString()
        {
            if (!_reloadAbility.AbilityAvailable)
                return $"{NameTag} reloading: {_reloadAbility.CoolDownProgress * 100}%";
            return $"{NameTag} Clip: {_ammunitionInClip}/{_clipSize}, total: {TotalAmmunition}";
        }


        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################

        protected override void FireAbility()
        {
            if (!IsAmmunitionInClip)
            {
                InvokeOnClipEmpty();
                return;
            }
            base.FireAbility();
            AmmunitionInClip--;
        }

        private void ExecuteReload()
        {
            if (!AreClipsLeft)
            {
                InvokeOnWeaponEmpty();
                return;
            }
            AmmunitionInClip = _clipSize;
            Clips--;
        }

        protected virtual void InvokeOnClipEmpty()
        {
            _clipEmptySound?.Play();
            OnClipEmpty?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void InvokeOnWeaponEmpty()
        {
            _weaponEmptySound?.Play();
            OnWeaponEmpty?.Invoke(this, EventArgs.Empty);
        }
    }
}