using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites
{
    public class Particle : Sprite
    {
        
        public Particle(Texture2D texture, IScreen Parent) : base(texture)
        {
            _parent = Parent;
            Origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        }

        public Particle(AnimationHandler animationHandler, IScreen Parent) : base(animationHandler)
        {
            _parent = Parent;
        }
    }
}