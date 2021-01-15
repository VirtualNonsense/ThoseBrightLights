using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Spaceships
{
    public abstract class EnemyWithViewbox : Enemy
    {
        public Polygon ViewBox;
        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                if (Rotation < 3 * Math.PI/2 && Rotation > Math.PI/2)
                {
                    if (HitBoxFlipped) return;
                    ViewBox = ViewBox?.MirrorSingleVertical(Position);
                }
                else if (HitBoxFlipped)
                {
                    ViewBox = ViewBox?.MirrorSingleVertical(Position);
                }
            }
        }


        protected EnemyWithViewbox(AnimationHandler animationHandler,
            Polygon viewBox,
            float maxSpeed = 3,
            float acceleration = 5,
            float rotationAcceleration = .1f,
            float maxRotationSpeed = 10,
            float health = 50,
            float? maxHealth = null,
            float impactDamage = 5,
            SoundEffect impactSound = null) : base(animationHandler, maxSpeed, acceleration, rotationAcceleration,
            maxRotationSpeed, health, maxHealth, impactDamage, impactSound: impactSound)
        {
            ViewBox = viewBox;
            RotateWeapon = true;
        }


        public override void Update(GameTime gameTime)
        {
            ViewBox.Position = Position;
            ViewBox.Rotation = Rotation;
            ViewBox.Layer = Layer;
            Rotate(Target, gameTime);
            Shoot.Update(gameTime);
            if (I == InterAction.InView && Target != null)
                Shoot.Fire();

            base.Update(gameTime);
        }

        
        

        protected override bool InteractAble(Actor other)
        {
            switch (other)
            {
                case Player p:
                    if (p.HitBox.Any(polygon => ViewBox.Overlap(polygon)))
                    {
                        I = InterAction.InView;
                        return true;
                    }

                    break;
            }

            return base.InteractAble(other);
        }

        protected override void Rotate(Actor target, GameTime gameTime)
        {
            if (!RotateWeapon || CurrentWeapons.Count == 0)
            {
                base.Rotate(target, gameTime);
                return;
            }
            if (Target == null || I != InterAction.InView) return;
            var weapon = CurrentWeapons[^1];
            if (weapon == null) return;
            var desiredRotation = MathExtensions.RotationToTarget(target, this);
            if (!(Math.Abs(desiredRotation - weapon.RelativeRotation) > RotationThreshold)) return;
            var rotationPortion =
                (float) ((gameTime.ElapsedGameTime.TotalMilliseconds / RotationSpeed) * (2 * Math.PI));
            //turn clock or anticlockwise
            var angleToRotate = MathExtensions.Modulo2PiAlsoNegative(desiredRotation - weapon.Rotation);
            //from back to front: rotate counter or clockwise
            //-> if the actor is flipped, then +rotation is anticlockwise, hence the sign at the front
            if (Math.Abs(angleToRotate) < RotationThreshold) return;
            weapon.RelativeRotation +=
                Math.Sign(Math.PI - Math.Abs(angleToRotate)) * Math.Sign(angleToRotate) * rotationPortion;
            //TODO: balancing
            if(Math.Abs(desiredRotation - Rotation) > weapon.MaxRelativeRotation*2/3)
                Rotation +=  Math.Sign(Math.PI - Math.Abs(angleToRotate)) * Math.Sign(angleToRotate) * rotationPortion;
        }

    }

   
}