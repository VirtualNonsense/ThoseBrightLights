using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Laser : Bullet
    {
        private readonly Vector2 _positionSpaceship;
        private readonly Vector2 _spaceShipVelocity;
        private float _elapsedTime = 0;

        public Laser(AnimationHandler animationHandler,Vector2 positionSpaceship, Particle explosion) : base(animationHandler, explosion)
        {
            Position = positionSpaceship;
            Velocity = new Vector2(5,0);
            Acceleration = Vector2.Zero;
            _spaceShipVelocity = Vector2.Zero;
        }
        
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            Position = Movement(_spaceShipVelocity,_elapsedTime);
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