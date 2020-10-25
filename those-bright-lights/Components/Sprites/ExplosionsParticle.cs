using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites
{
    public class ExplosionsParticle : Particle
    {
        public ExplosionsParticle(Texture2D texture, IScreen Parent) : base(texture, Parent)
        {
        }

        public ExplosionsParticle(AnimationHandler animationHandler, IScreen Parent) : base(animationHandler, Parent)
        {
            _animationHandler.OnAnimationComplete += AnimationHandlerOnOnAnimationComplete;
        }

        private void AnimationHandlerOnOnAnimationComplete(object? sender, EventArgs e)
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
    }
}