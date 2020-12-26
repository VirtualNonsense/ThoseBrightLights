using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class ParticleFactory
    {
        private readonly IScreen _screen;
        private readonly AnimationHandlerFactory _factory;
        private readonly ContentManager _contentManager;
        private readonly TileSetFactory _tileSetFactory;

        public ParticleFactory(IScreen screen, AnimationHandlerFactory _factory, ContentManager contentManager, TileSetFactory tileSetFactory)
        {
            _screen = screen;
            this._factory = _factory;
            _contentManager = contentManager;
           _tileSetFactory = tileSetFactory;
        }

        public ExplosionsParticle BuildExplosionParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(7, 50f, 1);
            var explosionTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\explosion_45_45.json", 0);
            //TileSet explosion = new TileSet(_contentManager.Load<Texture2D>("Artwork /effects/explosion_45_45"), 45, 45, null);
            return new ExplosionsParticle(_factory.GetAnimationHandler(explosionTileSet, animationSettings), _screen);
        }

        public ExplosionsParticle BuildLaserExplosionParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(6, 50f, 1);
            //TileSet explosion = new TileSet(_contentManager.Load<Texture2D>("Artwork/effects/laesr_explosion_6_6_6"),6,6, null);
            var laserexpTileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\laesr_explosion_6_6_6.json", 0);
            return new ExplosionsParticle(_factory.GetAnimationHandler(laserexpTileSet,animationSettings),_screen);
        }
        
    }
}