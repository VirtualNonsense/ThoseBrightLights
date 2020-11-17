using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Components.Sprites.SplashScreen;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.ParticleEmitter;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private IScreen _screen;
        private readonly ExplosionEmitter _explosionEmitter;
        public Song _song;
        public List<Actor> List;
        private Logger _logger;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter)
        {
            _screen = parent;
            _explosionEmitter = explosionEmitter;
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            // var  p = new TileSet(contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"),45,45, 1);
            // _explosionEmitter.TileSet = p;
            // _explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);

            // _song = contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            // MediaPlayer.Play(_song);
            // MediaPlayer.IsRepeating = true;
            
            CollisionCube p = new CollisionCube(
                new AnimationHandler(
                    new TileSet(contentManager.Load<Texture2D>("Artwork/effects/explosion_45_45"),45,45),
                    new AnimationSettings(7)),
                new Input(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Add, Keys.Subtract ),
                _screen);

            p.OnCollide += (sender, args) => { _logger.Warn("p collide"); };
            
            CollisionCube q = new CollisionCube(
                new AnimationHandler(
                    new TileSet(contentManager.Load<Texture2D>("Artwork/effects/explosion_45_45"),45,45),
                    new AnimationSettings(7)),
                new Input(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Q, Keys.E),
                _screen);
            List = new List<Actor> {p, q};
            
            q.OnCollide += (sender, args) => { _logger.Warn("q collide"); };

        }

        public override void UnloadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            // _explosionEmitter.Update(gameTime);
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
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            foreach(Actor actor in List) actor.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}