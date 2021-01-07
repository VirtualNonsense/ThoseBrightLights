using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Bullets
{
    public class Missile : Bullet
    {
        private readonly Vector2 _spaceShipVelocity;
        private readonly AnimationHandler _propulsionAnimationHandler;
        private readonly Vector2 _offSet;
        private float _elapsedTime = 0;

        public Missile(AnimationHandler animationHandler, 
                       Vector2 spaceShipVelocity,
                       Vector2 spaceShipPosition,
                       float rotation,
                       AnimationHandler propulsion,
                       Particle explosion,
                       Sprite parent,
                       SoundEffect midAirSound,
                       SoundEffect impactSound,
                       float damage = 20) 
            : base(animationHandler, explosion, midAirSound, impactSound, damage)
        {
            Rotation = rotation;
            Parent = parent;
            _spaceShipVelocity = spaceShipVelocity;
            var positionOffset = new Vector2(0,10);
            Position = spaceShipPosition + positionOffset;
            _propulsionAnimationHandler = propulsion;
            _propulsionAnimationHandler.Origin = new Vector2(_propulsionAnimationHandler.FrameWidth + animationHandler.FrameWidth/2,animationHandler.FrameHeight/2);
            _offSet = new Vector2(-animationHandler.FrameWidth/2-_propulsionAnimationHandler.FrameWidth/2,0);
            Acceleration = 5;
            MaxTime = 5;
        }


        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            TimeSinceUsedMidAir += gameTime.ElapsedGameTime.Milliseconds;
            Position = Movement(_spaceShipVelocity,_elapsedTime);
            _propulsionAnimationHandler.Position =  Position; 
            _propulsionAnimationHandler.Rotation = Rotation;
            _propulsionAnimationHandler.Update(gameTime);
            if (MidAirSoundCooldown < TimeSinceUsedMidAir)
            {
                TimeSinceUsedMidAir = 0;
                MidAirSound?.Play();
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _propulsionAnimationHandler.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}