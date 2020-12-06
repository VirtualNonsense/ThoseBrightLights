using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class EnemyFactory
    {
        private readonly AnimationHandlerFactory _animationHandlerFactory;
        private readonly WeaponFactory _weaponFactory;

        public EnemyFactory(AnimationHandlerFactory animationHandlerFactory, WeaponFactory weaponFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _weaponFactory = weaponFactory;
        }

        public Enemy GetInstance(ContentManager contentManager)
        {
            var texture2D = contentManager.Load<Texture2D>("Artwork/Actors/alien_ship_56_59");
            var tileSet = new TileSet(texture2D);
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var e = new Enemy(_animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings));
            e.Position = new Vector2(100,50);
            e.AddWeapon(_weaponFactory.GetLasergun(contentManager));

            return e;
        } 
    }
}