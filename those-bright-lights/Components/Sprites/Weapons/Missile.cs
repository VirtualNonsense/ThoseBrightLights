using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Missile : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private readonly AnimationHandler _propulsionAnimationHandler;
        private readonly Vector2 _offSet;
        private float _elapsedTime = 0;

        public Missile(AnimationHandler animationHandler, Vector2 spaceShipVelocity, AnimationHandler propulsion, Particle explosion) : base(animationHandler, explosion)
        {
            _spaceShipVelocity = spaceShipVelocity;
            _propulsionAnimationHandler = propulsion;
            _offSet = new Vector2(-animationHandler.FrameWidth/2-_propulsionAnimationHandler.FrameWidth/2,0);
            Layer -= 2 * Single.Epsilon;
            _propulsionAnimationHandler.Settings.Layer = Layer;
            Acceleration = new Vector2(3,0);
        }


        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            Position += Movement(_spaceShipVelocity,_elapsedTime);
            _propulsionAnimationHandler.Position = Position + _offSet; 
            _propulsionAnimationHandler.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _propulsionAnimationHandler.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }

        protected override void InvokeOnCollide()
        {
            base.InvokeOnCollide();
        }
    }
}