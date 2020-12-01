using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Missile : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;

        public Missile(AnimationHandler animationHandler, Vector2 spaceShipVelocity) : base(animationHandler)
        {
            _spaceShipVelocity = spaceShipVelocity;
        }

        public override void BaseCollide(Actor actor)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            Position += _spaceShipVelocity + Vector2.UnitX;
            base.Update(gameTime);
        }
    }
}