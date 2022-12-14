using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using ThoseBrightLights.Extensions;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites.Actors
{
    public abstract class Actor : Sprite
    {
        protected Actor LastAggressor;
        protected Actor Tool;
        private bool _indestructible;
        protected Particle Explosion;
        private float _health;
        private float _maxHealth;
        public readonly bool CollisionEnabled = true;
        private readonly Logger _logger;
        protected SoundEffect ImpactSound;
        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// Sprite that supports collision
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="impactSound"></param>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        /// <param name="impactDamage"></param>
        /// <param name="indestructible"></param>
        protected Actor(AnimationHandler animationHandler,
                     SoundEffect impactSound,
                     float health = 100,
                     float? maxHealth = 100,
                     float impactDamage = 5, 
                     bool indestructible = false) : base(animationHandler)
        {
            MaxHealth = maxHealth ?? health;
            Health = health;
            _logger = LogManager.GetCurrentClassLogger();
            ImpactSound = impactSound;
            _indestructible = indestructible;
            Damage = impactDamage;
        }
        
        // #############################################################################################################
        // Events
        // #############################################################################################################
        #region Events
        public event EventHandler<LevelEventArgs.ExplosionEventArgs> OnExplosion;
        public event EventHandler<LevelEventArgs.ActorDiedEventArgs> OnDeath;
        public event EventHandler OnHealthChanged;
        public event EventHandler OnMaxHealthChanged;
        public event EventHandler OnFlippedChanged;
        public event EventHandler<LevelEventArgs.InvincibilityChangedEventArgs> OnInvincibilityChanged; 
        
        #endregion
        // #############################################################################################################
        // Properties
        // #############################################################################################################

        public virtual Actor Parent { get; set; }
        public bool FlippedHorizontal => _animationHandler.SpriteEffects == SpriteEffects.FlipVertically;
        public bool IsCollideAble => HitBox != null;
        public Polygon[] HitBox => _animationHandler.CurrentHitBox;
        public float Damage { get; protected set; }
        public float Health {
            get=> _health;
            set
            {
                if (value < _health && Indestructible)
                    return;
                if (value <= 0)
                {
                    _health = 0;
                    IsRemoveAble = true;
                    InvokeOnHealthChanged();
                    InvokeDeath();
                    return;
                }

                if (value > _maxHealth)
                {
                    _health = _maxHealth;
                    InvokeOnHealthChanged();
                    return;
                }
                _health = value;
                InvokeOnHealthChanged();
            }
        }

        public float MaxHealth { 
            get => _maxHealth;
            set
            {
                if (value <= 1)
                {
                    _health = MathExtensions.Remap(value,0,_maxHealth,0,1);
                    _maxHealth = 1;
                    InvokeOnMaxHealthChanged();
                    return;
                }
                _health = MathExtensions.Remap(value, 0, _maxHealth, 0, value);
                _maxHealth = value;
                InvokeOnMaxHealthChanged();
            }
        }

        public bool Indestructible
        {
            get => _indestructible;
            set
            {
                _indestructible = value;
                InvokeOnInvincibilityChanged();
            }
        }
        
        // #############################################################################################################
        // Public Methods
        // #############################################################################################################
        public virtual void InterAct(Actor other)
        {
            if(InteractAble(other))
                ExecuteInteraction(other);
            
            if(other.InteractAble(this))
                other.ExecuteInteraction(this);
        }
        
        // #############################################################################################################
        // Protected/ Private Methods
        // #############################################################################################################

        /// <summary>
        /// This method checks whether the other has an impact on this
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual bool InteractAble(Actor other)
        {
            return this != other && (CollisionEnabled && IsCollideAble && other.CollisionEnabled && other.IsCollideAble) && Collide(other);
        }
        
        /// <summary>
        /// This method implements the effect of other on this
        /// </summary>
        /// <param name="other"></param>
        protected abstract void ExecuteInteraction(Actor other);

        protected virtual bool Collide(Actor other)
        {
            if ( this == other || 
                 Math.Abs(Layer - other.Layer) > float.Epsilon || 
                 !(CollisionEnabled && IsCollideAble && other.CollisionEnabled && other.IsCollideAble)) 
                return false;
            foreach (var polygon in HitBox)
            {
                foreach (var polygon1 in other.HitBox)
                {
                    if(polygon.Overlap(polygon1)) return true;
                }
            }
            return false;
        }
        
        protected virtual void ApproachDestination(Actor other, int maxIteration, int iteration=0)
        {
            if (iteration >= maxIteration)
            {
                _logger.Debug($"Approachdestination after {iteration} abborted");
                return;
            }
            if (DeltaPosition.Length() <= 10 * float.Epsilon)
            {
                var v = Position - other.Position;
                v /= v.Length();
                Position += 10 * v;
            }
            else
            {
                Position -= DeltaPosition;
                DeltaPosition /= 2;
                Position += DeltaPosition;
            }

            if (!Collide(other))
                return;
            iteration++;
            ApproachDestination(other, maxIteration, iteration);
        }

        #region EventInvoker

        protected void InvokeExplosion()
        {
            Explosion.Position = Position;
            Explosion.Layer = Layer;
            var explosionArgs = new LevelEventArgs.ExplosionEventArgs {Particle = Explosion};
            OnExplosion?.Invoke(this, explosionArgs);
        }

        protected virtual void InvokeDeath()
        {
            var e = GetOnDeadEventArgs();
            e.Aggressor = LastAggressor;
            e.Tool = Tool;
            e.Victim = this;
            OnDeath?.Invoke(this, e);
        }

        protected abstract LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs();

        private void InvokeOnHealthChanged()
        {
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }

        private void InvokeOnMaxHealthChanged()
        {
            OnMaxHealthChanged?.Invoke(this, EventArgs.Empty);
        }
        
        protected void InvokeOnFlippedChanged()
        {
            OnFlippedChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        private void InvokeOnInvincibilityChanged()
        {
            OnInvincibilityChanged?.Invoke(this, new LevelEventArgs.InvincibilityChangedEventArgs(){Target = this});
        }
    }
}