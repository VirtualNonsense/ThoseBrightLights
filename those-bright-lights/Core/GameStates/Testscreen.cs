using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Core.GameStates
{
    public class Testscreen : GameState
    {
        private IScreen _screen;
        private Logger _logger;
        public List<Actor> List;
        
        public Testscreen(IScreen parent)
        {
            _screen = parent;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public override void LoadContent()
        {
        }

        public override void UnloadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Actor actor in List)
            {
                actor.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            foreach (Actor actor in List)
            {
                foreach (Actor collide in List)
                {
                    actor.Intersects(collide);
                }
            }
            
            var count = 0;
            while (count < List.Count)
            {
                if (List[count].IsRemoveAble)
                    List.RemoveAt(count);
                else
                    count++;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            foreach(Actor actor in List) actor.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}