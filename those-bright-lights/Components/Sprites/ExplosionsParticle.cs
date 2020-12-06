using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites
{
    public class ExplosionsParticle : Particle
    {
        private readonly IScreen _parent;

        public ExplosionsParticle(AnimationHandler animationHandler, IScreen Parent) : base(animationHandler, Parent)
        {
            _animationHandler.OnAnimationComplete += AnimationHandlerOnOnAnimationComplete;
            _parent = Parent;
        }

        private void AnimationHandlerOnOnAnimationComplete(object sender, EventArgs e)
        {
            IsRemoveAble = true;
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity;
            if (Position.Y > (_parent.ScreenHeight + _animationHandler?.FrameHeight))
                IsRemoveAble = true;
            base.Update(gameTime);
        }

        public override Particle Clone()
        {
            return this.MemberwiseClone() as Particle;
        }
    }
}