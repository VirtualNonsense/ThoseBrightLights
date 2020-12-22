using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class WeaponFactory
    {
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;

        public WeaponFactory(AnimationHandlerFactory animationHandlerFactory, ParticleFactory particleFactory, TileSetFactory tileSetFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _particleFactory = particleFactory;
            _tileSetFactory = tileSetFactory;
        }

        public MissileLauncher GetMissileLauncher(ContentManager contentManager)
        {
            var textureTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missile.json",0);
            //Texture2D propulsion = contentManager.Load<Texture2D>("Artwork/projectiles/missile_propulsion_15_15");
            //TileSet propulsionTileSet = new TileSet(propulsion,15,15, null);
            var propulsionTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\missile_propulsion_15_15.json", 0);
            SoundEffect flightEffect = null; //contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Flight_plane_c");
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Big_Explo");
            var m = new MissileLauncher(_animationHandlerFactory,textureTileSet, propulsionTileSet,
                _particleFactory, null, flightEffect, impactSound);
            return m;
        }

        public Lasergun GetLasergun(ContentManager contentManager)
        {
            var laserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\laser.json",0);
            SoundEffect soundEffect = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/Laser_Short");
            SoundEffect flightEffect = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Clink");
            var l = new Lasergun(_animationHandlerFactory, laserTileSet, _particleFactory, soundEffect, flightEffect, impactSound);
            return l;
        }

        public Lasergun EnemyGetLasergun(ContentManager contentManager)
        {
            var enemylaserTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\enemylaser.json", 0);
            SoundEffect soundEffect = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot/Laser_Short");
            SoundEffect flightEffect = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Airborne/Wobble_test");
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/Clink");
            var k = new Lasergun(_animationHandlerFactory, enemylaserTileSet, _particleFactory, soundEffect, flightEffect, impactSound);
            return k;
        }
    }
}