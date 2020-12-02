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

        public override void LoadContent(ContentManager contentManager)
        {
            Player p = new Player(
                new AnimationHandler(
                    new TileSet(contentManager.Load<Texture2D>("Artwork/actors/spaceship")), new AnimationSettings(1)),
                new Input(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Q, Keys.E, Keys.Space ),
                100,1);
            
            Player q = new Player(
                new AnimationHandler(
                    new TileSet(contentManager.Load<Texture2D>("Artwork/actors/spaceship")),
                    new AnimationSettings(1)),
                new Input(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Add, Keys.Subtract, Keys.Enter ),100,1);
            q.Position = new Vector2(200,200);

            List = new List<Actor> {p, q};
            
            //p.OnCollide += (sender, args) => { _logger.Warn("p collide"); };
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