using System;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using SE_Praktikum.Services;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Models;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Extensions;

namespace SE_Praktikum.Components.Sprites
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
        
        [Obsolete]
        public Vector2? PixelPerfectCollide(Actor actor)
        {
            //actors can't collide with themselves
            if (this == actor) return null;
            if (!CollisionEnabled || !actor.CollisionEnabled) return null;
            if (Math.Abs(actor.Layer - Layer) > float.Epsilon ) return null;
            
            
            var t1 = _animationHandler.GetDataOfFrame();
            var t2 = actor._animationHandler.GetDataOfFrame();
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            var transformAToB = Transform * Matrix.Invert(actor.Transform);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            for (int yA = 0; yA < Rectangle.Height; yA++)
            {
                // Start at the beginning of the row
                var posInB = yPosInB;

                for (int xA = 0; xA < Rectangle.Width; xA++)
                {
                    // Round to the nearest pixel
                    var xB = (int)Math.Round(posInB.X);
                    var yB = (int)Math.Round(posInB.Y);

                    if (0 <= xB && xB < actor.Rectangle.Width &&
                        0 <= yB && yB < actor.Rectangle.Height)
                    {
                        // Get the colors of the overlapping pixels
                        var alphaA = t1[xA + yA * Rectangle.Width];
                        var alphaB = t2[xB + yB * actor.Rectangle.Width];

                        // If both pixel are not completely transparent
                        if (alphaA != 0 && alphaB != 0)
                        {
                            //InvokeOnCollide();
                            return new Vector2(xB,yB);
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return null;
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