using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Bullets;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class WeaponFactory
    {
        private readonly ContentManager _contentManager;
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly BulletFactory _bulletFactory;

        public WeaponFactory(ContentManager contentManager,
                             AnimationHandlerFactory animationHandlerFactory,
                             ParticleFactory particleFactory,
                             TileSetFactory tileSetFactory,
                             BulletFactory bulletFactory)
        {
            _contentManager = contentManager;
            _animationHandlerFactory = animationHandlerFactory;
            _particleFactory = particleFactory;
            _tileSetFactory = tileSetFactory;
            _bulletFactory = bulletFactory;
        }

        public SingleShotWeapon GetMissileLauncher(Actor owner, int clipSize = 4, int clips = 10, int shotCooldown = 10, int reloadTime = 1000)
        {
            // TODO: create and load missing sound effects
            var m = new SingleShotWeapon(owner,
                                         null, 
                                         null, 
                                         null,
                                         null,
                                         "Missile Launcher",
                                         clipSize,
                                         clips,
                                         () => _bulletFactory.GetMissile(owner),
                                         shotCoolDown: shotCooldown,
                                         reloadTime: reloadTime);
            return m;
        }

        public SingleShotWeapon GetLasergun(Actor owner)
        {
            var laserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\laser.json",0);
            SoundEffect soundEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/Laser_Short");
            // var l = new Lasergun(_animationHandlerFactory, laserTileSet, _particleFactory, soundEffect, flightEffect, impactSound);
            return null;
        }

        public SingleShotWeapon EnemyGetLasergun(Actor owner)
        {
            var enemylaserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\enemylaser.json", 0);
            SoundEffect soundEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/Laser_Short");
            // var k = new Lasergun(_animationHandlerFactory, enemylaserTileSet, _particleFactory, soundEffect, flightEffect, impactSound);
            return null;
        }
    }
}