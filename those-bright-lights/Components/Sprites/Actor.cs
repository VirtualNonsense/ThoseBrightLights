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

namespace SE_Praktikum.Components.Sprites
{
    public abstract class Actor : Sprite
    {

        public bool CollisionEnabled = true;
        private Logger _logger;
        

        public Actor(AnimationHandler animationHandler) : base(animationHandler)
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        private Rectangle? _hitbox = null;
        public Rectangle HitBox => _hitbox ?? Rectangle;
        protected float Damage;
        protected float Health { get; set; }
        private bool _indestructible;
        protected Particle Explosion;


        #region Events
        public event EventHandler<EventArgs> OnCollide;
        public event EventHandler<EventArgs> OnExplosion; 
        #endregion




        public Vector2? Intersects(Actor actor)
        {
            //actors can't collide with themselves
            if (this == actor) return null;
            if (!CollisionEnabled || !actor.CollisionEnabled) return null;
            if (Math.Abs(actor.Layer - Layer) > float.Epsilon ) return null;
            //if actors are not rotated, use faster method Intersects of Rectangle for now, 
            //-> no exact value for the collision position
            //TODO: doesn't work currently
            if (Rotation == 0 && actor.Rotation == 0)
            {
                if (!HitBox.Intersects(actor.HitBox))
                    return null;
            }
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
                            InvokeOnCollide();
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