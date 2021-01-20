using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    /// <summary>
    /// constructs and configures bullets
    /// </summary>
    public class BulletFactory
    {
        private readonly ContentManager _contentManager;
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ParticleFactory _particleFactory;

        public BulletFactory(ContentManager contentManager,
                             AnimationHandlerFactory animationHandlerFactory,
                             TileSetFactory tileSetFactory,
                             ParticleFactory particleFactory)
        {
            _contentManager = contentManager;
            _animationHandlerFactory = animationHandlerFactory;
            _tileSetFactory = tileSetFactory;
            _particleFactory = particleFactory;
        }
        
        public Bullet GetMissile(Actor owner, float damage, float bulletHealth)
        {
            var missileTiles =  _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missile.json",0);
            var propulsionTiles =  _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missile_propulsion_15_15.json", 0);
            var particle = _particleFactory.BuildExplosionParticle();

            var missileAnimationHandler = _animationHandlerFactory.GetAnimationHandler(missileTiles, new AnimationSettings(1, isPlaying:false));
            var propulsionAnimationHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTiles, new AnimationSettings(6, isPlaying:true, isLooping:true));
            

            return new Missile(missileAnimationHandler,
                Vector2.Zero, 
                Vector2.Zero, 
                0, propulsionAnimationHandler,
                _particleFactory.BuildExplosionParticle(),
                owner,
                null,
                _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Small_Explo"),
                health: bulletHealth,
                damage: damage);
        }

        public Bullet GetLaser(Actor owner, float damage,float bulletHealth)
        {
            var laserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\laser.json", 0);
            var laserTileAnimation =
                _animationHandlerFactory.GetAnimationHandler(laserTileSet, new AnimationSettings(1, isPlaying: false));
            var flightEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/laser_impact");
            return new Laser(laserTileAnimation, 
                Vector2.Zero,
                0,
                _particleFactory.BuildLaserExplosionParticle(),
                owner, 
                flightEffect,
                impactSound,
                health: bulletHealth,
                damage: damage);

        }
        // Todo: only the laser color is different. make laser color a enum and use method above
        public Bullet GetEnemyLaser(Actor owner, float damage, float bulletHealth)
        {
            var laserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\enemylaser.json", 0);
            var laserTileAnimation =
                _animationHandlerFactory.GetAnimationHandler(laserTileSet, new AnimationSettings(1, isPlaying: false));
            var flightEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/laser_impact");
            return new Laser(laserTileAnimation, 
                Vector2.Zero,
                0,
                _particleFactory.BuildLaserExplosionParticle(),
                owner, 
                flightEffect,
                impactSound, 
                health: bulletHealth,
                damage: damage);

        }
        
        public Bullet GetProjectile(Actor owner, float damage, float bulletHealth)
        {
            var projectileTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\projectile_3_4.json", 0);
            var projectileTileAnimation =
                _animationHandlerFactory.GetAnimationHandler(projectileTileSet, new AnimationSettings(1, isPlaying: false));
            var flightEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/MinigunProjectile_impact");
            return new Projectile(projectileTileAnimation, 
                Vector2.Zero,
                0,
                _particleFactory.BuildProjectileExplosionParticle(),
                owner, 
                flightEffect,
                impactSound,
                health: bulletHealth,
                damage: damage);

        }
        
        public Bullet GetPallet(Actor owner, float damage, float bulletHealth)
        {
            var projectileTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\shotgunpallet.json", 0);
            var projectileTileAnimation =
                _animationHandlerFactory.GetAnimationHandler(projectileTileSet, new AnimationSettings(1, isPlaying: false));
            var flightEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/shotgunpallet_impact");
            return new Pallet(projectileTileAnimation, 
                Vector2.Zero,
                0,
                _particleFactory.BuildProjectileExplosionParticle(),
                owner, 
                flightEffect,
                impactSound,
                health: bulletHealth,
                damage: damage);

        }

    }
}