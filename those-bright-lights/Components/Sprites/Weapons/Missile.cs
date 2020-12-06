using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        public Missile(AnimationHandler animationHandler, Vector2 spaceShipVelocity,Vector2 spaceShipPosition,float rotation, AnimationHandler propulsion, Particle explosion, Sprite parent, SoundEffect midAirSound, SoundEffect impactSound) : base(animationHandler, explosion, midAirSound, impactSound)
        {
            Rotation = rotation;
            Parent = parent;
            _spaceShipVelocity = spaceShipVelocity;
            var positionOffset = new Vector2(0,10);
            Position = spaceShipPosition + positionOffset;
            _propulsionAnimationHandler = propulsion;
            _offSet = new Vector2(-animationHandler.FrameWidth/2-_propulsionAnimationHandler.FrameWidth/2,0);
            Acceleration = 5;
            MaxTime = 5;
            Damage = 20;
        }


        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            _timeSinceUsedMidAir += gameTime.ElapsedGameTime.Milliseconds;
            Position = Movement(_spaceShipVelocity,_elapsedTime);
            _propulsionAnimationHandler.Position =  Position + _offSet; 
            _propulsionAnimationHandler.Update(gameTime);
            if (_midAirSoundCooldown < _timeSinceUsedMidAir)
            {
                _timeSinceUsedMidAir = 0;
                _midAirSound?.Play();
            }
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