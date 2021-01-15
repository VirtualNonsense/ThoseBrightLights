using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services.Abilities;

namespace SE_Praktikum.Services.ParticleEmitter
{
    public abstract class ParticleEmitter : IComponent
    {
        private List<Particle> _particles;
        private Logger _logger;

        public static int MaxParticle = 1000;
        public static int GlobalParticleCount = 0;

        private CooldownAbility _spawnAbility;

        public List<Actor> TargetZones;

        public int SpawnTime
        {
            get => _spawnAbility.TargetTime;
            set => _spawnAbility.TargetTime = value;
        }

        protected Random _random;
        
        public ParticleEmitter(int spawnTime)
        {
            _random = new Random();
            _logger = LogManager.GetCurrentClassLogger();
            TargetZones = new List<Actor>();
            _particles = new List<Particle>();
            _spawnAbility = new CooldownAbility(spawnTime, AddParticles);
        }


        public Vector2 Position { get; set; }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in _particles)
            {
                particle.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            _spawnAbility.Update(gameTime);
            _spawnAbility.Fire();
            foreach (var particle in _particles)
            {
                particle.Update(gameTime);
            }

            for (int i = 0; i < _particles.Count;)
            {
                if (_particles[i].IsRemoveAble)
                {
                    _particles.RemoveAt(i);
                    GlobalParticleCount--;
                    continue;
                }
                i++;
            }
        }

        protected virtual void AddParticles()
        {
            foreach (var actor in TargetZones)
            {
                if (GlobalParticleCount >= MaxParticle) return;
                var zone = actor.HitBox.GetBoundingRectangle();
                var x = _random.Next(zone.X, zone.X + zone.Width);
                var y = _random.Next(zone.Y, zone.Y + zone.Height);
                var p = SpawnParticle(new Vector2(x, y));
                p.Layer = actor.Layer + 1;
                _particles.Add(p);
                GlobalParticleCount++;
            }
        }

        protected abstract Particle SpawnParticle(Vector2 pos);

        public bool IsRemoveAble { get; set; }
    }
}