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
            _tilemap = new TileMap(contentManager.Load<Texture2D>("Artwork/Tilemaps/space2_4-frames"), 4, 1);
            var p = new Animation(contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"), 7);
            _explosionEmitter.Animation = p;
            //_explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);
            LevelBlueprint test = JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@"C:\Users\wenk-\Desktop\5.Semester\Software Engineering\those-bright-lights\those-bright-lights\Content\Level\32x32_TestMap.json"));
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
            _explosionEmitter.Update(gameTime);
        }

        public override void PostUpdate()
        {
            //throw new System.NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            _explosionEmitter.Draw(gameTime, spriteBatch);
           //spriteBatch.Draw(_tilemap.texture, _tilemap.Frame(1, 0), Color.White);
            spriteBatch.End();

        }
    }
}