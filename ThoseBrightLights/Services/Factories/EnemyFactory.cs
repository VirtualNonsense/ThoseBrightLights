using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThoseBrightLights.Components.Sprites.Actors;
using ThoseBrightLights.Extensions;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Services.Factories
{
    public class EnemyFactory
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;
        private readonly ParticleFactory _particleFactory;

        public EnemyFactory(AnimationHandlerFactory animationHandlerFactory, WeaponFactory weaponFactory,
            TileSetFactory tileSetFactory, ParticleFactory particleFactory, ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _weaponFactory = weaponFactory;
            _tileSetFactory = tileSetFactory;
            _contentManager = contentManager;
            _particleFactory = particleFactory;
        }

         public AlienShip GetAlienShip()
         {
             var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/alien_impact");
             var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\alien_ship_65_65_4.json",0);
             var animationSettings = new List<AnimationSettings>(new[]
                 {new AnimationSettings(4, isPlaying: true, duration: 200f, isLooping: true)});
             var propulsionTileSet = _tileSetFactory.GetInstance(
                 @".\Content\MetaData\TileSets\alienshippropulsion_35_9_6.json",
                 0);
             var propulsionHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTileSet,
                 new List<AnimationSettings>(new[]
                 {
                     new AnimationSettings(6,
                         75f,
                         isLooping: true)
                 }));
             var e = new AlienShip(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                  viewBox:new Polygon(Vector2.Zero, Vector2.Zero, 0,
                      new List<Vector2>
                      {
                          new Vector2(0, 0),
                          new Vector2(300, -100),
                          new Vector2(300, 100),
                      }),
                  impactSound: impactSound,
                  propulsion: propulsionHandler)
             {
             };
             e.AddWeapon(_weaponFactory.GetEnemyLaserGun(e));
        
             return e;
         }

        public Turret GetTurret()
        {
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/turret_impact");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\turret_16_21.json", 0);
            var animationSettings = new List<AnimationSettings>();
            animationSettings.Add(new AnimationSettings(1, isPlaying: false));
            var e = new Turret(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                impactSound: impactSound,
                viewbox: new Polygon(Vector2.Zero, Vector2.Zero, 0,
                    new List<Vector2> { new Vector2(0, 0), new Vector2(300, -100), new Vector2(300, 100), }))
            {
                Scale = 2
            };
            e.AddWeapon(_weaponFactory.GetTurretLaserGun(e));

            return e;
        }
        public Enemy GetMines()
        {
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\mine_35_35_8_2x.json", 0);         
            var idleAnimationSettings = new AnimationSettings(updateList:new List<(int, float)>
            {
                (0,100f),
                (1,100f),
                (2,100f),
                (3,100f),
                (4,100f),
                (5,100f),
                (6,100f),
                (7,100f),
            }, isLooping:true);
            var explosion = _particleFactory.BuildMineExplosionsParticle();
            var animationSettings = new List<AnimationSettings> {idleAnimationSettings};
            var m = new Mine(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),explosion,
                impactSound: impactSound
              )
            {
                Scale = 2
            };
            

            return m;
        }


        public Boss GetBoss()
        {
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ey");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\boss_64_110_8.json", 0);
            var animationSettings = 
                new List<AnimationSettings>(new []{new AnimationSettings(8,isLooping:true)});
            var propulsionTileSet =
                _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\bosspropulsion_11_9_6.json", 0);
            var propulsionSettings =
                new List<AnimationSettings>(new[] {new AnimationSettings(6, duration: 30f, isLooping: true)});
            var propulsionHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTileSet, propulsionSettings);
            var b = new Boss(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                viewBox: new Polygon(Vector2.Zero,
                    Vector2.Zero, 0,
                    new List<Vector2>
                    {
                        new Vector2(0, 0),
                        new Vector2(1000, -300),
                        new Vector2(1000, 300),
                    }),
                propulsionHandler,
                impactSound: impactSound)
            {
                Scale = 2
            };
            b.AddWeapon(_weaponFactory.GetUpperBossLaserGun(b));
            b.AddWeapon(_weaponFactory.GetLowerBossLaserGun(b));

            return b;
        }
        
        public Kamikaze GetKamikaze()
        {
            var impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/kamikaze_impact");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\kamikaze_40_25_6_2x.json", 0);
            var idleSettings = new AnimationSettings(updateList: new List<(int, float)>
                {
                    (0, 100f),
                    (1, 100f),
                    (2, 100f),
                    (3, 100f),
                    (4, 100f),
                    (5, 100f),
                } , 50, isPlaying: true, isLooping: true);
            var engageSettings = new AnimationSettings(updateList: new List<(int, float)>
            {
                (6, 100f),
                (7, 100f),
                (8, 100f),
                (9, 100f),
                (10, 100f),
                (11, 100f),
            },50, isPlaying: true, isLooping: true);
            var animationSettings = new List<AnimationSettings>(new[] {idleSettings, engageSettings});
            var b = new Kamikaze(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                new Polygon(Vector2.Zero, Vector2.Zero, 0,
                    new List<Vector2>
                    {
                        new Vector2(0, 0),
                        new Vector2(400, -150),
                        new Vector2(400, 150),
                    }),
                impactSound: impactSound)
            
            {
                Scale = 2
            };
            

            return b;
        }
    }
}