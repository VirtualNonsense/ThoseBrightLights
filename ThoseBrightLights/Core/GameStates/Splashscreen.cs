using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
using ThoseBrightLights.Extensions;
using ThoseBrightLights.Services;
using ThoseBrightLights.Components;
using ThoseBrightLights.Models;
using ThoseBrightLights.Services.Factories;
using ThoseBrightLights.Services.StateMachines;

namespace ThoseBrightLights.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private Song _song;
        private Logger _logger;
        private IScreen _screen;
        private Sprite _teamName;
        private float _elapsedTime;
        private Sprite _gameEngineLogo;
        private int _splashscreenTime = 10;
        private readonly IGameEngine _engine;
        private ContentManager _contentManager;
        private AnimationHandlerFactory _factory;

        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        public Splashscreen(IGameEngine engine, IScreen _screen, ContentManager contentManager, AnimationHandlerFactory factory)
        {
            _engine = engine;
            _factory = factory;
            _logger = LogManager.GetCurrentClassLogger();
            this._screen = _screen;
            _contentManager = contentManager;
        }
        
        // #############################################################################################################
        // public methods
        // #############################################################################################################
        public override void LoadContent()
        {
            // loading monogamelogo
            var settings = new AnimationSettings(1, isPlaying: false, layer: 1,scale:0.2f);
            var tileset = new TileSet(_contentManager.Load<Texture2D>("MonoGame"));
            _gameEngineLogo = new Sprite(_factory.GetAnimationHandler(tileset,
                new List<AnimationSettings>(new[] {settings}),
                origin: new Vector2(tileset.TextureWidth / 2f, tileset.TextureHeight / 2f)));
            
            // loading teamlogo
            settings = new AnimationSettings(1, isPlaying: false, layer: 1);
            tileset = new TileSet(_contentManager.Load<Texture2D>("NWWP"));
            _teamName = new Sprite(_factory.GetAnimationHandler(tileset, new List<AnimationSettings>(new[] {settings}),
                origin: new Vector2(tileset.TextureWidth / 2f, tileset.TextureHeight / 2f)));
            _factory.GetAnimationHandler(tileset, new List<AnimationSettings>(new []{settings}),
                origin: new Vector2(tileset.TextureWidth / 2f, tileset.TextureHeight / 2f));

            _song = _contentManager.Load<Song>("Audio/Music/Intro_mp3");
            
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;
        }

        public override void UnloadContent()
        {
            _logger.Debug("unloading content");
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds/1000f;
            if(Keyboard.GetState().IsKeyDown(Keys.Escape) || _splashscreenTime < _elapsedTime)
            {
                _logger.Debug("exiting splashscreen");
                _subject.OnNext(GameStateMachine.GameStateMachineTrigger.SkipSplashScreen);
            }
            _screen.Camera.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            if(_elapsedTime>_splashscreenTime/2f)
            {
                _engine.Render(_teamName);
            }
            else
                _engine.Render(_gameEngineLogo);
        }
    }
}