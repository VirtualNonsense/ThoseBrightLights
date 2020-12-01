using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Missile : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private AnimationHandler _propulsionAnimationHandler;
        private Vector2 OffSet;
        private Vector2 _velocity;
        private float _elapsedTime = 0;

        public Missile(AnimationHandler animationHandler, Vector2 spaceShipVelocity, AnimationHandler propulsion) : base(animationHandler)
        {
            _spaceShipVelocity = spaceShipVelocity;
            _propulsionAnimationHandler = propulsion;
            OffSet = new Vector2(-animationHandler.FrameWidth/2-_propulsionAnimationHandler.FrameWidth/2,0);
            Layer -= 2 * Single.Epsilon;
            _propulsionAnimationHandler.Settings.Layer = Layer;
            _velocity = new Vector2(2,0);
        }


        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            Position += _spaceShipVelocity + _velocity*_elapsedTime;
            _propulsionAnimationHandler.Position = Position + OffSet; 
            _propulsionAnimationHandler.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _propulsionAnimationHandler.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }
    }
}