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

        public PlayerFactory(AnimationHandlerFactory animationHandlerFactory, InputFactory inputFactory, WeaponFactory weaponFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _inputFactory = inputFactory;
            _weaponFactory = weaponFactory;
        }

        public Player GetInstance(ContentManager contentManager)
        {
            var texture2D = contentManager.Load<Texture2D>("Artwork/Actors/ship_135_86");
            var tileSet = new TileSet(texture2D);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var input = _inputFactory.GetInstance();
            var p = new Player(_animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings),input);
            p.Position = new Vector2(-50,50);
            p.AddWeapon(_weaponFactory.GetLasergun(contentManager));

            return p;
        } 
    }
}