using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
            SoundEffect impactSound = contentManager.Load<SoundEffect>("Audio/Sound_Effects/Collusion/ClinkBell");
            var tileSet = new TileSet(texture2D, new []
            {
                new Polygon(Vector2.Zero, Vector2.Zero, 0, new List<Vector2>
                {
                    new Vector2(-32.5f, 32.5f),
                    new Vector2(32.5f, 32.5f),
                    new Vector2(32.5f, -32.5f),
                    new Vector2(-32.5f, -32.5f),
                }), 
            });
            var animationSettings = new AnimationSettings(1,isPlaying:false);
            var e = new Enemy(_animationHandlerFactory.GetAnimationHandler(tileSet,animationSettings), impactSound:impactSound);
            e.Position = new Vector2(100,50);
            e.ViewBox = new Polygon(Vector2.Zero, Vector2.Zero, 0, new List<Vector2>
            {
                new Vector2(0,0),
                new Vector2(-300,-100),
                new Vector2(-300,100),
            });
            e.AddWeapon(_weaponFactory.GetLasergun(contentManager));

            return e;
        } 
    }
}