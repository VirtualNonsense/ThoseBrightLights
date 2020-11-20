using Microsoft.Xna.Framework;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class Bullet : Actor
    {
        protected float Speed;
        protected Vector2 Direction;
        protected float Damage;
        protected Vector2 Position;



        protected Bullet(AnimationHandler animationHandler) : base(animationHandler)
        {
            
        }

        public override void BaseCollide(Actor actor)
        {
            throw new System.NotImplementedException();
        }
    }
}