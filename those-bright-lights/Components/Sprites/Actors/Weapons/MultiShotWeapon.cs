using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Weapons
{
    public class MultiShotWeapon : SingleShotWeapon
    {
        private int _bulletsPerShot;
        private Random _random;
        private float _spreadAngle;
        private float _speedVariation;
        public MultiShotWeapon(AnimationHandler animationHandler,
                               Actor parent,
                               Vector2 relativePosition,
                               float relativeRotation,
                               Vector2 bulletSpawnPoint,
                               SoundEffect shotSoundEffect,
                               SoundEffect impactSound,
                               string nameTag,
                               float health,
                               float? maxHealth,
                               SoundEffect clipEmptySound,
                               SoundEffect weaponEmptySound,
                               SoundEffect reloadSound,
                               int clipSize,
                               int clips,
                               Func<Bullet> constructPreconfiguredBullet,
                               int shotCoolDown = 10,
                               int reloadTime = 1000,
                               int bulletsPerShot = 8,
                               float? spreadAngle = null)
            : base(animationHandler,
                   parent,
                   relativePosition,
                   relativeRotation,
                   bulletSpawnPoint,
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
                   constructPreconfiguredBullet,
                   shotCoolDown,
                   reloadTime)
        {
            _bulletsPerShot = bulletsPerShot;
            _random = new Random();
            _spreadAngle = spreadAngle ?? MathExtensions.DegToRad(15);
        }

        protected override void FireAbility()
        {
            for (int i = 0; i < _bulletsPerShot; i++)
            {
                var e = new EmitBulletEventArgs() { Bullet = GetBullet()};
                e.Bullet.Rotation += -_spreadAngle / 2 + (float)_random.NextDouble() * _spreadAngle;
                e.Bullet.Speed *= (float)(_random.NextDouble() * 0.2f) + 0.9f;
                InvokeOnEmitBullet(e);
            }
            ShotSoundEffect?.Play();
            AmmunitionInClip--;
        }
        
        
    }
}