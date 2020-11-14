using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
using SE_Praktikum.Models;
using SE_Praktikum.Services.ParticleEmitter;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private IScreen _screen;
        private int _splashscreenTime = 60;
        private readonly ExplosionEmitter _explosionEmitter;
        public Song _song;
        private Logger _logger;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _screen = parent;
            _explosionEmitter = explosionEmitter;
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            var p = new Animation(contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"), 7);
            _explosionEmitter.Animation = p;
            //_explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);

            //_song = contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            //MediaPlayer.Play(_song);
            //MediaPlayer.IsRepeating = true;
        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
        }

        public override void Update(GameTime gameTime)
        {
            _explosionEmitter.Update(gameTime);
            if(Keyboard.GetState().IsKeyDown(Keys.Space) || _splashscreenTime < gameTime.ElapsedGameTime.Seconds)
            {
                _logger.Debug("exiting splashscreen");
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SkipSplashScreen);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            _explosionEmitter.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}