using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Extensions;
using SE_Praktikum.Services;
using SE_Praktikum.Components;
using NLog;
using NLog.LayoutRenderers;
using SE_Praktikum.Components.Sprites.Actors.Bullets;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public abstract class SpaceshipAddOn : Actor
    {
        private readonly Vector2 _rotationPoint;
        private float _relativeRotation;
        private Vector2 _relativePosition;
        private Logger _logger;
        private float _absoluteRotation;
        public float MaxRelativeRotation;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        protected SpaceshipAddOn(
            AnimationHandler animationHandler, 
            Actor parent, 
            Vector2 relativePosition, 
            float relativeRotation, 
            SoundEffect impactSound, 
            float health = 100, 
            float? maxHealth = null,
            Vector2? rotationPoint = null,
            float? maxRelativeRotation = null) 
            : base(
                animationHandler, 
                impactSound, 
                health, 
                maxHealth)
        {
            Parent = parent;
            RelativePosition = relativePosition;
            RelativeRotation = relativeRotation;
            _rotationPoint = rotationPoint ?? Vector2.Zero;
            MaxRelativeRotation = maxRelativeRotation ?? MathExtensions.DegToRad(30);
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public override Actor Parent
        {
            get => base.Parent;
            set
            {
                if (value == null) return;
                if (base.Parent != null)
                {
                    base.Parent.OnPositionChanged -= ParentOnOnPositionChanged;
                    base.Parent.OnRotationChanged -= ParentOnOnRotationChanged;
                    base.Parent.OnLayerChanged -= ParentOnOnLayerChanged;
                    base.Parent.OnFlippedChange -= ParentOnOnFlippedChange;
                }

                base.Parent = value;
                base.Parent.OnPositionChanged += ParentOnOnPositionChanged;
                base.Parent.OnRotationChanged += ParentOnOnRotationChanged;
                base.Parent.OnLayerChanged += ParentOnOnLayerChanged;
                base.Parent.OnFlippedChange += ParentOnOnFlippedChange;
            } 
        }
        public Vector2 RelativePosition
        {
            get => _relativePosition;
            set
            {
                _relativePosition = value;
                UpdateBasePosition();
            }
        }
    
        public float RelativeRotation
        {
            get => _relativeRotation;
            set
            {
                if (Parent == null || value == _relativeRotation) return;
                if (Math.Abs(value) > MaxRelativeRotation)
                    value = MaxRelativeRotation * Math.Sign(value);
                Rotation = Parent.Rotation + value;
                _relativeRotation = value;
                RelativePosition.RotateAroundPoint(value, _rotationPoint);
                UpdateBasePosition();
            }
        }

        public override float Rotation
        {
            get => _absoluteRotation;
            set
            {
                _absoluteRotation = value;
                UpdateBasePosition();
                base.Rotation = value;
            }
        }

        public override Vector2 Position
        {
            get => base.Position;
            set => throw new NotSupportedException("Should not set Position");
        }

        
        private Vector2 TransformIntoWorldSpace(Vector2 vector2)
        {
            var (x, y) = vector2;
            var (px, py) = Parent.Position;
            return new Vector2(x + px, y + py);
        }

        
        
        // #############################################################################################################
        // Public Methods
        // #############################################################################################################

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Parent != null)
                _animationHandler.SpriteEffects = Parent.FlippedHorizontal ? SpriteEffects.FlipVertically : SpriteEffects.None;
            base.Draw(spriteBatch);
        }

        public void Rotate(float rotation)
        {
            _relativeRotation += rotation;
        }
        
        // #############################################################################################################
        // protected/private Methods
        // #############################################################################################################
        

        
        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                case Bullet b:
                    if (b.Parent == Parent) return;
                    Health -= b.Damage;
                    _logger.Debug($"{this} health: {Health}");
                    break;
                case Tile t:
                    ApproachDestination(other, 100);
                    break;
            }
        }

        protected override void ApproachDestination(Actor other, int maxIteration, int iteration = 0)
        {
            if (iteration >= maxIteration)
            {
                _logger.Debug($"Approachdestination after {iteration} abborted");
                return;
            }
            if (Parent.DeltaPosition.Length() <= 10 * float.Epsilon)
            {
                var v = Parent.Position - other.Position;
                v /= v.Length();
                Parent.Position += 10 * v;
            }
            else
            {
                Parent.Position -= DeltaPosition;
                Parent.DeltaPosition /= 2;
                Parent.Position += DeltaPosition;
            }

            if (!Collide(other))
                return;
            iteration++;
            ApproachDestination(other, maxIteration, iteration);
        }

        protected void UpdateBasePosition()
        {
            if (Parent == null) return;
            base.Position = Parent.Position + RelativePosition.Rotate(Parent.Rotation);
        }
        
        private void ParentOnOnFlippedChange(object sender, EventArgs e)
        {
            RelativePosition = new Vector2(_relativePosition.X,-_relativePosition.Y);
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
            UpdateBasePosition();
        }

    }
}