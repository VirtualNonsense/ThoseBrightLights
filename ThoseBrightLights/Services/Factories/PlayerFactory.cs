using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThoseBrightLights.Components.Sprites.Actors;
using ThoseBrightLights.Components.Sprites.Actors.Spaceships;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Services.Factories
{
    public class PlayerFactory
    {
        // Fields
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly InputFactory _inputFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public PlayerFactory(AnimationHandlerFactory animationHandlerFactory,
                             InputFactory inputFactory,
                             WeaponFactory weaponFactory, 
                             TileSetFactory tileSetFactory,
                             ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _inputFactory = inputFactory;
            _weaponFactory = weaponFactory;
            _tileSetFactory = tileSetFactory;
            _contentManager = contentManager;
        }
        /// <summary>
        /// Build a player ship via json with desired animation settings, startweapon and position
        /// </summary>
        /// <returns></returns>
        public Player GetInstance()
        {
            SoundEffect impactSound = _contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/player_impact");
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\shipv3.json",0);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var input = _inputFactory.GetInstance();
            var propulsionTileSet =_tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\flyingEngineOnly.json",0);
            var propulsionSettings = new AnimationSettings(6, 75f, isLooping: true);
            var propulsionHandler = _animationHandlerFactory.GetAnimationHandler(propulsionTileSet,
                new List<AnimationSettings>(new[] {propulsionSettings}));
            
            var p = new Player(
                _animationHandlerFactory.GetAnimationHandler(tileSet,
                    new List<AnimationSettings>(new[] {animationSettings})), propulsionHandler, input, impactSound: impactSound);
            p.Position = new Vector2(-50,50);
            p.AddWeapon(_weaponFactory.GetMinigun(p));
            return p;
        } 
    }
}