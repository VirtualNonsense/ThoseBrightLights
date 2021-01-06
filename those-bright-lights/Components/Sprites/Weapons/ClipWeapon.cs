using System;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Bullets;
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


        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Base class for all weapons with a magazine
        /// </summary>
        /// <param name="Parent">Wielder of gun</param>
        /// <param name="shotSoundEffect"></param>
        /// <param name="nameTag">Name of the Weapon</param>
        /// <param name="clipSize">Amount of bullets in magazine</param>
        /// <param name="clips">Amount of magazine</param>
        /// <param name="shotCoolDown">in Milliseconds</param>
        /// <param name="reloadTime">in Milliseconds</param>
        /// <param name="clipEmptySound"></param>
        /// <param name="weaponEmptySound"></param>
        protected ClipWeapon(Actor Parent,
                             SoundEffect shotSoundEffect,
                             SoundEffect clipEmptySound,
                             SoundEffect weaponEmptySound,
                             string nameTag,
                             int clipSize,
                             int clips,
                             int shotCoolDown = 10,
                             int reloadTime = 1000)
            : base(Parent, shotSoundEffect, nameTag, shotCoolDown)
        {
            Clips = clips;
            _clipSize = clipSize;
            _reloadAbility = new CooldownAbility(reloadTime, ExecuteReload);
            _clipEmptySound = clipEmptySound;
            _weaponEmptySound = weaponEmptySound;
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
            _reloadAbility.Fire();
        }


        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################
        
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