using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors.Bullets
{
    public abstract class Bullet : Actor
    {
        private Vector2 Direction => new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        protected float Acceleration;
        protected float MaxTime;
        private float _timeAlive;
        private readonly Logger _logger;
        protected SoundEffect MidAirSound;
        protected float MidAirSoundCooldown;
        protected float TimeSinceUsedMidAir;
        
        // #################################################################################################################
        // Properties
        // #################################################################################################################
        public float Speed { get; set; }
    

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
                         float damage,
                         float health,
                         float? maxHealth,
                         bool indestructible) 
            : base(animationHandler, impactSound, health: health, maxHealth: maxHealth, indestructible: indestructible)
        {
            Parent = parent;
            Explosion = explosion;
            Speed = 0;
            Acceleration = 0;
            MidAirSound = midAirSound;
            _logger = LogManager.GetCurrentClassLogger();
            Damage = damage;
        }

        public override bool IsRemoveAble
        {
            get=>base.IsRemoveAble;
            set
            {
                base.IsRemoveAble = value;
                if(base.IsRemoveAble)
                    InvokeExplosion();
            }
        }

        protected override bool InteractAble(Actor other)
        {
            if (Parent == other || Parent == other.Parent) return false;
            return base.InteractAble(other);
        }

        protected Vector2 Movement(Vector2 spaceshipVelocity, float elapsedTime)
        {
            var position = spaceshipVelocity +
                           0.5f * Acceleration * Direction * elapsedTime + Speed * Direction + Position;
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
                    Health -= other.Damage;
                    _impactSound?.Play();
                    break;
            }
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.BulletDiedEventArgs();
        }
    }
}
