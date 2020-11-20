using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.Weapons;

namespace SE_Praktikum.Services.Factories
{
    public class WeaponFactory
    {
        private AnimationHandlerFactory _animationHandlerFactory;
        
        public WeaponFactory()
        {
            
        }

        public Missile GetMissile(ContentManager contentManager, Vector2 direction)
        {
            throw new NotImplementedException();
        }
    }
}