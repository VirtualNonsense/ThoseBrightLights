using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
using SE_Praktikum.Components.Sprites;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private Logger _logger;
        private IScreen _screen;
        private float _elapsedTime = 0;
        private int _splashscreenTime = 10;
        private Song _song;
        private ContentManager _contentManager;
        private Sprite _teamName;
        private readonly IGameEngine _engine;
        private AnimationHandlerFactory _factory;
        private Sprite _gameEngineLogo;
        private Polygon _p;

        public Splashscreen(IGameEngine engine, IScreen _screen, ContentManager contentManager, AnimationHandlerFactory factory)
        {
            _engine = engine;
            _factory = factory;
            _logger = LogManager.GetCurrentClassLogger();
            this._screen = _screen;
            _contentManager = contentManager;
        }
        
        public override void LoadContent()
        {

            var settings = new AnimationSettings(1, isPlaying: false, layer: 1);

            var tileset = new TileSet(_contentManager.Load<Texture2D>("NWWP"));
            _p = new Polygon(Vector2.Zero, Vector2.Zero, 0, new List<Vector2>
            {
                new Vector2(-1 * 100, 1* 100),
                new Vector2(1* 100, 1* 100),
                new Vector2(1* 100, -1* 100),
                new Vector2(-1* 100, -1* 100),
            });

            _p.Origin = _p.Vertices[0];

            _teamName = new Sprite(_factory.GetAnimationHandler(tileset, settings,origin:new Vector2(tileset.TextureWidth/2f, tileset.TextureHeight/2f)));_factory.GetAnimationHandler(tileset, settings,origin:new Vector2(tileset.TextureWidth/2, tileset.TextureHeight/2));

            settings = new AnimationSettings(1, isPlaying: false, layer: 1,scale:0.2f);
            tileset = new TileSet(_contentManager.Load<Texture2D>("MonoGame"));
            _gameEngineLogo = new Sprite(_factory.GetAnimationHandler(tileset, settings, origin: new Vector2(tileset.TextureWidth / 2f, tileset.TextureHeight / 2f)));
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
            _p.Rotation += MathExtensions.DegToRad(1);
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            _engine.Render(_p);
            if(_elapsedTime>_splashscreenTime/2f)
            {
                _engine.Render(_teamName);
            }
            else
                _engine.Render(_gameEngineLogo);
        }
    }
}