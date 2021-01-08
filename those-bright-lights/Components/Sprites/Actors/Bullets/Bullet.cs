using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Bullets
{
    public class Bullet : Actor
    {
        private Vector2 Direction => new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        protected new float Velocity;
        protected float Acceleration;
        protected float MaxTime;
        private float _timeAlive;
        private readonly Logger _logger;
        protected SoundEffect MidAirSound;
        protected float MidAirSoundCooldown;
        protected float TimeSinceUsedMidAir;

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                if (Explosion == null) return;
                Explosion.Position = base.Position;
            }
        }

        public override float Layer
        {

            get => base.Layer;
            set
            {
                base.Layer = value;
                if (Explosion == null) return;
                Explosion.Layer = base.Layer;
            }
        }

        public override float Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = value;
                if (Explosion == null) return;
                Explosion.Rotation = base.Rotation;
            }
        }

        protected Bullet(AnimationHandler animationHandler,
                         Actor parent,
                         Particle explosion,
                         SoundEffect midAirSound,
                         SoundEffect impactSound,
                         float damage) 
            : base(animationHandler, impactSound)
        {
            Parent = parent;
            Explosion = explosion;
            Velocity = 0;
            Acceleration = 0;
            MidAirSound = midAirSound;
            _logger = LogManager.GetCurrentClassLogger();
            Damage = damage;
        }

        protected Vector2 Movement(Vector2 spaceshipVelocity, float elapsedTime)
        {
            var position = spaceshipVelocity +
                           0.5f * Acceleration * Direction * elapsedTime + Velocity * Direction + Position;
            return position;
        }

        public override void Update(GameTime gameTime)
        {
            _timeAlive += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            //TODO: replace with CooldownAbility
            if (_timeAlive >= MaxTime)
            {
                IsRemoveAble = true;
            }
            base.Update(gameTime);
        }
        
        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                default:
                    if (Parent == other) return;
                    IsRemoveAble = true;
                    InvokeExplosion();
                    break;
            }
        }
    }
}
