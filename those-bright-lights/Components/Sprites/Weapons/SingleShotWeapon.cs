using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Bullets;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class SingleShotWeapon : ClipWeapon
    {
        private readonly Func<Bullet> _constructPreconfiguredBullet;
        
        /// <summary>
        /// A precise gun that fires one shot at a time.
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="shotSoundEffect"></param>
        /// <param name="clipEmptySound"></param>
        /// <param name="weaponEmptySound"></param>
        /// <param name="reloadSound"></param>
        /// <param name="nameTag"></param>
        /// <param name="clipSize"></param>
        /// <param name="clips"></param>
        /// <param name="constructPreconfiguredBullet"></param>
        /// <param name="shotCoolDown"></param>
        /// <param name="reloadTime"></param>
        public SingleShotWeapon(Actor Parent,
                                SoundEffect shotSoundEffect, 
                                SoundEffect clipEmptySound,
                                SoundEffect weaponEmptySound,
                                SoundEffect reloadSound,
                                string nameTag,
                                int clipSize,
                                int clips,
                                Func<Bullet> constructPreconfiguredBullet,
                                int shotCoolDown = 10,
                                int reloadTime = 1000)
            : base(Parent,
                shotSoundEffect,
                clipEmptySound,
                weaponEmptySound,
                reloadSound,
                nameTag,
                clipSize,
                clips,
                shotCoolDown,
                reloadTime)
        {
            _constructPreconfiguredBullet = constructPreconfiguredBullet;
        }

        protected override Bullet GetBullet()
        {
            var b = _constructPreconfiguredBullet();
            b.Parent = _parent;
            b.Position = _parent.Position;
            b.Rotation = _parent.Rotation;
            b.Velocity = _parent.Velocity;
            b.Layer = _parent.Layer;
            return b;
        }
    }
}