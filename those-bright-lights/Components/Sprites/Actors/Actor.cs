using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors
{
    public abstract class Actor : Sprite
    {

        private float _health;
        public bool CollisionEnabled = true;
        private Logger _logger;
        protected SoundEffect _impactSound;
        public Actor Parent;
        

        public Actor(AnimationHandler animationHandler, SoundEffect impactSound) : base(animationHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _impactSound = impactSound;
        }

        public bool IsCollideAble => HitBox != null;
        public Polygon[] HitBox => _animationHandler.CurrentHitBox;
        public float Damage { get; protected set; }
        public float Health {
            get=> _health;
            set
            {
                if (value <= 0)
                {
                    _health = 0;
                    IsRemoveAble = true;
                    return;
                }

                _health = value;
            }
        }
        protected bool _indestructible;
        protected Particle Explosion;


        #region Events
        public event EventHandler<EventArgs> OnExplosion;
        public event EventHandler<EventArgs> OnDeath;
        #endregion

        public void InterAct(Actor other)
        {
            if(InteractAble(other))
                ExecuteInteraction(other);
            
            if(other.InteractAble(this))
                other.ExecuteInteraction(this);
        }

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

        #endregion
    }
}