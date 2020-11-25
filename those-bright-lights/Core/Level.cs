using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;

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
            foreach(var i in _components)
            {
                i.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(var i in _components)
            {
                i.Update(gameTime);
            }
            foreach(ICollideAble i in _components)
            {
                foreach(ICollideAble j in _components)
                {
                    if(i.Collides(j))
                    {

                    }
                    
                }
            }
        }

        
        public void OnNotify(LevelEvent levelEvent)
        {
            var t = (LevelEvent.ShootBullet)levelEvent;
            //if player or enemy shoots the ShootBullet event triggers
            if (t!=null)                                    
            {
                _components.Add(t.Bullet);
            }
        }
    }
}