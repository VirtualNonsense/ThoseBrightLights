using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites;

namespace SE_Praktikum.Core
{
    public class Level : IComponent
    {
        private List<IComponent> _components;

        
        //Constructor
        public Level(List<IComponent> components)
        {
            _components = components;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}