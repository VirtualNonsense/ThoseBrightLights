using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class EnemyFactory
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;

        public EnemyFactory(AnimationHandlerFactory animationHandlerFactory, WeaponFactory weaponFactory, TileSetFactory tileSetFactory, ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _weaponFactory = weaponFactory;
            _tileSetFactory = tileSetFactory;
            _contentManager = contentManager;
        }

         public Alienship GetAlienship()
         {
             SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
             var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\alien_ship_65_65_4.json",0);
             var animationSettings = new AnimationSettings(4, isPlaying: true, duration: 200f, isLooping: true);
             var propulsionTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\alienshippropulsion_35_9_6.json",0);
             var propulsionHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTileSet,
                 new AnimationSettings(frames: 6, duration: 75, isLooping: true));
             var e = new Alienship(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                  viewBox:new Polygon(Vector2.Zero, Vector2.Zero, 0,
                      new List<Vector2> { new Vector2(0, 0), new Vector2(300, -100), new Vector2(300, 100), }), impactSound: impactSound, propulsion: propulsionHandler)
             {
             };
             e.AddWeapon(_weaponFactory.GetEnemyLaserGun(e));
        
             return e;
         }

        public Turret GetTurret()
        {
            SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\turret_16_21.json", 0);
            var animationSettings = new AnimationSettings(1, isPlaying: false);
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
            SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\mine_29_29_8.json", 0);         
            var animationSettings = new AnimationSettings(8, isLooping:true);
            var m = new Enemy(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                impactSound: impactSound
              )
            {
                Scale = 2
            };
            

            return m;
        }


        public Boss GetBoss()
        {
            SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ey");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\boss.json", 0);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var b = new Boss(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                viewBox: new Polygon(Vector2.Zero,
                    Vector2.Zero, 0,
                    new List<Vector2>
                    {
                        new Vector2(0, 0),
                        new Vector2(100, -300),
                        new Vector2(100, 300),
                    }),
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
            SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\kamikazeIdle_40_25_6.json", 0);
            var animationSettings = new AnimationSettings(6, 50, isPlaying:true, isLooping:true);
            var b = new Kamikaze(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                new Polygon(Vector2.Zero, Vector2.Zero, 0,
                    new List<Vector2> {new Vector2(0, 0), new Vector2(400, -150), new Vector2(400, 150),}),impactSound:impactSound)
            
            {
                Scale = 2
            };
            

            return b;
        }
    }
}