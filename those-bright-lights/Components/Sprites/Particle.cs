using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components
{
    public abstract class Particle : Sprite
    {
        private IScreen _parent;

        public Particle(AnimationHandler animationHandler, IScreen Parent) : base(animationHandler)
        {
            _parent = Parent;
        }


        public abstract Particle Clone();
    }
}