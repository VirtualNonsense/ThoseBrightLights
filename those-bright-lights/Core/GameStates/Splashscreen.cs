using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SE_Praktikum.Models;
using SE_Praktikum.Services.ParticleEmitter;
using Newtonsoft.Json;
using System.IO;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private TileMap _tilemap;
        private IScreen _screen;
        private readonly ExplosionEmitter _explosionEmitter;
        public Song _song;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter)
        {
            _screen = parent;
            _explosionEmitter = explosionEmitter;
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            _song = contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;
        }

        public override void UnloadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //explosionEmitter.Update(gameTime);
        }

        public override void PostUpdate()
        {
            //throw new System.NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            //_explosionEmitter.Draw(gameTime, spriteBatch);
            //spriteBatch.Draw(_tilemap.texture, _tilemap.Frame(1, 0), Color.White);
            spriteBatch.End();

        }
    }
}