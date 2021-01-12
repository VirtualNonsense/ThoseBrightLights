using Microsoft.Xna.Framework.Content;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Weapons;
using System;

namespace SE_Praktikum.Services.Factories
{
    public class WeaponFactory
    {
        private readonly ContentManager _contentManager;
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly BulletFactory _bulletFactory;
        private readonly Random _random;
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
            _random = new Random();
            
        }

        /// <summary>
        /// A missile launcher. Rocket goes brrrr
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="clipSize"></param>
        /// <param name="clips"></param>
        /// <param name="shotCooldown"></param>
        /// <param name="reloadTime"></param>
        /// <param name="damage"></param>
        /// <param name="nameTag"></param>
        /// <returns></returns>
        public SingleShotWeapon GetMissileLauncher(Actor owner,
                                                   int clipSize = 4,
                                                   int clips = 10,
                                                   int shotCooldown = 100,
                                                   int reloadTime = 1000,
                                                   float damage = 20,
                                                   string nameTag = "Missile Launcher")
        {
            // TODO: create and load missing sound effects
            var m = new SingleShotWeapon(owner,
                null, 
                null, 
                null,
                null,
                nameTag,
                clipSize,
                clips,
                () => _bulletFactory.GetMissile(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            return m;
        }

        /// <summary>
        /// A vanilla laser gun
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="clipSize"></param>
        /// <param name="clips"></param>
        /// <param name="shotCooldown"></param>
        /// <param name="reloadTime"></param>
        /// <param name="damage"></param>
        /// <param name="nameTag"></param>
        /// <returns></returns>
        public SingleShotWeapon GetLaserGun(Actor owner, 
                                            int clipSize = 20,
                                            int clips = 10,
                                            int shotCooldown = 100,
                                            int reloadTime = 1000,
                                            float damage = 5,
                                            string nameTag = "Laser gun")
        {
            // TODO: create and load missing sound effects
            var m = new SingleShotWeapon(owner,
                null, 
                null, 
                null,
                null,
                nameTag,
                clipSize,
                clips,
                () => _bulletFactory.GetLaser(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            return m;
        }

        /// <summary>
        /// Returns a gun that is meant for the current std. enemy.
        /// But i'm just a dev you don't have to listen to me.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="clipSize"></param>
        /// <param name="clips"></param>
        /// <param name="shotCooldown"></param>
        /// <param name="reloadTime"></param>
        /// <param name="damage"></param>
        /// <param name="nameTag"></param>
        /// <returns></returns>
        public SingleShotWeapon GetEnemyLaserGun(Actor owner,
                                                 int clipSize = 20,
                                                 int clips = 3,
                                                 int shotCooldown = 20,
                                                 int reloadTime = 100,
                                                 float damage = 10,
                                                 string nameTag = "Enemy laser gun")
        {
            // TODO: create and load missing sound effects
            var m = new SingleShotWeapon(owner,
                null, 
                null, 
                null,
                null,
                nameTag,
                clipSize,
                clips,
                () => _bulletFactory.GetEnemyLaser(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            return m;
        }

        //public Weapon GetRandomWeapon(Actor owner)
        //{
        //    var i = _random.Next(3);
        //    switch(i)
        //    {
        //        case 0:
        //            return GetMinigun(owner);
        //        case 1:
        //            return GetMissileLauncher(owner);
                    
        //        case 2:
        //            return GetShotgun(owner);

        //        default:
        //            throw new NotImplementedException();
        //    }
        //}
    }
}