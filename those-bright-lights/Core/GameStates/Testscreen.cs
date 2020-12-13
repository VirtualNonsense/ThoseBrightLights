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
        private readonly IGameEngine _engine;
        private IScreen _screen;
        private Logger _logger;
        public List<Actor> _actors;
        
        public Testscreen(IGameEngine engine, IScreen parent)
        {
            _engine = engine;
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
            foreach (Actor actor in _actors)
            {
                actor.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            foreach (Actor actor in _actors)
            {
                foreach (Actor collide in _actors)
                {
                    actor.Intersects(collide);
                }
            }
            
            var count = 0;
            while (count < _actors.Count)
            {
                if (_actors[count].IsRemoveAble)
                    _actors.RemoveAt(count);
                else
                    count++;
            }
        }

        public override void Draw()
        {
            _engine.Render(_actors);
        }
    }
}