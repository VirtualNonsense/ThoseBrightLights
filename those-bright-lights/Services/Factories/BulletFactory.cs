using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
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
        
        public Bullet GetMissile(Actor owner, float damage)
        {
            var missileTiles =  _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missile.json",0);
            var propulsionTiles =  _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missile_propulsion_15_15.json", 0);
            var particle = _particleFactory.BuildExplosionParticle();

            var missileAnimationHandler = _animationHandlerFactory.GetAnimationHandler(missileTiles, new AnimationSettings(1, isPlaying:false));
            var propulsionAnimationHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTiles, new AnimationSettings(6, isPlaying:true, isLooping:true));
            
            // var flightEffect = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Flight_plane_c");
            // var impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Big_Explo");

            return new Missile(missileAnimationHandler,
                Vector2.Zero, 
                Vector2.Zero, 
                0, propulsionAnimationHandler,
                _particleFactory.BuildExplosionParticle(),
                owner,
                null,
                null,
                damage: damage);
        }

        public Bullet GetLaser(Actor owner, float damage)
        {
            var laserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\laser.json", 0);
            var laserTileAnimation =
                _animationHandlerFactory.GetAnimationHandler(laserTileSet, new AnimationSettings(1, isPlaying: false));
            SoundEffect flightEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Clink");
            return new Laser(laserTileAnimation, 
                Vector2.Zero,
                0,
                _particleFactory.BuildLaserExplosionParticle(),
                owner, 
                flightEffect,
                impactSound,
                damage: damage);

        }
        // Todo: only the laser color is different. make laser color a enum and use method above
        public Bullet GetEnemyLaser(Actor owner, float damage)
        {
            var laserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\enemylaser.json", 0);
            var laserTileAnimation =
                _animationHandlerFactory.GetAnimationHandler(laserTileSet, new AnimationSettings(1, isPlaying: false));
            SoundEffect flightEffect = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Clink");
            return new Laser(laserTileAnimation, 
                Vector2.Zero,
                0,
                _particleFactory.BuildLaserExplosionParticle(),
                owner, 
                flightEffect,
                impactSound, 
                damage: damage);

        }

    }
}