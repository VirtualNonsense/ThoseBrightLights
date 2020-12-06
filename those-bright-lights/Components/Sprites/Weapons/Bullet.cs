using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using SE_Praktikum.Models;
using Microsoft.Xna.Framework.Audio;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Bullet : Actor
    {
        private Vector2 Direction => new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        protected new float Velocity;
        protected float Acceleration;
        protected float MaxTime;
        private float TimeAlive;
        private readonly Logger _logger;
        protected SoundEffect _midAirSound;
        protected float _midAirSoundCooldown;
        protected float _timeSinceUsedMidAir;

        protected Bullet(AnimationHandler animationHandler, Particle explosion, SoundEffect midAirSound) : base(animationHandler)
        {
            Explosion = explosion;
            Velocity = 0;
            Acceleration = 0;
            _midAirSound = midAirSound;
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected Vector2 Movement(Vector2 spaceshipVelocity, float elapsedTime)
        {
            var position = spaceshipVelocity +
                           0.5f * Acceleration * Direction * elapsedTime + Velocity *Direction + Position;
            return position;
        }
        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

       

        public override void Update(GameTime gameTime)
        {
            TimeAlive += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (TimeAlive >= MaxTime)
            {
                IsRemoveAble = true;
                _logger.Info("Removed Bullet because it was on the screen for to long");
            }

            Explosion.Position =
                Position;
            base.Update(gameTime);
        }


        protected override void InvokeOnCollide()
        {
            base.InvokeOnCollide();
        }

    }
}
