using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Extensions;

namespace SE_Praktikum.Components.Sprites.Actors
{
    public abstract class Actor : Sprite
    {

        protected bool _indestructible;
        protected Particle Explosion;
        private float _health;
        private float _maxHealth;
        public bool CollisionEnabled = true;
        private Logger _logger;
        protected SoundEffect _impactSound;
        
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
        public Actor(AnimationHandler animationHandler, SoundEffect impactSound, float health = 100, float maxHealth = 100) : base(animationHandler)
        {
            MaxHealth = maxHealth;
            Health = health;
            _logger = LogManager.GetCurrentClassLogger();
            _impactSound = impactSound;
        }
        
        // #############################################################################################################
        // Events
        // #############################################################################################################
        #region Events
        public event EventHandler<EventArgs> OnExplosion;
        public event EventHandler<EventArgs> OnDeath;
        public event EventHandler OnHealthChanged;
        public event EventHandler OnMaxHealthChanged;
        public event EventHandler OnFlippedChange;
        
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
            set => _indestructible = value;
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

        protected virtual void InvokeExplosion()
        {
            var explosionArgs = new LevelEvent.Explosion {Particle = Explosion};
            OnExplosion?.Invoke(this, explosionArgs);
        }

        protected virtual void InvokeDeath()
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void InvokeOnHealthChanged()
        {
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void InvokeOnMaxHealthChanged()
        {
            OnMaxHealthChanged?.Invoke(this, EventArgs.Empty);
        }
        
        protected virtual void InvokeOnFlippedChange()
        {
            OnFlippedChange?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}