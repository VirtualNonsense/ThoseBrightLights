using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;
using NLog;

namespace SE_Praktikum.Services.Factories
{
    public class PowerUpFactory
    {
        public Logger _logger;
        private AnimationHandlerFactory _animationHandlerFactory;
        private readonly ParticleFactory _particleFactory;
        private readonly TileSetFactory _tileSetFactory;
        private readonly ContentManager _contentManager;

        public PowerUpFactory(AnimationHandlerFactory animationHandlerFactory, ParticleFactory particleFactory, TileSetFactory tileSetFactory,ContentManager contentManager)
        {
            _animationHandlerFactory = animationHandlerFactory;
            _particleFactory = particleFactory;
            _tileSetFactory = tileSetFactory;
            _contentManager = contentManager;
        }

        public HealthPowerUp GetInstance(float health)
        {
            var tileSet = _tileSetFactory.GetInstance(@".\Content\MetaData\TileSets\PowerUpTry1.json", 0);
            var animationSettings = new AnimationSettings(1, isPlaying: false);
            var hp = new HealthPowerUp(_animationHandlerFactory.GetAnimationHandler(tileSet, animationSettings), healthbonus:health);
            return hp;
        }
    }
}
