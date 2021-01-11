using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services;
using SE_Praktikum.Components;
using NLog;
using NLog.LayoutRenderers;

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
            get => !Parent.FlippedHorizontal ? _relativePosition : new Vector2(_relativePosition.X, -_relativePosition.Y);
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

        public override Vector2 Position
        {
            get => base.Position;
            set => base.Position = Parent.Position + RelativePosition.Rotate(Rotation);
        }

        private Vector2 TransformIntoWorldSpace(Vector2 vector2)
        {
            var (x, y) = vector2;
            var (px, py) = Parent.Position;
            return new Vector2(x + px, y + py);
        }

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
            base.Parent = parent;
            _relativePosition = relativePosition;
            _relativeRotation = relativeRotation;
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        

        public override Actor Parent
        {
            get => base.Parent;
            set
            {
                if (base.Parent != null)
                {
                    base.Parent.OnPositionChanged -= ParentOnOnPositionChanged;
                    base.Parent.OnRotationChanged -= ParentOnOnRotationChanged;
                    base.Parent.OnLayerChanged -= ParentOnOnLayerChanged;
                }

                base.Parent = value;
                base.Parent.OnPositionChanged += ParentOnOnPositionChanged;
                base.Parent.OnRotationChanged += ParentOnOnRotationChanged;
                base.Parent.OnLayerChanged += ParentOnOnLayerChanged;
            } 
        }

        private void ParentOnOnLayerChanged(object sender, EventArgs e)
        {
            Layer = Parent.Layer;
        }

        private void ParentOnOnRotationChanged(object sender, EventArgs e)
        {
            Rotation = Parent.Rotation;
        }

        private void ParentOnOnPositionChanged(object sender, EventArgs e)
        {
                Position = Parent.Position - _relativePosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _animationHandler.SpriteEffects = Parent.FlippedHorizontal ? SpriteEffects.FlipVertically : SpriteEffects.None;
            base.Draw(spriteBatch);
        }

    }
}