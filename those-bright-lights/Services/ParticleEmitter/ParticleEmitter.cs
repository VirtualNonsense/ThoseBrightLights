using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

namespace SE_Praktikum.Services.ParticleEmitter
{
    public abstract class ParticleEmitter : IComponent
    {
        private float _generateTimer;


        protected List<Sprite> _particles;

        private ILogger _logger;

        public TileSet TileSet { get; set; }
        public Texture2D Texture { get; set; }

        /// <summary>
        /// How often a particle is produced
        /// </summary>
        public float GenerateSpeed = 0.005f;

        /// <summary>
        /// How often we apply the "GlobalVelociy" to our particles
        /// </summary>

        public int MaxParticles = 1000;

        public static int ParticleCount = 0;

        public ParticleEmitter()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _particles = new List<Sprite>();
        }

        public virtual void Update(GameTime gameTime)
        {
            _generateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            AddParticle();

            foreach (var particle in _particles)
                particle.Update(gameTime);

            RemovedFinishedParticles();
        }

        protected virtual void AddParticle()
        {
            if (_generateTimer > GenerateSpeed)
            {
                _generateTimer = 0;

                if (ParticleCount < MaxParticles)
                {
                    _particles.Add(GenerateParticle());
                    ParticleCount++;
                }
            }
        }
        
        protected virtual void RemovedFinishedParticles()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsRemoveAble)
                {
                    _particles.RemoveAt(i);
                    i--;
                    ParticleCount--;
                }
            }
        }

        protected abstract Sprite GenerateParticle();

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var particle in _particles)
                particle.Draw(gameTime, spriteBatch);
        }
    }
}