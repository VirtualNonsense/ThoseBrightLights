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

        public Missile(AnimationHandler animationHandler, Vector2 spaceShipVelocity, AnimationHandler propulsion) : base(animationHandler)
        {
            _spaceShipVelocity = spaceShipVelocity;
            _propulsionAnimationHandler = propulsion;
            OffSet = new Vector2(-animationHandler.FrameWidth/2-_propulsionAnimationHandler.FrameWidth/2,0);
            _propulsionAnimationHandler.Settings.Layer = Layer;
        }

        public override void BaseCollide(Actor actor)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            Position += _spaceShipVelocity + Vector2.UnitX;
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