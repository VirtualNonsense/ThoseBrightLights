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
            Texture2D _texture = contentManager.Load<Texture2D>("Artwork/projectiles/missile");
            TileSet tileSet = new TileSet(_texture);
            Texture2D _propulsion = contentManager.Load<Texture2D>("Artwork/projectiles/missile_propulsion_15_15");
            TileSet propulsionTileSet = new TileSet(_propulsion,15,15);
            var m = new MissileLauncher(_animationHandlerFactory,tileSet, propulsionTileSet,
                () => _particleFactory.BuildExplosionParticle(contentManager));
            return m;
        }
    }
}