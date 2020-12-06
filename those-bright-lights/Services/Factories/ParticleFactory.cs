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

        public ParticleFactory(IScreen screen, AnimationHandlerFactory _factory, ContentManager contentManager)
        {
            _screen = screen;
            this._factory = _factory;
            _contentManager = contentManager;
        }

        public ExplosionsParticle BuildExplosionParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(7, 50f, 1);
            TileSet explosion = new TileSet(_contentManager.Load<Texture2D>("Artwork/effects/explosion_45_45"), 45, 45);
            return new ExplosionsParticle(_factory.GetAnimationHandler(explosion, animationSettings), _screen);
        }

        public ExplosionsParticle BuildLaserExplosionParticle(AnimationSettings settings = null)
        {
            var animationSettings = settings ?? new AnimationSettings(6, 50f, 1);
            TileSet explosion = new TileSet(_contentManager.Load<Texture2D>("Artwork/effects/laesr_explosion_6_6_6"),6,6);
            return new ExplosionsParticle(_factory.GetAnimationHandler(explosion,animationSettings),_screen);
        }
        
    }
}