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

        public bool CollisionEnabled = true;
        private Logger _logger;
        protected SoundEffect _impactSound;
        

        public Actor(AnimationHandler animationHandler, SoundEffect impactSound) : base(animationHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _impactSound = impactSound;
        }

        private Rectangle? _hitbox = null;
        public Rectangle HitBox => _hitbox ?? Rectangle;
        protected float Damage;
        protected float Health { get; set; }
        protected bool _indestructible;
        protected Particle Explosion;


        #region Events
        public event EventHandler<EventArgs> OnCollide;
        public event EventHandler<EventArgs> OnExplosion; 
        #endregion


        public virtual void TakeDamage(Actor enemy)
        {
            if (_indestructible) return;
            Health -= enemy.Damage;
            _logger.Info(Health);
            if (!(Health <= 0)) return;
            IsRemoveAble = true;
            InvokeExplosion();
        }

        public bool RoughHitBoxCollision(Actor actor)
        {

            if (Rotation == 0 && actor.Rotation == 0)
                return HitBox.Intersects(actor.HitBox);
            var p = Position;
            var a = new Vector2((float)Math.Cos(Rotation) * HitBox.Width, (float)Math.Sin(Rotation) * HitBox.Width);
            var b = new Vector2((float)Math.Sin(Rotation) * HitBox.Height, (float)Math.Cos(Rotation) * HitBox.Height);

            var TR = p + a;
            var BL = p + b;
            var BR = p + a + b;
            
            var new_origin = new Vector2(Math.Min(Math.Min(p.X, TR.X), Math.Min(BL.X, BR.X)), Math.Min(Math.Min(p.Y, TR.Y), Math.Min(BL.Y, BR.Y)));
            var new_BottomRight = new Vector2(Math.Max(Math.Max(p.X, TR.X), Math.Max(BL.X, BR.X)), Math.Max(Math.Max(p.Y, TR.Y), Math.Max(BL.Y, BR.Y)));
            var new_div = new_BottomRight - new_origin;
            Rectangle t = new Rectangle((int)new_origin.X, (int)new_origin.Y, (int)new_div.X, (int)new_div.Y);
            
            p = actor.Position;
            a = new Vector2((float)Math.Cos(Rotation) * HitBox.Width, (float)Math.Sin(Rotation) * HitBox.Width);
            b = new Vector2((float)Math.Sin(Rotation) * HitBox.Height, (float)Math.Cos(Rotation) * HitBox.Height);
            
            TR = p + a;
            BL = p + b;
            BR = p + a + b;
            
            new_origin = new Vector2(Math.Min(Math.Min(p.X, TR.X), Math.Min(BL.X, BR.X)), Math.Min(Math.Min(p.Y, TR.Y), Math.Min(BL.Y, BR.Y)));
            new_BottomRight = new Vector2(Math.Max(Math.Max(p.X, TR.X), Math.Max(BL.X, BR.X)), Math.Max(Math.Max(p.Y, TR.Y), Math.Max(BL.Y, BR.Y)));
            new_div = new_BottomRight - new_origin;
            Rectangle t2 = new Rectangle((int)new_origin.X, (int)new_origin.Y, (int)new_div.X, (int)new_div.Y);
            return t.Intersects(t2);
        }
        
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
        protected virtual void InvokeOnCollide()
        {
            _impactSound?.Play();
            OnCollide?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void InvokeExplosion()
        {
            var explosionArgs = new LevelEvent.Explosion {Particle = Explosion};
            OnExplosion?.Invoke(this, explosionArgs);
        }
        #endregion
    }
}