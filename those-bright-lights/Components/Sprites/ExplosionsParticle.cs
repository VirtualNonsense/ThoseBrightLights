using System;
using Microsoft.Xna.Framework;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

// TODO: Move namespace when everybody finished commenting
namespace SE_Praktikum.Components.Actors
{
    public class ExplosionsParticle : Particle
    {
        /// <summary>
        /// Simple class that represent an explosion
        /// It will remove itself when the animation finished playing
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="Parent"></param>
        public ExplosionsParticle(AnimationHandler animationHandler, IScreen Parent) : base(animationHandler, Parent)
        {
            _animationHandler.OnAnimationComplete += AnimationHandlerOnOnAnimationComplete;
        }

        private void AnimationHandlerOnOnAnimationComplete(object sender, EventArgs e)
        {
            IsRemoveAble = true;
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity;
            base.Update(gameTime);
        }

        public override Particle Clone()
        {
            return MemberwiseClone() as Particle;
        }
    }
}