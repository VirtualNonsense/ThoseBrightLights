using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors.Bullets
{
    public abstract class Bullet : Actor
    {
        private Vector2 Direction => new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        protected float Acceleration;
        protected float MaxTime;
        private float _timeAlive;
        private readonly Logger _logger;
        protected readonly SoundEffect MidAirSound;
        protected float MidAirSoundCooldown;
        protected float TimeSinceUsedMidAir;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Base class for all Bullets
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="parent"></param> Actor the bullet is shot from
        /// <param name="explosion"></param>
        /// <param name="midAirSound"></param>
        /// <param name="impactSound"></param>
        /// <param name="damage"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="indestructible"></param>
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
        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
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

        // #############################################################################################################
        // public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            _timeAlive += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (_timeAlive >= MaxTime)
            {
                IsRemoveAble = true;
            }
            base.Update(gameTime);
        }
        
        // #############################################################################################################
        // protected/private Methods
        // #############################################################################################################
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

        
        protected override void ExecuteInteraction(Actor other)
        {
            switch (other)
            {
                default:
                    if (Parent == other) return;
                    Health -= other.Damage;
                    ImpactSound?.Play();
                    break;
            }
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.BulletDiedEventArgs();
        }
    }
}
