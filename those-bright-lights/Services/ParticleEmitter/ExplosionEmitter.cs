﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Services.ParticleEmitter
{
    public class ExplosionEmitter : ParticleEmitter
    {
        private readonly Random _random;
        private readonly ParticleFactory _factory;


        private readonly ILogger _logger;
        private Rectangle _spawnArea;

        public ExplosionEmitter(IScreen screen, ParticleFactory factory)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _factory = factory;
            _spawnArea = new Rectangle(0, 0, screen.ScreenWidth, 0);
            _random = new Random() ;
        }
        
        
        public Rectangle SpawnArea
        {
            get => _spawnArea;
            set => _spawnArea = value;
        }

        protected override Sprite GenerateParticle()
        {   
            
            var xPosition = _spawnArea.X + _random.Next(0, _spawnArea.Width);
            var yPosition = _spawnArea.Y + _random.Next(0, _spawnArea.Height);
            var ySpeed = _random.Next(10, 100) / 100f;
            var animation_duration = _random.Next(10, 200);
            
            var settings = new AnimationSettings(updateList: new List<(int, float)>
            {
                (0,animation_duration),
                (2,2*animation_duration),
                (3,3*animation_duration),
                (4,4*animation_duration),
                (5,5*animation_duration),
                (6,6*animation_duration),
            });

            var sprite = _factory.BuildExplosionParticle(TileSet, settings);
            
            sprite.Position = new Vector2(xPosition, yPosition);
            sprite.Opacity = (float) _random.NextDouble();
            sprite.Rotation = MathHelper.ToRadians(_random.Next(0, 360));
            sprite.Scale = (float) _random.NextDouble() + _random.Next(0, 3);
            sprite.Velocity = new Vector2(0, ySpeed);
            sprite.Layer = sprite.Opacity*20;


            return sprite;
        }
    }
}