using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NLog;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services.Factories;

namespace ThoseBrightLights.Services.ParticleEmitter
{
    public class ExplosionEmitter
    {
        private readonly Random _random;
        private readonly ParticleFactory _particleFactory;


        private readonly ILogger _logger;
        private Rectangle _spawnArea;

        public ExplosionEmitter(IScreen screen, ParticleFactory particleFactory)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _particleFactory = particleFactory;
            _spawnArea = new Rectangle(0, 0, screen.ScreenWidth, 0);
            _random = new Random() ;
        }
        
        
        public Rectangle SpawnArea
        {
            get => _spawnArea;
            set => _spawnArea = value;
        }

    }
}