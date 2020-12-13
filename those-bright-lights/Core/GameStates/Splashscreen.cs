using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NLog;
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
        private int _splashscreenTime = 100;
        private Song _song;
        private ContentManager _contentManager;
        private AnimationHandler _teamname;
        private AnimationHandlerFactory _factory;
        private AnimationHandler _gameengine;
        private Polygon _p;

        public Splashscreen(IScreen _screen, ContentManager contentManager, AnimationHandlerFactory factory)
        {
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

            _teamname = _factory.GetAnimationHandler(tileset, settings,origin:new Vector2(tileset.TextureWidth/2, tileset.TextureHeight/2));

            settings = new AnimationSettings(1, isPlaying: false, layer: 1,scale:0.2f);
            tileset = new TileSet(_contentManager.Load<Texture2D>("MonoGame"));
            _gameengine = _factory.GetAnimationHandler(tileset, settings, origin: new Vector2(tileset.TextureWidth / 2, tileset.TextureHeight / 2));
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
            _logger.Trace(_elapsedTime);
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            var effect = _screen.Camera.GetCameraEffectForPrimitives();
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList, _p.DrawAbleVertices, 0, _p.DrawAbleVertices.Length, _p.VertexDrawingOrder, 0, _p.TriangleCount);
            }
            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                              BlendState.Opaque,
                              SamplerState.PointClamp, // Sharp Pixel rendering
                              DepthStencilState.Default,
                              RasterizerState.CullCounterClockwise, // Render only the texture side that faces the camara to boost performance 
                              _screen.Camera.GetCameraEffect());
            
            if(_elapsedTime>_splashscreenTime/2)
            {
                _teamname.Draw(spriteBatch);
            }
            else
                _gameengine.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}