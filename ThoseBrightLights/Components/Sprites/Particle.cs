using ThoseBrightLights.Services;

namespace ThoseBrightLights.Components
{
    public abstract class Particle : Sprite
    {
        /// <summary>
        /// simple visual effect class
        /// use this to draw e.g. explosion particle etc.
        /// </summary>
        /// <param name="animationHandler"></param>
        protected Particle(AnimationHandler animationHandler) : base(animationHandler)
        {
        }


        public abstract Particle Clone();
    }
}