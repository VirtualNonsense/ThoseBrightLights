using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Laser : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private float _elapsedTime = 0;

        public Laser(AnimationHandler animationHandler, Particle explosion) : base(animationHandler, explosion)
        {
            Velocity = new Vector2(2,0);
        }
        
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            Position += _spaceShipVelocity+Velocity;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        protected override void InvokeOnCollide()
        {
            base.InvokeOnCollide();
        }
    }
}