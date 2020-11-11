using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites
{
    public class Particle : Sprite
    {
        private IScreen _parent;

        public Particle(AnimationHandler animationHandler, IScreen Parent) : base(animationHandler)
        {
            _parent = Parent;
        }
    }
}