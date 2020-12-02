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
        public Bullet(AnimationHandler animationHandler, Particle explosion) : base(animationHandler)
        {
            Explosion = explosion;
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
            var p = Position; 
            base.InvokeOnCollide();
        }

    }
}
