using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Weapons;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class WeaponFactory
    {
        private readonly ContentManager _contentManager;
        private readonly AnimationHandlerFactory _animationHandlerFactory;
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
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <returns></returns>
        public SingleShotWeapon GetMissileLauncher(Actor owner,
                                                   int clipSize = 4,
                                                   int clips = 10,
                                                   int shotCooldown = 100,
                                                   int reloadTime = 1000,
                                                   float damage = 20,
                                                   string nameTag = "Missile Launcher",
                                                   float health = 1,
                                                   float maxHealth = 1)
        {
            // TODO: create and load missing sound effects
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(null,null),
                owner,
                new Vector2(0,10),
                0,
                new Vector2(0,0),
                null, 
                null,
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
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
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <returns></returns>
        public SingleShotWeapon GetUpperLaserGun(Actor owner, 
                                            int clipSize = 20,
                                            int clips = 10,
                                            int shotCooldown = 100,
                                            int reloadTime = 1000,
                                            float damage = 5,
                                            string nameTag = "Laser gun",
                                            float health = 1,
                                            float maxHealth = 1)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\lasergunFire_30_13_7.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(7,isPlaying:false,duration:50f)),
                owner,
                new Vector2(28, -38),
                0,
                new Vector2(13,0),
                null, 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetLaser(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.5f;
            return m;
        }
        public SingleShotWeapon GetLowerLaserGun(Actor owner, 
                                            int clipSize = 20,
                                            int clips = 10,
                                            int shotCooldown = 100,
                                            int reloadTime = 1000,
                                            float damage = 5,
                                            string nameTag = "Laser gun",
                                            float health = 1,
                                            float maxHealth = 1)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\lasergunFire_30_13_7.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(7,isPlaying:false,duration:50f)),
                owner,
                new Vector2(28, 36),
                0,
                new Vector2(13,0),
                null, 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetLaser(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.5f;
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
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <returns></returns>
        public SingleShotWeapon GetEnemyLaserGun(Actor owner,
                                                 int clipSize = 20,
                                                 int clips = 3,
                                                 int shotCooldown = 20,
                                                 int reloadTime = 100,
                                                 float damage = 10,
                                                 string nameTag = "Enemy laser gun",
                                                 float health = 1,
                                                 float maxHealth = 1)
        {
            // TODO: create and load missing sound effects
            
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(null,null),
                owner,
                new Vector2(0,0),
                0,
                new Vector2(0,0),
                null, 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetEnemyLaser(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            return m;
        }
        
        /// <summary>
        /// Minigun
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="clipSize"></param>
        /// <param name="clips"></param>
        /// <param name="shotCooldown"></param>
        /// <param name="reloadTime"></param>
        /// <param name="damage"></param>
        /// <param name="nameTag"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <returns></returns>
        public SingleShotWeapon GetMinigun(Actor owner, 
            int clipSize = 200,
            int clips = 10,
            int shotCooldown = 100,
            int reloadTime = 1000,
            float damage = 5,
            string nameTag = "Minigun",
            float health = 1,
            float maxHealth = 1)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\minigunFire_50_20_4.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(4,isPlaying:false,duration:50f)),
                owner,
                new Vector2(55, 25),
                0,
                new Vector2(30,8),
                null, 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetProjectile(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.8f;
            return m;
        } 
        
        /// <summary>
        /// Shotgun
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="clipSize"></param>
        /// <param name="clips"></param>
        /// <param name="shotCooldown"></param>
        /// <param name="reloadTime"></param>
        /// <param name="damage"></param>
        /// <param name="nameTag"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <returns></returns>
        public SingleShotWeapon GetShotgun(Actor owner, 
            int clipSize = 5,
            int clips = 10,
            int shotCooldown = 500,
            int reloadTime = 2000,
            float damage = 5,
            string nameTag = "Minigun",
            float health = 1,
            float maxHealth = 1)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\shotgunFire_28_13_9.json", 0);
            var m = new MultiShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new AnimationSettings(updateList:new List<(int, float)>
                    {
                        (0,50f),
                        (1,50f),
                        (2,50f),
                        (3,50f),
                        (4,100f),
                        (5,100f),
                        (6,200f),
                        (7,200f),
                        (8,300f)
                        
                    },isPlaying:false)),
                owner,
                new Vector2(45, 20),
                0,
                new Vector2(20,-2),
                null, 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetPallet(owner, damage),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.8f;
            return m;
        } 
    }
}