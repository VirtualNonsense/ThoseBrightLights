using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;
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
            var texture2D = contentManager.Load<Texture2D>("Artwork/Actors/spaceship");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\spaceship.json",0);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var input = _inputFactory.GetInstance();
            var propulsionTileSet =_tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\flyingEngineOnly.json",0);
            var propulsionHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTileSet,
                new AnimationSettings(frames: 6, duration: 75, isLooping: true));
            var p = new Player(_animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings), propulsionHandler, input);
            p.Position = new Vector2(-50,50);
            p.AddWeapon(_weaponFactory.GetLaserGun(p));

            return p;
        } 
    }
}