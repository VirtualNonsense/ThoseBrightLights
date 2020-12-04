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
        private Vector2 Direction => new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        protected new float Velocity;
        protected float Acceleration;
        protected float MaxTime;
        protected float TimeAlive;

        protected Bullet(AnimationHandler animationHandler, Particle explosion) : base(animationHandler)
        {
            Explosion = explosion;
            Velocity = 0;
            Acceleration = 0;
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
            }

            Explosion.Position =
                Position + new Vector2(_animationHandler.FrameWidth / 2, _animationHandler.FrameHeight / 2);
            base.Update(gameTime);
        }

        protected override void InvokeOnCollide()
        {
            base.InvokeOnCollide();
        }

    }
}
