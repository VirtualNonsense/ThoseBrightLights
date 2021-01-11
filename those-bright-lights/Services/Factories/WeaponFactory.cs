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
        public SingleShotWeapon GetLaserGun(Actor owner, 
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
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\lasergun.json", 0);
            var m = new SingleShotWeapon(
                _animationHandlerFactory.GetAnimationHandler(tileSet,new AnimationSettings(1,isPlaying:false)),
                owner,
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
            m.RelativePosition = new Vector2(30, -50);
            m.BulletSpawnPoint = new Vector2(0, 0);
            m.Scale = 2;
            m.Layer = owner.Layer;
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
    }
}