using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using SE_Praktikum.Models;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Bullet : Actor
    {
        public readonly Particle Explosion;
        private Vector2 _direction => new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        protected float Velocity;
        protected float Acceleration;
        protected float maxTime;
        protected float timeAlive;

        protected Bullet(AnimationHandler animationHandler, Particle explosion) : base(animationHandler)
        {
            Explosion = explosion;
            Velocity = 0;
            Acceleration = 0;
        }

        protected Vector2 Movement(Vector2 spaceshipVelocity, float elapsedTime)
        {
            Vector2 position = spaceshipVelocity +
                                0.5f * Acceleration * _direction * elapsedTime + Velocity *_direction + Position;
            return position;
        }
        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

       

        public override void Update(GameTime gameTime)
        {
            timeAlive += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (timeAlive >= maxTime)
            {
                IsRemoveAble = true;
            }
            
            Explosion.Position = Position;
            base.Update(gameTime);
        }

        protected override void InvokeOnCollide()
        {
            base.InvokeOnCollide();
        }

    }
}
