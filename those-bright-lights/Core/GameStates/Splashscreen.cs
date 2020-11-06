using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SE_Praktikum.Models;
using SE_Praktikum.Services.ParticleEmitter;
using Newtonsoft.Json;
using System.IO;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Components;
using SE_Praktikum.Models.Tiled;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        
        private IScreen _screen;
        private readonly ExplosionEmitter _explosionEmitter;
        private readonly MapFactory mapfactory;
        public Song _song;
        private Map map;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter, MapFactory mapfactory)
        {
            _screen = parent;
            _explosionEmitter = explosionEmitter;
            this.mapfactory = mapfactory;
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            _song = contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;
            var t = JsonConvert.DeserializeObject<LevelBlueprint>(File.ReadAllText(@".\Content\Level\TestMap_3_3.json")); ;
            map = mapfactory.LoadMap(contentManager, t);
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
            map.Draw(gameTime, spriteBatch);
            spriteBatch.End();

        }
    }
}