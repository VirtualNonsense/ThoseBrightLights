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

        public Animation Animation { get; set; }
        public Texture2D Texture { get; set; }

        /// <summary>
        /// How often a particle is produced
        /// </summary>
        public float GenerateSpeed = 0.005f;

        /// <summary>
        /// How often we apply the "GlobalVelociy" to our particles
        /// </summary>

        public int MaxParticles = 1000;
        

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

                if (_particles.Count < MaxParticles)
                {
                    _particles.Add(GenerateParticle());
                }
            }
        }
        
        protected virtual void RemovedFinishedParticles()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsRemoveAble)
                {
                    _logger.Trace("particle removed");
                    _particles.RemoveAt(i);
                    i--;
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