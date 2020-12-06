using System.Linq.Expressions;
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

        public WeaponFactory(AnimationHandlerFactory animationHandlerFactory, ParticleFactory particleFactory)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _particleFactory = particleFactory;
        }

        public MissileLauncher GetMissileLauncher(ContentManager contentManager)
        {
            Texture2D texture = contentManager.Load<Texture2D>("Artwork/projectiles/missile");
            TileSet textureTileSet = new TileSet(texture);
            Texture2D propulsion = contentManager.Load<Texture2D>("Artwork/projectiles/missile_propulsion_15_15");
            TileSet propulsionTileSet = new TileSet(propulsion,15,15);
            var m = new MissileLauncher(_animationHandlerFactory,textureTileSet, propulsionTileSet,
                _particleFactory);
            return m;
        }

        public Lasergun GetLasergun(ContentManager contentManager)
        {
            Texture2D texture = contentManager.Load<Texture2D>("Artwork/projectiles/laser");
            TileSet textureTileSet = new TileSet(texture);
            var l = new Lasergun(_animationHandlerFactory, textureTileSet, _particleFactory);
            return l;
        }
    }
}