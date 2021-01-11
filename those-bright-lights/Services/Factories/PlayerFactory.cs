using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Actors;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class PlayerFactory
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly InputFactory _inputFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly TileSetFactory _tileSetFactory;

        public PlayerFactory(AnimationHandlerFactory animationHandlerFactory, InputFactory inputFactory, WeaponFactory weaponFactory, TileSetFactory tileSetFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _inputFactory = inputFactory;
            _weaponFactory = weaponFactory;
            _tileSetFactory = tileSetFactory;
        }

        public Player GetInstance(ContentManager contentManager)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\shipv3.json",0);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var input = _inputFactory.GetInstance();
            var propulsionTileSet =_tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\flyingEngineOnly.json",0);
            var propulsionHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTileSet,
                new AnimationSettings(frames: 6, duration: 75, isLooping: true));
            var p = new Player(_animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings), propulsionHandler, input);
            p.Position = new Vector2(-50,50);
            p.AddWeapon(_weaponFactory.GetUpperLaserGun(p));
            p.AddWeapon(_weaponFactory.GetLowerLaserGun(p));

            return p;
        } 
    }
}