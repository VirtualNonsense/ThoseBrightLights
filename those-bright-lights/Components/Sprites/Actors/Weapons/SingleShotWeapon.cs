﻿using System;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Weapons
{
    public class SingleShotWeapon : ClipWeapon
    {
        private readonly Func<Bullet> _constructPreconfiguredBullet;

        /// <summary>
        /// A precise gun that fires one shot at a time.
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="parent">the owner of the gun. it will be assigned to each bullet as well</param>
        /// <param name="shotSoundEffect"></param>
        /// <param name="impactSound"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="clipEmptySound"></param>
        /// <param name="weaponEmptySound"></param>
        /// <param name="reloadSound"></param>
        /// <param name="nameTag">Guns can have names i guess. Don't use this for weapon differentiation</param>
        /// <param name="clipSize">amount of bullets in one magazine</param>
        /// <param name="clips">amount of full magazine</param>
        /// <param name="constructPreconfiguredBullet">method that will create a bullet</param>
        /// <param name="shotCoolDown">in milliseconds</param>
        /// <param name="reloadTime">in milliseconds</param>
        public SingleShotWeapon(AnimationHandler animationHandler,
                                Actor parent,
                                SoundEffect shotSoundEffect, 
                                SoundEffect impactSound,
                                string nameTag,
                                float health,
                                float maxHealth,
                                SoundEffect clipEmptySound,
                                SoundEffect weaponEmptySound,
                                SoundEffect reloadSound,
                                int clipSize,
                                int clips,
                                Func<Bullet> constructPreconfiguredBullet,
                                int shotCoolDown = 10,
                                int reloadTime = 1000) 
                            : base(animationHandler,
                                parent,
                                shotSoundEffect,
                                impactSound,
                                nameTag,
                                health,
                                maxHealth,
                                clipEmptySound,
                                weaponEmptySound,
                                reloadSound,
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
            b.Parent = Parent;
            //b.Position = Parent.Position;
            b.Position = Position + BulletSpawnPoint.Rotate(Rotation);
            b.Rotation = Parent.Rotation + RelativeRotation;
            b.Velocity = Parent.Velocity;
            b.Layer = Parent.Layer;
            return b;
        }

        protected override void ExecuteInteraction(Actor other)
        {
            throw new NotImplementedException();
        }
        
    }
}