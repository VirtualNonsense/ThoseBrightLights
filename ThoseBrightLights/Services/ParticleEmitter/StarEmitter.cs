using Microsoft.Xna.Framework;
using ThoseBrightLights.Components;
using ThoseBrightLights.Services.Factories;

namespace ThoseBrightLights.Services.ParticleEmitter
{
    public class StarEmitter : ParticleEmitter
    {
        private readonly int _aliveTime;
        private readonly ParticleFactory _factory;

        public StarEmitter(int spawnTime, int aliveTime, ParticleFactory factory) : base(spawnTime)
        {
            _aliveTime = aliveTime;
            _factory = factory;
        }

        protected override Particle SpawnParticle(Vector2 pos)
        {
            var p = _factory.BuildStarEffectParticle(_random.Next(_aliveTime - _aliveTime / 10,
                _aliveTime + _aliveTime / 10));
            p.Position = pos;
            return p;
        }
    }
}