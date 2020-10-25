using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Models;
using SE_Praktikum.Services.ParticleEmitter;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private IScreen _screen;
        private readonly ExplosionEmitter _explosionEmitter;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter)
        {
            _screen = parent;
            _explosionEmitter = explosionEmitter;
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            var p = new Animation(contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"), 7);
            _explosionEmitter.Animation = p;
            //_explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);

        }

        public override void UnloadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            _explosionEmitter.Update(gameTime);
        }

        public override void PostUpdate()
        {
            //throw new System.NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _explosionEmitter.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}