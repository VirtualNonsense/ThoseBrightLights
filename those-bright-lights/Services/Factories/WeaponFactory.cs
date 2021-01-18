using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Weapons;
using SE_Praktikum.Models;
using System;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;

namespace SE_Praktikum.Services.Factories
{
    public class WeaponFactory
    {
        private readonly ContentManager _contentManager;
        private readonly AnimationHandlerFactory _animationHandlerFactory;
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
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <returns></returns>
        public SingleShotWeapon GetMissileLauncher(Actor owner,
                                                   int clipSize = 4,
                                                   int clips = 10,
                                                   int shotCooldown = 100,
                                                   int reloadTime = 2500,
                                                   float damage = 100,
                                                   string nameTag = "Missile Launcher",
                                                   float health = 10,
                                                   float? maxHealth = null)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missilelauncher_4_12.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(1,2000f)),
                owner,
                new Vector2(0,10),
                0,
                new Vector2(0,0),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/missile_w_ignition"), 
                null,
                nameTag,
                health,
                maxHealth,
                null,
                null,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/weapon_specific_stuff/missile_launcher_reload"),
                clipSize,
                clips,
                () => _bulletFactory.GetMissile(owner, damage, 1),
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
                                            float health = 10,
                                            float? maxHealth = null)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\lasergunFire_30_13_7.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(7,isPlaying:false,duration:50f)),
                owner,
                new Vector2(28, -38),
                0,
                new Vector2(16,0),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/laser_shot"), 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetLaser(owner, damage, 1),
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
                                            float? maxHealth = null)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\lasergunFire_30_13_7.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(7,isPlaying:false,duration:50f)),
                owner,
                new Vector2(28, 36),
                0,
                new Vector2(16,0),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/laser_shot"), 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetLaser(owner, damage, 1),
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
                                                 int shotCooldown = 1000,
                                                 int reloadTime = 100,
                                                 float damage = 5,
                                                 string nameTag = "Enemy laser gun",
                                                 float health = 1,
                                                 float? maxHealth = null)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\enemylasergun_18_10_3.json", 0);
            
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(3,isPlaying:false,duration:600f)),
                owner,
                new Vector2(30,15),
                0,
                new Vector2(0,0),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/laser_shot"), 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                null,
                clipSize,
                clips,
                () => _bulletFactory.GetEnemyLaser(owner, damage, 1),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.4f;
            return m;
        }
        
        /// <summary>
        /// Weapon for the Turret
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
        public SingleShotWeapon GetTurretLaserGun(Actor owner,
                                                 int clipSize = 2,
                                                 int clips = 1,
                                                 int shotCooldown = 1000,
                                                 int reloadTime = 100,
                                                 float damage = 5,
                                                 string nameTag = "Enemy laser gun",
                                                 float health = 10,
                                                 float? maxHealth = null)
        {
            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\turretlaser_41_40_6.json", 0);
            
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(6,isPlaying:false,duration:600f)),
                owner,
                new Vector2(-28,-4),
                0,
                new Vector2(30,-4),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/turret_gun_shot"), 
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/turret_gun_impact"), 
                nameTag,
                health,
                maxHealth,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/weapon_specific_stuff/turret_gun_clip_empty"),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/weapon_specific_stuff/turret_gun_empty"),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/weapon_specific_stuff/turret_reloading"),
                clipSize,
                clips,
                () => _bulletFactory.GetEnemyLaser(owner, damage, 1),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.8f;
            return m;
        }
        
        /// <summary>
        /// Minigun. ta ta ta ta ta ta
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
            int clipSize = 100,
            int clips = 10,
            int shotCooldown = 90,
            int reloadTime = 2500,
            float damage = 5,
            string nameTag = "Minigun",
            float health = 10,
            float? maxHealth = null)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\minigunFire_50_20_4.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(4,isPlaying:false,duration:50f)),
                owner,
                new Vector2(50, 23),
                0,
                new Vector2(30,5),
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/tav2"), 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/weapon_specific_stuff/minigun_reload"),
                clipSize,
                clips,
                () => _bulletFactory.GetProjectile(owner, damage, 1),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.5f;
            return m;
        } 
        
        /// <summary>
        /// Boss Weapon p1
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
            string nameTag = "Shotgun",
            float health = 10,
            float? maxHealth = null)
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
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/shotgun_shot_w_reload"), 
                null, 
                nameTag,
                health,
                maxHealth,
                null,
                null,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/weapon_specific_stuff/shotgun_reload"),
                clipSize,
                clips,
                () => _bulletFactory.GetPallet(owner, damage, 1),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.8f;
            return m;
        }

        public List<Weapon> GetRandomWeapon(Actor owner)
        {
            var i = _random.Next(4);
            var weaponList = new List<Weapon>();
            switch (i)
            {
                case 0:
                     weaponList.Add(GetMissileLauncher(owner));
                    break;
                    
                case 1:
                    weaponList.Add(GetShotgun(owner));
                    break;

                case 2:
                    weaponList.Add(GetMinigun(owner));
                    break;
                case 3:
                    weaponList.Add(GetLowerLaserGun(owner));
                    weaponList.Add(GetUpperLaserGun(owner));
                    break;
                default:
                    throw new NotImplementedException();

            }
            return weaponList;
        }

        #region BossWeapon

        /// <summary>
        /// Boss Weapon p2
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
        public Weapon GetLowerBossLaserGun(Actor owner, 
            int clipSize = 5,
            int clips = 10,
            int shotCooldown = 500,
            int reloadTime = 2000,
            float damage = 5,
            string nameTag = "Boss Lasergun",
            float health = 10,
            float? maxHealth = null)
        {

            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\bossweaponFire_20_16_4.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new AnimationSettings(4,200f, isPlaying: false)),
                owner,
                new Vector2(60, -10),
                0,
                new Vector2(22, 5),
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
                () => _bulletFactory.GetEnemyLaser(owner, damage, 1),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.8f;
            return m;
        } /// <summary>
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
        public Weapon GetUpperBossLaserGun(Actor owner, 
            int clipSize = 5,
            int clips = 10,
            int shotCooldown = 500,
            int reloadTime = 2000,
            float damage = 5,
            string nameTag = "Boss Lasergun",
            float health = 10,
            float? maxHealth = null)
        {

            // TODO: create and load missing sound effects
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\bossweaponFire_20_16_4.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new AnimationSettings(4,200f, isPlaying: false)),
                owner,
                new Vector2(50, 15),
                0,
                new Vector2(22, 5),
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
                () => _bulletFactory.GetEnemyLaser(owner, damage, 1),
                shotCoolDown: shotCooldown,
                reloadTime: reloadTime);
            m.Scale = 1.8f;
            return m;
        } 

        #endregion
    }
}