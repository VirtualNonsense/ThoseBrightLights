using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;
using System;
using System.Collections.Generic;
using System.Text;
using SE_Praktikum.Models;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Bullet : Actor
    {
        public Particle Explosion;
        protected Vector2 Velocity;
        protected Vector2 Acceleration;
        protected Vector2 BulletPosition;
        public Bullet(AnimationHandler animationHandler, Particle explosion) : base(animationHandler)
        {
            Explosion = explosion;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }

        public Vector2 Movement(Vector2 spaceshipVelocity,
            float elapsedGameTime)
        {
            Vector2 position = spaceshipVelocity * elapsedGameTime +
                               0.5f * Acceleration * elapsedGameTime * elapsedGameTime + Velocity * elapsedGameTime;
            return position;
        }
        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

       

        public override void Update(GameTime gameTime)
        {
            Explosion.Position = Position;
            base.Update(gameTime);
        }

        protected override void InvokeOnCollide()
        {
            base.InvokeOnCollide();
        }

    }
}
