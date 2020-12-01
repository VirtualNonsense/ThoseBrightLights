using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
{
    public class Missile : Bullet
    {
        public Missile(AnimationHandler animationHandler) : base(animationHandler)
        {
        }

        public override void BaseCollide(Actor actor)
        {
            throw new System.NotImplementedException();
        }
    }
}