using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class EnemyFactory
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly TileSetFactory _tileSetFactory;

        public EnemyFactory(AnimationHandlerFactory animationHandlerFactory, WeaponFactory weaponFactory, TileSetFactory tileSetFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _weaponFactory = weaponFactory;
            _tileSetFactory = tileSetFactory;
        }

        public Enemy GetInstance(ContentManager contentManager)
        {
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\alien_ship.json",0);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var e = new Enemy(_animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings), impactSound:impactSound);
            e.Position = new Vector2(100,50);
            e.AddWeapon(_weaponFactory.EnemyGetLasergun(contentManager));

            return e;
        }

        public Turret GetTurret(ContentManager contentManager)
        {
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\turret_16_21.json",0);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var e = new Turret(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                impactSound: impactSound, 
                viewbox:new Polygon(Vector2.Zero, Vector2.Zero, 0,
                    new List<Vector2> {new Vector2(0, 0), new Vector2(300, -100), new Vector2(300, 100),}))
            {
                Position = new Vector2(100, 50),
                Scale = 2
            };
            e.AddWeapon(_weaponFactory.EnemyGetLasergun(contentManager));

            return e;
        }

        public Boss GetBoss(ContentManager contentManager)
        {
            SoundEffect impactSound;
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\boss.json", 0);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var b = new Boss(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings),
                new Polygon(Vector2.Zero, Vector2.Zero, 0,
                    new List<Vector2> {new Vector2(0, 0), new Vector2(1000, -300), new Vector2(1000, 300),}))
            {
                Position = new Vector2(100, 50),
                Scale = 2
            };
            b.AddWeapon(_weaponFactory.EnemyGetLasergun(contentManager));

            return b;
        }
    }
}