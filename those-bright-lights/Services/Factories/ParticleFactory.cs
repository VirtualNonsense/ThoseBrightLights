using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.Factories
{
    public class ParticleFactory
    {
        private readonly IScreen _screen;
        private readonly AnimationHandlerFactory _factory;

        public ParticleFactory(IScreen screen, AnimationHandlerFactory _factory)
        {
            _screen = screen;
            this._factory = _factory;
        }

        public ExplosionsParticle BuildExplosionParticle(Animation animations, AnimationSettings settings)
        {
            return new ExplosionsParticle(_factory.GetAnimationHandler(animations, settings), _screen);
        }
        public ExplosionsParticle BuildExplosionParticle(Texture2D texture)
        {
            return new ExplosionsParticle(texture, _screen);
        }
        
    }
}