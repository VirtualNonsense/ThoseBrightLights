using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services;
using NLog;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public abstract class SpaceshipAddOn : Actor
    {
        private float _relativeRotation;
        private Vector2 _relativePosition;
        private Logger _logger;
        private float _absoluteRotation;

        public Vector2 RelativePosition
        {
            get => _relativePosition;
            set
            {
                _relativePosition = value;
                Position = Parent.Position + value;
            }
        }

        public float RelativeRotation
        {
            get => _relativeRotation;
            set
            {
                Rotation = Parent.Rotation + value;
                _relativeRotation = value;
            }
        }

        public override float Rotation
        {
            get => _absoluteRotation;
            set
            {
                _absoluteRotation = value;
                Position = TransformIntoWorldSpace(RelativePosition.Rotate(value));
                base.Rotation = value;
            }
        }

        private Vector2 TransformIntoWorldSpace(Vector2 vector2)
        {
            var (x, y) = vector2;
            var (px, py) = Parent.Position;
            return new Vector2(x + px, y + py);
        }

        //TODO: set relative position in constructor
        protected SpaceshipAddOn(
                        AnimationHandler animationHandler, 
                        Actor parent, 
                        Vector2 relativePosition, 
                        float relativeRotation, 
                        SoundEffect impactSound, 
                        float health = 100, 
                        float maxHealth = 100) 
            : base(
                        animationHandler, 
                        impactSound, 
                        health, 
                        maxHealth)
        {
            RelativePosition = relativePosition;
            RelativeRotation = relativeRotation;
            Parent = parent;
            _logger = LogManager.GetCurrentClassLogger();
        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            _animationHandler.Draw(spriteBatch);
            _logger.Info("Drawn at " + Position);
        }

        public override void Update(GameTime gameTime)
        {
            RelativePosition = _relativePosition;
            RelativeRotation = _relativeRotation;
            base.Update(gameTime);
        }
    }
}