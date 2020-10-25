using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;

namespace SE_Praktikum.Components.Sprites
{
    public class Particle : Sprite
    {
        public Vector2 Velocity;
        
        public Particle(Texture2D texture, IScreen Parent) : base(texture)
        {
            _parent = Parent;
            Origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        }

        public Particle(Dictionary<string, Animation> animations, IScreen Parent) : base(animations)
        {
            Position += Velocity;
            _parent = Parent;
            if (Position.Y > (Parent.ScreenHeight + _texture.Height))
                IsRemoveAble = true;
        }
    }
}