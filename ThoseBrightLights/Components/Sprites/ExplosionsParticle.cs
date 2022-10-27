using System;
using Microsoft.Xna.Framework;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components.Sprites
{
    public class ExplosionsParticle : Particle
    {
        /// <summary>
        /// Simple class that represent an explosion
        /// It will remove itself when the animation finished playing
        /// </summary>
        /// <param name="animationHandler"></param>
        /// <param name="parent"></param>
        public ExplosionsParticle(AnimationHandler animationHandler, IScreen parent) : base(animationHandler)
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